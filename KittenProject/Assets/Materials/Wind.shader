Shader "Custom/Wind"
{
	Properties
	{
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_TimeOffset("Time Offset", Float) = 0
	}

	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			float _TimeOffset;
			float4 _MainTex_ST;

			v2f vert(appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex) * _Time.x;
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				const float fadeSize = 0.2;
				const float timeScale = 3.5;

				float alphaFactor = 1.0f;
				float time = _Time.y * timeScale + _TimeOffset;
		
				if (i.texcoord.x > 1 - fadeSize)
				{
					alphaFactor = (1 - i.texcoord.x) / fadeSize;
				}
				else if (i.texcoord.x < fadeSize)
				{
					alphaFactor = i.texcoord.x / fadeSize;
				}

				if (floor(time) % 2)
				{
					time = frac(time);
				}
				else
				{
					time = 1 - frac(time);
				}

				i.texcoord.y -= _Time.y * 0.5 + _TimeOffset;

				fixed4 col = tex2D(_MainTex, i.texcoord) * time;
				col += tex2D(_MainTex, (half2(1, 1) - i.texcoord)) * (1 - time);

				col.a *= alphaFactor * 0.12;

				return col;
			}

			ENDCG
		}

	}

}
