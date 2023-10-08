Shader "Unlit/Note Shader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_AlterTex("Alter Texture", 2D) = "white"{}
		_IsActive("Is Active", range(0,1)) = 0
	}
		SubShader
		{
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
			ZTest Always
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
				float4 _MainTex_ST;
				float4 _AlterTex_ST;
				sampler2D _AlterTex;
				float _IsActive;
				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					TRANSFORM_TEX(v.uv, _AlterTex);
					//o.uv = v.uv;
					return o;
				}

				float4 frag(v2f i) : SV_Target
				{
					float4 col1 = tex2D(_MainTex, i.uv);
					float4 col2 = tex2D(_AlterTex, i.uv);
					col2.xyz += 0.2;
					float4 finalCol = lerp(col1, col2, _IsActive);
				return finalCol;
			}
			ENDCG
		}
		}
}
