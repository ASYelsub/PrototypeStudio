Shader "Custom/Cubemap Skybox"
{
    Properties 
    {
        [NoScaleOffset] _texCube ("cube map", Cube) = "black" {}
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

            struct MeshData
            {
                float4 vertex : POSITION;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float3 objPos : TEXCOORD0;
            };

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.objPos = v.vertex.xyz;

                return o;
            }

            float4 frag (Interpolators i) : SV_Target
            {
                float3 color = 0;
                float3 sampleVec = normalize(i.objPos);
                

                color = texCUBElod(_texCube, float4(sampleVec,0)); //0 is the highest resolution mip level


                return float4(color, 1.0);
            }
            ENDCG
        }
    }
}
