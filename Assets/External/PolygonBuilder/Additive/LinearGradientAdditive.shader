Shader "PolygonBuilder/Additive/LinearGradientAdditive" 
{
	Properties
	{
		begin ("Begin", Vector) = (0,0,0,1)
		direction ("Direction", Vector) = (0,1,0,0)
		radius ("Radius", Float) = 5
		colorBegin ("Center color", Color) = (1,1,1,1)
		colorEnd ("Edge color", Color) = (0,0,0,1)	
	}

    SubShader 
    {
		Tags 
		{ 
			"RenderType" = "Transparent" 
			"Queue" = "Transparent" 
		}

		Blend SrcAlpha One
		     
        pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

			uniform float3 begin;
			uniform float3 direction;
			uniform float radius;
			uniform float4 colorBegin;
			uniform float4 colorEnd;

			//vertex
			 struct VertOut
			 {
				 float4 position : POSITION;
				 float4 localPosition : NORMAL;
			 };
			 struct VertIn
			 {
				 float4 vertex : POSITION;
			 };

			 VertOut vert(VertIn input)
			 {
				 VertOut output;
				 output.position = mul(UNITY_MATRIX_MVP, input.vertex);
				 output.localPosition = input.vertex;
				 return output;
			 }

			 //fragment
			 struct FragOut
			 {
				 float4 color : COLOR;
			 };

			 FragOut frag(VertOut v)
			 {
				 FragOut output;
				 float weight = length(cross(v.localPosition - begin, direction)) / radius;

				 if (dot(float3(-direction.y, direction.x, 0),  v.localPosition - begin) > 0)
				 {
					output.color = colorBegin;
				 }
				 else
				 {
					output.color = colorBegin * (1 - weight) + colorEnd * weight;
				 }

				 return output;
			 }

			 ENDCG
		}
     }
 }