Shader "Unlit/DepthFog"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FogColor ("Fog Color", Color) = (1,1,1,1)
        _FogIntensity ("Fog Intensity",  Range (0, 1)) = .5
    }

    SubShader
    {
        Cull off 
        ZWrite Off 
        ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #define PI 3.14159
            sampler2D _CameraDepthTexture;
            float4 _FogColor;
            float _FogIntensity;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 scrPos : TEXCOORD1;
            };           

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.scrPos = ComputeScreenPos(o.vertex);
                o.uv = v.uv;

                return o;
            }

            sampler2D _MainTex;
            
            float4 easeOutBack(float4 x) {
                float4 c1 = 1.70158;
                float4 c3 = c1 + 1;

                return 1 + c3 * pow(x - 1, 3) + c1 * pow(x - 1, 2);

            }


            //this one looks CRAZY
            float4 easeInOutBack(float4 x){
            float4 c1 = 1.70158;
            float4 c2 = c1 * 1.525;
                return x < 0.5
                ? (pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) / 2
                : (pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2;
            }

            float4 easeOutQuint(float4 x) {
            return 1 - pow(1 - x, 5);
            }

            float4 easeOutSine(float4 x){
            return sin((x * PI) / 2);
            }

            float4 easeInQuint(float4 x){
            return x * x * x * x * x;
            }

            float4 easeInQuad(float4 x){
            return x * x;
            }
            float4 easeOutQuad(float4 x){
            return 1 - (1 - x) * (1 - x);
            }

            fixed4 frag(v2f i) : COLOR
            {
                float depthValue = Linear01Depth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)).r);

                float4 col = tex2Dproj(_MainTex, i.scrPos);
                float fogIntensity = _FogIntensity;
                float4 fogColor = _FogColor;
                
                depthValue = easeOutSine(depthValue);                
                float4 skyLerp = lerp(fogColor, col, depthValue);


            
                float4 output = skyLerp;
                return output; 
            }
            ENDCG
        }
    }
}
