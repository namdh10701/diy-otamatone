Shader"Custom/VerticalMaskedTexture"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _MaskTex ("Mask Texture", 2D) = "white" {}
    }
 
    SubShader
    {
        Tags { "Queue"="Transparent" }
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
sampler2D _MaskTex;

v2f vert(appdata_t v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;
    return o;
}

half4 frag(v2f i) : SV_Target
{
                // Sample the mask texture.
    float maskValue = tex2D(_MaskTex, i.uv).r;

                // Check if the pixel should be drawn or made transparent.
    if (maskValue > 0.5)
    {
        return tex2D(_MainTex, i.uv);
    }
    else
    {
        return half4(0, 0, 0, 0); // Transparent
    }
}
            ENDCG
        }
    }
}



