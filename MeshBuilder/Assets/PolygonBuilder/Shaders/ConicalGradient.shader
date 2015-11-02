Shader "PolygonBuilder/ConicalGradient" 
{
	Properties
	{
		center ("Center", Vector) = (0,0,0,1)
		direction ("Direction", Vector) = (0,1,0,0)
		radius ("Radius", Float) = 4
		maxRange ("Max range", Float) = 4
		maxAngle ("Max angle", Float) = 0.3
		cutoff ("Cutoff", Float) = 0.25
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
			uniform float cutoff;
			uniform float maxRange;
			uniform float maxAngle;
			uniform float3 center;
			uniform float3 direction;
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
				 float dist = distance(v.localPosition, center);
				 float weight = min(dist, radius) / radius;
				 FragOut output;

				 output.color = (colorCenter * (1 - weight) + colorEdge * weight);

				 float angle = max(dot(direction, normalize(v.localPosition - center)),0);

				 if (angle < cutoff)
				 {
					output.color.a *= 1 - (cutoff - angle) / (maxAngle);
				 }

				 output.color.a *= 1 - min((max(radius, dist) - radius) / maxRange  , 1);

				 return output;
			 }

			 ENDCG
		}
     }
 }