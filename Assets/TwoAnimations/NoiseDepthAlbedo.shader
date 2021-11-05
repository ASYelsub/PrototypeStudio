Shader "Custom/Noise Depth Albedo"
{
     Properties 
    {
        _albedo ("albedo", 2D) = "white" {}
         [NoScaleOffset] _normalMap ("normal map", 2D) = "bump" {}
        _surfaceColor ("surface color", Color) = (0.4, 0.1, 0.9)
        _grainColor ("grain color", Color) = (0,0,0,0)
        _diffuseSteps ("diffuse steps", Int) = 4
        _noisePattern("noise pattern", Range(1,100)) = 10
        _normalIntensity ("normal intensity", Range(0, 1)) = 1
        _noiseIntensity("noise intensity",Range(1,20)) = 2
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

            
            float3 _surfaceColor;
            float3 _grainColor;
            int _diffuseSteps;
            float _noisePattern;
            sampler2D _albedo; float4 _albedo_ST;
            sampler2D _normalMap;
            float _normalIntensity;
            float _noiseIntensity;

            float white_noise (float2 value) {
                float uv = value;
                uv = floor(uv*600); //find the number for like it to not flicker
                float wn = 0;
                float samp = dot(uv,float2(128.239,-78.382));
                
                wn = frac(sin(samp)*90321);
                return wn;
            }

            float rand (float2 uv) {
                return frac(sin(dot(uv.xy, float2(12.9898, 78.233))) * 43758.5453123);
            }

            float value_noise (float2 uv) {
                float2 ipos = floor(uv);
                float2 fpos = frac(uv); 
                
                float o  = rand(ipos);
                float x  = rand(ipos + float2(1, 0));
                float y  = rand(ipos + float2(0, 1));
                float xy = rand(ipos + float2(1, 1));

                float2 smooth = smoothstep(0, 1, fpos);
                return lerp( lerp(o,  x, smooth.x), 
                             lerp(y, xy, smooth.x), smooth.y);
            }
            
            struct MeshData
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 uv : TEXCOORD0;
                float3 noise : TEXCOORD1;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 posWorld : TEXCOORD2; 
                float3 tangent : TEXCOORD3;
                float3 bitangent : TEXCOORD4;
                float3 noise : TEXCOORD5;
                 
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
                o.noise = v.noise;
                
                

                return o;
            }

            float4 frag (Interpolators i) : SV_Target
            {   
                float2 uv = i.uv;
                float3 wn = white_noise(value_noise(i.noise*_noisePattern));
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
                
                
                

                
                float grainFalloff = saturate(1-diffuseFalloff);
                
                

                float dSteps = max(2,_diffuseSteps);
                diffuseFalloff = floor(diffuseFalloff * dSteps)/dSteps;
                grainFalloff = floor(grainFalloff * dSteps)/dSteps;

                
                float3 grainColor = _grainColor;
                float3 grain = saturate((grainFalloff*wn)*grainColor);

                float3 diffuseColor = _surfaceColor*surfaceColor;
                
                float3 diffuse = saturate(diffuseFalloff*diffuseColor) + saturate((grainFalloff)*(1-wn)*diffuseColor);


                

                color = (diffuse+grain);

                return float4(color, 1.0);
            }
            ENDCG
        }
    }
}
