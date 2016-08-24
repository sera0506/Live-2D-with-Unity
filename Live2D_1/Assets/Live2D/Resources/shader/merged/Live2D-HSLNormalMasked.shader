
	Shader "Live2D/HSLNormalMasked" {
		
		CGINCLUDE
		#pragma vertex vert 
		#pragma fragment frag
		#include "UnityCG.cginc"

			
		uniform sampler2D _MainTex;
		uniform float4x4 _ConvertMat;
		uniform float _MainLineHigh;
		uniform float _MainLineLow;
		sampler2D _ClipTex;
		float4x4 _ClipMatrix ; 
		float4   _ChannelFlag; // Color Channel Flag 

	#if ! defined( SV_Target )
		#define SV_Target	COLOR
	#endif

	#if ! defined( SV_POSITION )
		#define SV_POSITION	POSITION
	#endif
		
			
		struct v2f {
			float4 position : POSITION;
			float2 texcoord : TEXCOORD0;
			float4 clipPos : TEXCOORD1;
			float4 color:COLOR0;
		};
		
		ENDCG
				
		SubShader {
			Tags { "Queue" = "Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
			LOD 100
			BindChannels{ Bind "Vertex", vertex Bind "texcoord", texcoord Bind "Color", color }

			Pass {
				Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha ZWrite Off Lighting Off Cull Off
				CGPROGRAM

							
				v2f vert(appdata_base v ,float4 color:COLOR)
				{
					v2f OUT;
					OUT.position = mul(UNITY_MATRIX_MVP, v.vertex);
					OUT.clipPos = mul(_ClipMatrix, v.vertex);
			#if UNITY_UV_STARTS_AT_TOP
			        OUT.clipPos.y = 1.0 - OUT.clipPos.y ;// invert
			#endif
					OUT.texcoord=v.texcoord;
					OUT.color=color;
					return OUT;
				}
				
							
				float4 frag ( v2f IN) : SV_Target
				{
						
					fixed4 src=tex2D (_MainTex, IN.texcoord);
				    fixed4 conv=mul(_ConvertMat, src);// HSL color conversion
					//conv = clamp(conv , 0.0 , src.a) ;//Unity is not pre-multiplied alpha.
					float light = max( src.r , max(src.g , src.b) ) ;// keep line
					float t = smoothstep( _MainLineLow* src.a , _MainLineHigh* src.a , light ) ;//min=>1, max=>0
					float4 col_formask =  lerp(src,conv,t); 				
					float4 clipMask = (1.0 - tex2D (_ClipTex, IN.clipPos.xy / IN.clipPos.w )) * _ChannelFlag ;
					col_formask.a = col_formask.a * (clipMask.r + clipMask.g + clipMask.b + clipMask.a) ;
					return col_formask ; 
				}
				
				ENDCG
			}
		}
	}