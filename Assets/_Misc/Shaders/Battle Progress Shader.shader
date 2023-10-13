Shader "Unlit/Battle Progress Shader"
{
	Properties
	{
		_P1Progress("P1 Progress", range(0,1)) = 0.5
		_P1Icon("P1 Icon", 2D) = "white" {}
		_P1Texture("P1 Texture", 2D) = "white" {}
		_P2Icon("P2 Icon", 2D) = "white" {}
		_P2Texture("P2 Texture", 2D) = "white" {}
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

				sampler2D _P1Texture;
				sampler2D _P2Texture;
				sampler2D _P1Icon;
				sampler2D _P2Icon;
				float _P1Progress;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					return o;
				}

				float4 frag(v2f i) : SV_Target
				{
					float4 p1TexCol = tex2D(_P1Texture, i.uv);
					float4 p2TexCol = tex2D(_P2Texture, i.uv);
					float4 p1IconCol = tex2D(_P1Icon, i.uv);
					float4 p2IconCol = tex2D(_P2Icon, i.uv);

					bool P1Mask = i.uv.x < _P1Progress;
					float pulseMask = sin(_Time.y * 6 - (i.uv.x + .2));
					p1TexCol.xyz = lerp(p1TexCol.xyz, p1TexCol.xyz * 1.4, pulseMask);
					p2TexCol.xyz = lerp(p2TexCol.xyz, p2TexCol.xyz * 1.4, pulseMask);

				return lerp(p2TexCol, p1TexCol, P1Mask);
			}
			ENDCG
		}
		}
}
