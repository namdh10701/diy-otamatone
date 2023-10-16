// Grayscale Shader

// Define the shader as a fragment shader
Shader "Custom/Grayscale" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _Ratio("Ratio", range(0,1)) = .5
    }
        SubShader{
            Tags { "Queue" = "Transparent" }
            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata_t {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float _Ratio;
                v2f vert(appdata_t v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                half4 frag(v2f i) : SV_Target {
                    // Sample the texture
                    half4 col = tex2D(_MainTex, i.uv);

                    // Convert the color to grayscale
                    half gray = dot(col.rgb, float3(0.299, 0.587, 0.114));
                    float4 grayScale = float4(gray, gray, gray, col.a);

                    // Set the output color to grayscale
                    return lerp(col,grayScale,_Ratio);
                }
                ENDCG
            }
    }
}