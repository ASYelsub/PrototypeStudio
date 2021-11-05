Shader "Custom/Leaf Shader"
{
    Properties
    {
         _albedo ("albedo", 2D) = "white" {}
         _alpha ("alpha", 2D) = "white" {}
         _normalMap ("normal map", 2D) = "bump" {}
         _normalIntensity ("normal intensity", Range(0, 1)) = 1
         _gloss ("gloss", Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "IgnoreProjector" = "True"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Lighting.cginc"
            uniform sampler2D _albedo; float4 _albedo_ST;
            uniform sampler2D _alpha;
            sampler2D _normalMap;
            float _normalIntensity;
            float _gloss;

            #define MAX_SPECULAR_POWER 256
            
            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };
            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 posWorld : TEXCOORD2; 
                float3 tangent : TEXCOORD3;
                float3 bitangent : TEXCOORD4;
            };
            Interpolators vert (MeshData v)
            {
                Interpolators o;
                 o.uv = TRANSFORM_TEX(v.uv, _albedo);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.tangent = UnityObjectToWorldNormal(v.tangent);
                o.bitangent = cross(o.normal, o.tangent) * v.tangent.w;
                
                return o;
            }
            float4 frag (Interpolators i) : SV_Target
            {
                float2 uv = i.uv;


                float3 tangentSpaceNormal = UnpackNormal(tex2D(_normalMap, uv));
                tangentSpaceNormal = normalize(lerp(float3(0, 0, 1), tangentSpaceNormal, _normalIntensity));

                float3x3 tangentToWorld = float3x3 
                (
                    i.tangent.x, i.bitangent.x, i.normal.x,
                    i.tangent.y, i.bitangent.y, i.normal.y,
                    i.tangent.z, i.bitangent.z, i.normal.z
                );

                float3 normal = mul(tangentToWorld, tangentSpaceNormal);
                float3 albedo = tex2D(_albedo, uv).rgb;
                float3 alpha = tex2D(_alpha, uv).rgb;
                float3 color = 0;


                float3 lightDirection = _WorldSpaceLightPos0;
                float3 lightColor = _LightColor0; // includes intensity

                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld);
                float3 halfDirection = normalize(viewDirection + lightDirection);


                float diffuseFalloff = max(0, dot(normal, lightDirection)); //add wn to normal to blur the change
                
                float specularFalloff = max(0, dot(normal, halfDirection));
                specularFalloff = pow(specularFalloff, _gloss * MAX_SPECULAR_POWER + 0.0001) * _gloss;

                 float3 diffuse = diffuseFalloff;
                diffuse = saturate(diffuse) * albedo;

                float3 specular = specularFalloff;


                color = specular + diffuse;

                return float4(color, alpha.r);
            }
            ENDCG
        }
    }
}