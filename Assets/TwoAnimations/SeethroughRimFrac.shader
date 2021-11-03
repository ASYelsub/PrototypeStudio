// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Seethrough Rim Frac" {
    Properties {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _fresnelPower("fresnel power", Range(0,10)) = 5
        _diffuseSteps ("diffuse steps", Int) = 4
        
    }
    SubShader 
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Lighting.cginc"

            #define MAX_SPECULAR_POWER 256

            float4 _Color;
            float _fresnelPower;
           int _diffuseSteps;

            struct MeshData
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };
            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.uv = v.uv;
                o.normal = v.normal;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld,v.vertex);
                return o;
            }

            float4 frag (Interpolators i) : SV_Target
            {
                float3 color = 0;
                float2 uv = i.uv;
                float3 normal = normalize(i.normal);

                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.worldPos);

                float3 fresnel = saturate(dot(viewDirection, normal));

                fresnel = pow(fresnel, _fresnelPower);

                float3 fresnelColor =  _Color;
                
                float4 value = float4(fresnelColor.rgb, saturate(1-fresnel.r));





                value = float4(value.rgb, value.a);
                return value;
            }
            ENDCG
        } 
    Pass
    {
     
    Cull Front
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "Lighting.cginc"

        #define MAX_SPECULAR_POWER 256

        float4 _Color;
        float _fresnelPower;

        struct MeshData
        {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float2 uv : TEXCOORD0;
        };
        struct Interpolators
        {
            float4 vertex : SV_POSITION;
            float2 uv : TEXCOORD0;
            float3 normal : TEXCOORD1;
            float3 worldPos : TEXCOORD2;
        };

         Interpolators vert (MeshData v)
        {
            Interpolators o;
            o.uv = v.uv;
            o.normal = v.normal;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.worldPos = mul(unity_ObjectToWorld,v.vertex);
            return o;
        }

        float4 frag (Interpolators i) : SV_Target
        {
            float3 color = 0;
            float2 uv = i.uv;
            float3 normal = normalize(i.normal);

            float3 viewDirection = normalize(1-((_WorldSpaceCameraPos.xyz) - i.worldPos));

            float3 fresnel = saturate(dot(viewDirection, normal));

            fresnel = pow(fresnel, _fresnelPower);

            float3 fresnelColor =  _Color;
            
            float4 value = float4(fresnelColor.rgb, saturate(1-fresnel.r));
            
            return value;
        }
    ENDCG
    }
        
    }
}