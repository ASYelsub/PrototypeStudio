    Shader "Custom/Invisible" {
      SubShader {
        Pass {
          CGPROGRAM
          #pragma vertex vert
          #pragma fragment frag
       

        struct appdata
        {
            float4 vertex : POSITION;
        };

        struct v2f {
            float4 pos : SV_POSITION;
            float4 vertex : SV_POSITION;
            float4 scrPos : TEXCOORD1;
        };
         
          v2f vert ()
          {
              v2f o;
              o.pos = fixed4(0,0,0,0);
              return o;
          }
     
          fixed4 frag (v2f i) : COLOR0 { 
              return fixed4(0,0,0,0); 
              }
          ENDCG
        }
      }
    }
