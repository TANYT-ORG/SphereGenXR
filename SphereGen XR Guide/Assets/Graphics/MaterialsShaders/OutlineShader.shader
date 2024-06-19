Shader "Custom/UnlitOutlineShader" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        _OutlineColor("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineThickness("Outline Thickness", Range(0, 0.1)) = 0.02
    }

        SubShader{
            // First pass: Render the unlit color and texture
            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _Color;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                    return col;
                }
                ENDCG
            }

            // Second pass: Render the outline
            Pass {
                Cull Front

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                };

                struct v2f {
                    float4 vertex : SV_POSITION;
                };

                float _OutlineThickness;
                float4 _OutlineColor;

                v2f vert(appdata v) {
                    v2f o;
                    float3 normal = UnityObjectToWorldNormal(v.normal);
                    float4 vertex = UnityObjectToClipPos(v.vertex);
                    o.vertex = vertex + float4(normal, 0) * _OutlineThickness;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    return _OutlineColor;
                }
                ENDCG
            }
        }
            FallBack "Diffuse"
}
