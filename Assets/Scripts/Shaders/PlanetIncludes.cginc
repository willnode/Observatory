
sampler2D _MainTex;
sampler2D _BumpMap;
float _RimPower;
float _RimExpo;

half4 LightingPlanet (SurfaceOutput s, half3 lightDir, 
  half3 viewDir, half atten) {

  half3 h = normalize (lightDir + viewDir);
  half diff = max(0, dot ( lightDir, s.Normal ));
  half diffPow = pow(diff, _RimPower) * _RimExpo;
  float nh = max (0, dot (s.Normal, h));

  float spec = pow (nh, 48.0);
  half4 c;

  c.rgb = (s.Albedo * _LightColor0.rgb * (diffPow + spec * 
    s.Gloss)) * (1);
  c.a = 0;
   return c;
}


 half3 UnpackNorm (half4 packednormal, half bumpScale)
  {
      half3 normal;
      normal.xy = (packednormal.wy * 2 - 1);
	  normal.xy *= bumpScale;
      normal.z = sqrt(1.0 - saturate(dot(normal.xy, normal.xy)));
      return normal;
  } 


struct Input {
  float2 uv_MainTex;
  float2 uv_CloudMap;
  float2 uv_BumpMap;
  float3 viewDir;
  float3 lightDir;
  float3 worldPos;
  float3 worldNormal;
  float3 worldRefl; INTERNAL_DATA
};