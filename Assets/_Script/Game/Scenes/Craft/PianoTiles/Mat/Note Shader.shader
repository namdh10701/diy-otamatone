

// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader"Unlit/Note Shader" {
Properties {
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
    _AlterTex("Alter Texture", 2D) = "white"{}
	_IsActive("Is Active", range(0,1)) = 0
}

SubShader {
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}

    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha

    Pass {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #include "UnityCG.cginc"

struct appdata_t
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float4 vertex : SV_POSITION;
    float2 uv : TEXCOORD0;
};

sampler2D _MainTex;
sampler2D _AlterTex;
float _IsActive;

v2f vert(appdata_t v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;
    return o;
}

fixed4 frag(v2f i) : SV_Target
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