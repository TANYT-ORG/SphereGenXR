Shader "Custom/ColorPulse" {
    Properties{
        _Color1("Color 1", Color) = (1, 0, 0, 1)
        _Color2("Color 2", Color) = (0, 0, 1, 1)
        _Speed("Speed", Range(0, 10)) = 1
    }

        SubShader{
            Tags {"RenderType" = "Opaque"}

            CGPROGRAM
            #pragma surface surf Standard

            sampler2D _MainTex;
            fixed4 _Color1;
            fixed4 _Color2;
            float _Speed;

            struct Input {
                float2 uv_MainTex;
            };

            void surf(Input IN, inout SurfaceOutputStandard o) {
                fixed4 c = lerp(_Color1, _Color2, (sin(_Time.y * _Speed) + 1) / 2);
                o.Albedo = c.rgb;
                o.Alpha = c.a;
            }
            ENDCG
    }
        FallBack "Diffuse"
}
