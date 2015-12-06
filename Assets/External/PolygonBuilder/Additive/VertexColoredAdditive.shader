Shader "PolygonBuilder/Additive/VertexColoredAdditive" 
{
    Properties {
    }

    SubShader 
    {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		Blend SrcAlpha One
		     
        pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

			//vertex
			 struct VertOut
			 {
				 float4 position : POSITION;
				 float4 color : COLOR;
			 };
			 struct VertIn
			 {
				 float4 vertex : POSITION;
				 float4 color : COLOR;
			 };

			 VertOut vert(VertIn input)
			 {
				 VertOut output;
				 output.position = mul(UNITY_MATRIX_MVP,input.vertex);
				 output.color = input.color;
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
				 output.color = v.color;
				 return output;
			 }

			 ENDCG
		}
     }
 }