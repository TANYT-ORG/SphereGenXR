Shader "UCLA Game Lab/Wireframe/Wireframe"
{
	Properties
	{
		_Color("Line Color", Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}
		_Thickness("Thickness", Float) = 1
		_Color2("Surface Color", Color) = (1,1,1,1)
	}

		SubShader
		{
			// New pass for regular surface rendering
			Pass
			{
				Tags { "RenderType" = "Opaque" "Queue" = "Geometry+10" }

				Blend SrcAlpha OneMinusSrcAlpha
				Cull Back
				LOD 200

				CGPROGRAM
					#pragma target 5.0
					#include "UnityCG.cginc"
					#pragma vertex vert2
					#pragma fragment frag2

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
					float4 _Color2;

					v2f vert2(appdata v)
					{
						v2f o;
						o.vertex = UnityObjectToClipPos(v.vertex);
						o.uv = v.uv;
						return o;
					}

					fixed4 frag2(v2f i) : SV_Target
					{
						fixed4 col = tex2D(_MainTex, i.uv) * _Color2;
						return col;
					}

				ENDCG
			}

			Pass
			{
				Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }

				Blend SrcAlpha OneMinusSrcAlpha
				Cull Off
				LOD 200

				CGPROGRAM
					#pragma target 5.0
					#include "UnityCG.cginc"
					#include "UCLA GameLab Wireframe Functions.cginc"
					#pragma vertex vert
					#pragma fragment frag
					#pragma geometry geom

						// Vertex Shader
						UCLAGL_v2g vert(appdata_base v)
						{
							return UCLAGL_vert(v);
						}

					// Geometry Shader
					[maxvertexcount(3)]
					void geom(triangle UCLAGL_v2g p[3], inout TriangleStream<UCLAGL_g2f> triStream)
					{
						UCLAGL_geom(p, triStream);
					}

					// Fragment Shader
					float4 frag(UCLAGL_g2f input) : COLOR
					{
						float4 col = UCLAGL_frag(input);
						if (col.a < 0.5f) discard;
						else col.a = 1.0f;

						return col;
					}

				ENDCG
			}
		}
}
