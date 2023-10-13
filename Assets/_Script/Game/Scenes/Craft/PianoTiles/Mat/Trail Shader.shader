Shader"Unlit/Trail Shader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_IsActive("Is Active", range(0,1)) = 0
		_NoteTex("Note Tex", 2D) = "white"{}
		_Height("Height", range(0,1)) = 1
		_IsFade("Is Fade", float) = 1
	}
		SubShader
		{
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}

			ZWrite Off
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
				sampler2D _NoteTex;
				float4 _MainTex_ST;
				float _IsActive;
				float _Height;
				float _IsFade;
				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					return o;
				}

				float4 frag(v2f i) : SV_Target
				{

					float4 col = tex2D(_MainTex, i.uv);
					bool HeightMask = i.uv.y < 1 - _Height;
					col = lerp(col, 0, HeightMask);
					if (_IsFade)
					{
					
							if (i.uv.y > 1 - _Height)
							{
								col.a = lerp(col.a * .01, col.a, i.uv.y-.01);
							}
			
					}
					float4 hightlightCol = float4(col.xyz + .2f, col.a);
					col = lerp(col, hightlightCol, _IsActive);

				return col;
			}
			ENDCG
		}
		}
}
