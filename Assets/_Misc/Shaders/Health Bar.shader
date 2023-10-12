//col += lerp(_EndColor, _StartColor, smoothstep(0.2, 0.8, _Amount)) * .1f;
Shader "Custom/Health Bar With Border"
{
	Properties
	{
		_StartColor("Start Color", Color) = (0, 0, 0, 0)
		_EndColor("End Color", Color) = (0, 0, 0, 0)
		_BorderColor("Border Color", Color) = (1, 1, 1, 1) // Add border color property
		_BorderWidth("Border Width", Range(0, 1)) = 0.02  // Add border width property
		_MainTex("Texture", 2D) = "white" {}
		_Amount("Amount", Range(0, 1)) = 1
		_BorderRadius("Border Radius", Range(0,.5)) = 0.5
			_FlashWidth("Flash Width",Range(0,1)) = 0.5
	}

		SubShader
		{
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"
				sampler2D _MainTex;
				float _Amount;
				float4 _StartColor;
				float4 _EndColor;
				float _BorderWidth; // Border width
				float _FlashWidth;
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

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					return o;
				}

				float4 frag(v2f i) : SV_Target
				{
					 float4 healthBarColor = tex2D(_MainTex, float2(_Amount, i.uv.y));
					 float2 coord = i.uv;
					 coord.x *= 8;
					 float2 lineSeg = float2(clamp(coord.x, .5, 7.5),.5);
					 float sdf = distance(coord, lineSeg) * 2 - 1;
					 clip(-sdf);
					 float borderMask = step(0, -(sdf + _BorderWidth));
					 if (_Amount < .2) {
						 float pulseMask = sin(_Time.y * 3 - (i.uv.x + _FlashWidth));
						 healthBarColor.xyz = lerp(healthBarColor.xyz, healthBarColor.xyz * 1.3, pulseMask);
					 }

					 float healthAmountMask = _Amount > i.uv.x;
					 return lerp(0, float4(healthBarColor.xyz * borderMask,healthBarColor.w), healthAmountMask);
				 }
				 ENDCG
			 }
		}
}
