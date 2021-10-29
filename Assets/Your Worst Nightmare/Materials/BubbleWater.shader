Shader "Custom/BubbleWater"
{
    Properties 
    {
        _highColor ("high color", Color) = (0.2899667, 0.1541919, 0.5943396, 1)
        _lowColor ("low color", Color) = (0.1607843, 0.5865988, 0.6235294, 1)
        _scale ("noise scale", Range(2, 10)) = 3.4
        _displacement ("displacement", Range(0, .5)) = .0058
        _waveSpeed("wave speed",Range(0,2)) =.2
        _crackColor("crack color", Color) = (0,0.4528301,1,1)
        _crackSize("crack size",Range(.98,1)) = .99

        
        _gloss ("glow", Range(0,1)) = .109
        _refractionIntensity ("refraction intensity", Range(0,.5)) = .176
        _opacity ("opacity", Range(0,1)) = .557
    }
    SubShader
    {
        //queue is transparent so the refraction samples 
        //from anything opaque rendered before it
        Tags { "LightMode"="ForwardBase" "Queue"="Transparent" "IgnoreProjector"="True"}

        GrabPass{
            "_BackgroundTex"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // might be UnityLightingCommon.cginc for later versions of unity
            #include "Lighting.cginc"

            #define MAX_SPECULAR_POWER 256

            sampler2D _BackgroundTex;
            float _gloss;
            float _refractionIntensity;
            float _opacity;

            float3 _highColor;
            float3 _lowColor;
            float _scale;
            float _displacement;
            float _waveSpeed;
            float3 _crackColor;
            float _crackSize;

            float2 voronoihash6( float2 p )
		    {
				p = p - 10 * floor( p / 10 );
				p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
				return frac( sin( p ) *43758.5453);
			}
			float voronoi6( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
			{
				float2 n = floor( v );
				float2 f = frac( v );
				float F1 = 8.0;
				float F2 = 8.0; float2 mg = 0;
				for ( int j = -1; j <= 1; j++ )
				{
					for ( int i = -1; i <= 1; i++ )
					{
						float2 g = float2( i, j );
						float2 o = voronoihash6( n + g );
						o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
						float d = 0.707 * sqrt(dot( r, r ));
						if( d<F1 ) {
							F2 = F1;
							F1 = d; mg = g; mr = r; id = o;
						} else if( d<F2 ) {
							F2 = d;
						
						}
					}
				}
				return F2 - F1;
			}
            float noise(float2 uv, float change){
                float2 n = 0;
                float f = voronoi6(uv * _scale, change, n, n, 0, n );
                return f;
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
                float3 tangent : TEXCOORD2;
                float3 bitangent : TEXCOORD3;
                float3 worldPos : TEXCOORD4;
                float4 screenUV : TEXCOORD5;
            };

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.uv = v.uv;
                float c = _Time.z*_waveSpeed;
				float n = noise(o.uv,c);
                n = sqrt(sqrt(n));
                
                
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.screenUV = ComputeGrabScreenPos(o.vertex);

                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                
                v.vertex.xyz += v.normal.xyz * n * _displacement;
                v.vertex.y += _displacement;
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.tangent = UnityObjectToWorldNormal(v.tangent);
                o.bitangent = cross(o.normal, o.tangent) * v.tangent.w;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                
                return o;
            }

            float4 frag (Interpolators i) : SV_Target
            {
                float c = _Time.z*_waveSpeed;
				float n = noise(i.uv,c);
                
                float3 c1 = (n);
                
                float3 c2 = ((1-n));
                
                float3 c3 = smoothstep(_crackSize,1,c2);
                
                 c1 -= c3; c2 -= c3; 
                c1 = saturate(c1)*_highColor;
                c2 = saturate(c2)*_lowColor;
                c3 = saturate(c3)*_crackColor;

                float3 color = c1+c2+c3;

                float2 uv = i.uv;
                
                float2 screenUV = i.screenUV.xy/i.screenUV.w;
                
                float3 tangentSpaceNormal = i.normal;
                tangentSpaceNormal = normalize(lerp(float3(0, 0, 1), tangentSpaceNormal,n));

                screenUV = screenUV + (tangentSpaceNormal).xy * (_refractionIntensity);                
                float3 background = tex2D(_BackgroundTex,screenUV);


                float3x3 tangentToWorld = float3x3 
                (
                    i.tangent.x, i.bitangent.x, i.normal.x,
                    i.tangent.y, i.bitangent.y, i.normal.y,
                    i.tangent.z, i.bitangent.z, i.normal.z
                );

                float3 normal = mul(tangentToWorld, tangentSpaceNormal);

                float gloss = _gloss;
                float opacity = _opacity * (1-n);
                // blinn phong
                float3 surfaceColor = color;

                float3 lightDirection = _WorldSpaceLightPos0;
                float3 lightColor = _LightColor0; // includes intensity

                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.worldPos);
                float3 halfDirection = normalize(viewDirection + lightDirection);

                float specularFalloff = max(0, dot(normal, halfDirection));

                float3 specular = pow(specularFalloff, n*gloss * MAX_SPECULAR_POWER + 0.0001) * lightColor * gloss;

                
                float3 final = (surfaceColor * opacity) + (background * (1-opacity))+specular;
                return float4(final, 1);
            //   return float4(normal.x,normal.y,normal.z,1);
            }
            ENDCG
        }
    }
}
