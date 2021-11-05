Shader "Custom/Cubemap Skybox"
{
    Properties 
    {
        [NoScaleOffset] _texCube ("cube map", Cube) = "black" {}
        _noisePattern("noise pattern", Range(1,100)) = 10
        _goghColor("gogh color", Color) = (0,0,0,0)
        _cloudSpeed("cloud speed", Range(.01,.2)) = .1
    }

    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            samplerCUBE _texCube;
            float _noisePattern;
            float3 _goghColor;
            float _cloudSpeed;

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
                float3 noise : TEXCOORD0;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float3 objPos : TEXCOORD0;
                float3 noise : TEXCOORD1;
            };

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.objPos = v.vertex.xyz;
                o.noise = v.noise;

                return o;
            }

            float4 frag (Interpolators i) : SV_Target
            {

                float3 wn = white_noise(value_noise(i.noise*_noisePattern+(_Time.z*_cloudSpeed)));
                float3 color = 0;
                float3 sampleVec = normalize(i.objPos);
                

                color = texCUBElod(_texCube, float4(sampleVec,0))*(1-wn); 
                float3 goghColor = wn*_goghColor;
                float3 val = color+ goghColor;

                return float4(val, 1.0);
            }
            ENDCG
        }
    }
}
