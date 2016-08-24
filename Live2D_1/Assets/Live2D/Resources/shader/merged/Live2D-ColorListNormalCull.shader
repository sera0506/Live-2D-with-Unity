
	Shader "Live2D/ColorListNormalCull" {
		
		CGINCLUDE
		#pragma vertex vert 
		#pragma fragment frag
		#include "UnityCG.cginc"

			
		uniform sampler2D _MainTex;
		uniform sampler2D _SubTex;

	#if ! defined( SV_Target )
		#define SV_Target	COLOR
	#endif

	#if ! defined( SV_POSITION )
		#define SV_POSITION	POSITION
	#endif
		
			
		struct v2f {
			float4 position : SV_POSITION;
			float2 texcoord : TEXCOORD0;
			float4 color:COLOR0;
		};
		
		ENDCG
				
		SubShader {
			Tags { "Queue" = "Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
			LOD 100
			BindChannels{ Bind "Vertex", vertex Bind "texcoord", texcoord Bind "Color", color }

			Pass {
				Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha ZWrite Off Lighting Off Cull Back
				CGPROGRAM

							
				v2f vert(appdata_base v ,float4 color:COLOR)
				{
					v2f OUT;
					OUT.position = mul(UNITY_MATRIX_MVP, v.vertex);
					OUT.texcoord = v.texcoord ;
					OUT.color=color;
					return OUT;
				}
				
							
				float4 frag ( v2f IN) : SV_Target
				{
						
					float4 c=tex2D (_MainTex, IN.texcoord);
					float maxValue=max(c.r,max(c.g,c.b));
					float minValue=min(c.r,min(c.g,c.b));
					float light=(maxValue+minValue)/2.0;
					float4 conv=tex2D(_SubTex,float2(light,0.5));
					return float4(conv.r,conv.g,conv.b,c.a); 
				}
				
				ENDCG
			}
		}
	}