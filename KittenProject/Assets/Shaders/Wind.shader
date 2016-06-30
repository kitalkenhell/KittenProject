Shader "Custom/Wind"
{
	Properties
	{
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_TimeOffset("Time Offset", Float) = 0
		_TimeScale("Time Scale", Float) = 0.5
	    _Alpha("Alpha", Float) = 0.2
		_FadeSize("FadeSize", Float) = 0.2
	}

	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 100

		ZWrite Off
		Blend SrcAlpha dstAlpha

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
			float4 _MainTex_ST;
			float _TimeOffset;
			float _TimeScale;
			float _Alpha;
			float _FadeSize;

			v2f vert(appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex) * _Time.x;
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				const float fadeSize = _FadeSize;
				float time = _Time.y * _TimeScale + _TimeOffset;

				time = abs(floor(time) % 2 - frac(time));
				float alphaFade = 1 - max(i.texcoord.y - (1 - fadeSize), 0) / fadeSize;
				i.texcoord.y -= _Time.y * _TimeScale + _TimeOffset;

				fixed4 col = lerp( tex2D(_MainTex, i.texcoord), tex2D(_MainTex, (half2(1, 1) - i.texcoord)), time);
				col.a *= _Alpha * alphaFade;
				return col;
			}

			ENDCG
		}

	}

}
