Shader "FX/Single Color" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
}

SubShader {
	Tags {}
	LOD 200

CGPROGRAM
#pragma surface surf Lambert

sampler2D _MainTex;
fixed4 _Color;

struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	o.Albedo = _Color.rgb;
}
ENDCG
}

Fallback "Mobile/VertexLit"
}
