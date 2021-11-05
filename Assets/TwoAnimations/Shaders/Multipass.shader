Shader "AdvancedShaderDevelopment/MultiPassShader"
{
    Properties
    {
        _FirstColor("First Color", Color)=(1,0,0,1)
        _SecondColor("Second Color", Color)=(0,1,0,1)
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            fixed4 _FirstColor;
            struct appdata
            {
                float4 vertex:POSITION;
            };
            struct v2f
            {
                float4 clipPos:SV_POSITION;
            };
            v2f vert (appdata v)
            {
                v2f o;
                v.vertex-=float4(1.5,0,0,0);
                o.clipPos=UnityObjectToClipPos(v.vertex);
                return o;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                return _FirstColor;
            }
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            fixed4 _SecondColor;
            struct appdata
            {
                float4 vertex:POSITION;
            };
            struct v2f
            {
                float4 clipPos:SV_POSITION;
            };
            v2f vert (appdata v)
            {
                v2f o;
                v.vertex+=float4(1.5,0,0,0);
                o.clipPos=UnityObjectToClipPos(v.vertex);
                return o;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                return _SecondColor;
            }
            ENDCG
        }
    }
}