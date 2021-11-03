Shader "Custom/Noise Depth Albedo"
{
     Properties 
    {
        _albedo ("albedo", 2D) = "white" {}
         [NoScaleOffset] _normalMap ("normal map", 2D) = "bump" {}
        _surfaceColor ("surface color", Color) = (0.4, 0.1, 0.9)
        _grainColor ("grain color", Color) = (0,0,0,0)
        _ambientColor ("ambient color", Color) = (1,1,1,1)
        _ambientIn("ambient intensity", Range(0,1)) = .1
        _gloss ("gloss", Range(0,1)) = 1
        _diffuseSteps ("diffuse steps", Int) = 4
        _specularSteps("specular steps", Int) = 4
        _noisePattern("noise pattern", Range(1,500)) = 10
        _normalIntensity ("normal intensity", Range(0, 1)) = 1
    }
    SubShader
    {
        // this tag is required to use _LightColor0
        Tags { "Queue" = "Transparent" "LightMode"="ForwardBase" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // might be UnityLightingCommon.cginc for later versions of unity
            #include "Lighting.cginc"

            #define MAX_SPECULAR_POWER 256
            
            float3 _surfaceColor;
            float3 _grainColor;
            float3 _ambientColor;
            float _ambientIn;
            float _gloss;
            int _diffuseSteps;
            int _specularSteps;
            float _noisePattern;
            sampler2D _albedo; float4 _albedo_ST;
            sampler2D _normalMap;
            float _normalIntensity;

             float white_noise (float2 value) {
                return frac(sin(dot(value, float2(128.239, -78.381))) * 90321);
            }

             float rand (float2 uv) {
                return frac(sin(dot(uv.xy, float2(12.9898, 78.233))) * 43758.5453123);
            }
            
            struct MeshData
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 uv : TEXCOORD0;
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
                float wn = saturate(white_noise(i.vertex/_noisePattern));
                //float wn = fractal_noise(i.vertex*_noisePattern); //this one interacts strangely with the falloff
                float3 surfaceColor = lerp(0, tex2D(_albedo, uv).rgb, 1);
                float3 color = 0;

                float3 tangentSpaceNormal = UnpackNormal(tex2D(_normalMap, uv));
                tangentSpaceNormal = normalize(lerp(float3(0, 0, 1), tangentSpaceNormal, _normalIntensity));
                
                float3x3 tangentToWorld = float3x3 
                (
                    i.tangent.x, i.bitangent.x, i.normal.x,
                    i.tangent.y, i.bitangent.y, i.normal.y,
                    i.tangent.z, i.bitangent.z, i.normal.z
                );

                float3 normal = mul(tangentToWorld, tangentSpaceNormal);
                
                float3 lightDirection = _WorldSpaceLightPos0;
                float3 lightColor = _LightColor0; // includes intensity

                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld);
                float3 halfDirection = normalize(viewDirection + lightDirection);

                float diffuseFalloff = max(0, dot(normal, lightDirection)); //add wn to normal to blur the change
                
                float specularFalloff = max(0, dot(normal, halfDirection));
                specularFalloff = pow(specularFalloff, _gloss * MAX_SPECULAR_POWER + 0.0001) * _gloss;
                

                float3 grainColor = _grainColor;
                float grainFalloff = saturate(1-diffuseFalloff);
                
                float nwn = saturate(1-wn);

                float dSteps = max(2,_diffuseSteps);
                float sSteps = max(1,_specularSteps);
                diffuseFalloff = floor(diffuseFalloff * dSteps)/dSteps;
                grainFalloff = floor(grainFalloff * dSteps)/dSteps;

                float3 grain = grainFalloff*wn;
                grain = saturate(grain) * grainColor*lightColor;

                float3 diffuse = diffuseFalloff;
                diffuse = saturate(diffuse) * surfaceColor * _surfaceColor * lightColor;


                specularFalloff = floor(specularFalloff * sSteps) / sSteps;
                float3 specular = specularFalloff * lightColor;
                float3 ambient = _ambientColor*_ambientIn;
                

                color = (diffuse+grain+specular+ambient);

                return float4(color, 1.0);
            }
            ENDCG
        }
    }
}
