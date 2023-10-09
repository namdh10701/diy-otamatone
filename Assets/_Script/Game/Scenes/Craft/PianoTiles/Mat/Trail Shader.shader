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
					float4 col = tex2D(_MainTex, i.uv);
					bool HeightMask = i.uv.y < 1 - _Height;
					col = lerp(col, 0, HeightMask);
					if (col.a > .2) {
						if (i.uv.y > 1 - _Height) {
							col.a = lerp(0, 1, i.uv.y);
						}
					}
					float4 hightlightCol = float4(col.xyz + .3f, col.a);
					col = lerp(col, hightlightCol, _IsActive);

				return col;
			}
			ENDCG
		}
		}
}
