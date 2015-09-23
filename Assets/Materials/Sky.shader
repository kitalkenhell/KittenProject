Shader "Custom/Sky"
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
    SubShader
	{
        Pass 
		{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

			uniform sampler2D _MainTex;

            struct vertOut
			{
                float4 pos : SV_POSITION;
                float4 scrPos : COLOR;
            };

            vertOut vert(appdata_base v)
			{
                vertOut o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.scrPos = ComputeScreenPos(o.pos);
                return o;
            }

            fixed4 frag(vertOut i:COLOR) : SV_Target 
			{
                float2 uv = (i.scrPos.xy/i.scrPos.w);
				float factor = _ScreenParams.x / _ScreenParams.y / (2.0f);
				float offset = (1.0f - factor) / 2.0f;

				return tex2D(_MainTex, float2(uv.x * factor + offset,  uv.y) );
            }

            ENDCG

        }
    }
}
