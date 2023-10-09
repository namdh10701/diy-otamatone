Shader "Unlit/Trail Shader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_IsActive("Is Active", range(0,1)) = 0
			_Height("Height", range(0,1)) = 1
	}
		SubShader
		{
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}

		Blend SrcAlpha OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float _IsActive;
				float _Height;
				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					return o;
				}

				float4 frag(v2f i) : SV_Target
				{
					float4 col1 = tex2D(_MainTex, i.uv);
					if (col1.w > .2) {
						col1.w = .3;
					}
					//if (_IsActive > .8) {
					//	if (col1.w >= .3) {
					//		col1.w = .7;
					//	}
					//}
	
					float fade = 1.0 - saturate((i.uv.y - (1.0 - _Height)) / _Height);

			
						col1.w = lerp(col1.w, .5, fade); // Fade out the alpha
			
					float4 col2 = col1;
					col2.xyz += .3;
					float4 finalCol = lerp(col1, col2, _IsActive);

					if (i.uv.y < (1 - _Height)) {
						finalCol.w = 0;
					}
				return finalCol;
			}
			ENDCG
		}
		}
}
