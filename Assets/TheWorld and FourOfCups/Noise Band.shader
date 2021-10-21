Shader "Custom/Noise Band"
{
     Properties 
    {
        _surfaceColor ("surface color", Color) = (0.4, 0.1, 0.9)
        _ambientColor ("ambient color", Color) = (0,0,0,0)
        _ambientIntensity("ambient intensity", Range(0,6)) = 0.1
        _gloss ("gloss", Range(0,1)) = 1
        _diffuseSteps ("diffuse steps", Int) = 4
        _specularSteps("specular steps", Int) = 4
        _noisePattern("noise pattern", Range(1,500)) = 10
    }
    SubShader
    {
        // this tag is required to use _LightColor0
        Tags { "LightMode"="ForwardBase" }

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
            float3 _ambientColor;
            float _ambientIntensity;
            float _gloss;
            int _diffuseSteps;
            int _specularSteps;
            float _noisePattern;


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
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD0;
                float3 posWorld : TEXCOORD1; 
            };

            Interpolators vert (MeshData v)
            {
                Interpolators o;
               
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                

                return o;
            }

            float4 frag (Interpolators i) : SV_Target
            {   
                float wn = white_noise(i.vertex/_noisePattern);
                //float wn = fractal_noise(i.vertex*_noisePattern); //this one interacts strangely with the falloff
                
                float3 color = 0;

                float3 normal = normalize(i.normal);
                
                float3 lightDirection = _WorldSpaceLightPos0;
                float3 lightColor = _LightColor0; // includes intensity

                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld);
                float3 halfDirection = normalize(viewDirection + lightDirection);

                float diffuseFalloff = max(0, dot(normal, lightDirection));
                
                float specularFalloff = max(0, dot(normal, halfDirection));
                specularFalloff = pow(specularFalloff, _gloss * MAX_SPECULAR_POWER + 0.0001) * _gloss;
                

                float3 ambientColor = _ambientColor;
               float ambientAntiFalloff = saturate(1-diffuseFalloff);
                
                float dSteps = max(2,_diffuseSteps);
                float sSteps = max(1,_specularSteps);
                diffuseFalloff = floor(diffuseFalloff * dSteps) / dSteps;
                ambientAntiFalloff = floor(ambientAntiFalloff * dSteps)/dSteps;
                



                specularFalloff = floor(specularFalloff * sSteps) / sSteps;
                float3 surfaceColor = _surfaceColor;
                float3 diffuse = diffuseFalloff * surfaceColor * lightColor;
                float3 specular = specularFalloff * lightColor;
                float3 ambient = ambientAntiFalloff * surfaceColor * lightColor * ambientColor*_ambientIntensity*wn;

                
                ambient = saturate(ambient);
                diffuse = saturate(diffuse);
            


                color = (diffuse + specular + ambient);

                return float4(color, 1.0);
            }
            ENDCG
        }
    }
}
