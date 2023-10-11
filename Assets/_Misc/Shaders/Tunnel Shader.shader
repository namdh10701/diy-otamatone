Shader"Custom/TunnelGateShader" {
   // Shader Properties
Properties
{
    _MainTex ("Texture", 2D) = "white" {}
    _Speed ("Zoom Speed", Range(0, 1)) = 0.1
}

SubShader
{
    Pass
    {
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
#include "UnityCG.cginc"

struct appdata_t
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
float _Speed;

v2f vert(appdata_t v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);

            // Scale the UV coordinates over time
    float timeFactor = _Time.y * _Speed;
    o.uv = v.uv * (1.0 + timeFactor);

    return o;
}

half4 frag(v2f i) : SV_Target
{
            // Loop the texture
    float2 uv = frac(i.uv);

            // You can create the effect of a centered loop by using the absolute function
    uv -= 0.5;
    uv = abs(uv);
    uv += 0.5;

    half4 col = tex2D(_MainTex, uv);
    return col;
}
        ENDCG
    }
}
}