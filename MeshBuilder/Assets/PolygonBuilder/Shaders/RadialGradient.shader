Shader "PolygonBuilder/RadialGradient" 
{
	Properties
	{
		center ("Center", Vector) = (0,0,0,1)
		radius ("Radius", Float) = 4
		colorCenter ("Center color", Color) = (1,1,1,1)
		colorEdge ("Edge color", Color) = (0,0,0,1)	
	}

    SubShader 
    {
		Tags 
		{ 
			"RenderType" = "Transparent" 
			"Queue" = "Transparent" 
		}

		Blend SrcAlpha OneMinusSrcAlpha
		     
        pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

			uniform float radius;
			uniform float3 center;
			uniform float4 colorCenter;
			uniform float4 colorEdge;

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
				 float weight = min(distance(v.localPosition, center), radius) / radius;
				 FragOut output;

				 output.color = colorCenter * (1 - weight) + colorEdge * weight;

				 return output;
			 }

			 ENDCG
		}
     }
 }