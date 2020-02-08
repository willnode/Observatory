// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Planet/PlanetForEarth"
{
 Properties {
     [Header(Surface)]
     _MainTex ("Diffuse(RGB) Spec(A)", 2D) = "white" {}
     _BumpMap ("Bumpmap", 2D) = "bump" {}
     _SpecMap ("Specular Map", 2D) = "white" {}
     _CloudMap ("Cloud Decal Map", 2D) = "black" {}
     _NightMap ("Night Map", 2D) = "black" {}
     _NightPower ("Night Power", Range(0,3.0)) = 1.0
     _RimPower ("Rim Power", Range(0,8.0)) = 3.0
     _RimExpo ("Rim Expo", Range(0.5,5.0)) = 1.0
     _BumpScale ("Bump Scale", Float) = 1.0
     [Header(Atmosphere)]
     _AtmoColor("Atmosphere Color", Color) = (0.5, 0.5, 1.0, 1)
     _Size("Size", Float) = 0.1
     _Falloff("Smooth Falloff", Range(1,20)) = 5
     _Falloff2("Backface Falloff", Range(-1,1)) = .5
     _Falloff3("BackfaceSmoothFalloff", Range(0,1)) = 5
     _Transparency("Transparency", Float) = 15
 }

 SubShader {
     Tags { "RenderType" = "Opaque" }
     CGPROGRAM
 //  #pragma multi_compile __ NO_SUN_ILLUMIN
     #include "PlanetIncludes.cginc"
     #pragma surface surf Planet nometa nolightmap nodynlightmap noshadow nofog noforwardadd
             
     sampler2D _SpecMap;
     sampler2D _CloudMap;
     sampler2D _NightMap;
     float _NightPower;
     float _BumpScale;

        fixed3 screen(fixed3 source, fixed3 dest)
        {
            fixed3 rem = -source+fixed3(1,1,1);
            return source + fixed3(dest.r * rem.r, dest.g * rem.g, dest.b * rem.b);
        }

         fixed3 screendark(fixed3 source, fixed3 dest)
        {
            fixed3 rem = source;
            return source - fixed3(dest.r * rem.r, dest.g * rem.g, dest.b * rem.b);
        }

     half3 LightingPrePass (half3 c, float m)
    {
        //   half3 spec = light.a * s.Gloss;
        c = c * max(m+0.3,0);
        m = max(m,0);
        c.g -= .01 * m;
        c.r -= .03 * m;
       c.b = (c.b + m * .02) * 0.5;
        return c;
    } 

     

     void surf (Input IN, inout SurfaceOutput o) {
         o.Normal = UnpackNorm (tex2D (_BumpMap, IN.uv_BumpMap), _BumpScale);
         fixed3 cloud = tex2D (_CloudMap, IN.uv_CloudMap).rgb;
         o.Albedo = screen(tex2D (_MainTex, IN.uv_MainTex).rgb, cloud);
         o.Alpha = 1; 
         o.Gloss = max(tex2D (_SpecMap, IN.uv_MainTex).r, cloud.r) * 2;

        float3 lightDir = normalize(IN.worldPos - _WorldSpaceLightPos0);
        float multNight =  dot(lightDir, WorldNormalVector(IN, o.Normal));
        half3 darkColor = screendark(tex2D(_NightMap, IN.uv_MainTex).rgb, (cloud));
        o.Emission =  LightingPrePass( darkColor * _NightPower, multNight);
     }
     ENDCG
     
     
             Tags {"LightMode" = "ForwardBase" "Queue"="Transparent-1" "IgnoreProjector"="True" "RenderType"="Transparent" }
     Pass
     {
         Name "GlowAtmosphere"
         Cull Front
         Blend SrcAlpha One
         ZWrite Off

         CGPROGRAM
         #pragma vertex vert
         #pragma fragment frag

         //#pragma fragmentoption ARB_fog_exp2
         //#pragma fragmentoption ARB_precision_hint_fastest

         #include "UnityCG.cginc"

         float4 _Color;
         float4 _AtmoColor;
         float _Size;
         float _Falloff;
         float _Falloff2;
         float _Falloff3;
         float _Transparency;
         float _RimPower;

         struct v2f
         {
             float4 pos : SV_POSITION;
             float3 normal : TEXCOORD0;
             float3 worldvertpos : TEXCOORD1;
         };

         v2f vert(appdata_base v)
         {
             v2f o;

             v.vertex.xyz *= _Size;
             o.pos = UnityObjectToClipPos (v.vertex);
             o.normal = mul((float3x3)unity_ObjectToWorld, v.normal);
             o.worldvertpos = mul(unity_ObjectToWorld, v.vertex);

             return o;
         }

         float4 frag(v2f i) : COLOR
         {
             i.normal = normalize(i.normal);
             float3 viewdir = normalize(i.worldvertpos-_WorldSpaceCameraPos);
             float3 lightdir = normalize(i.worldvertpos- _WorldSpaceLightPos0);

             float4 color = _AtmoColor;
             color.a = dot(viewdir, i.normal);
             color.a -= clamp((dot(i.normal,lightdir)-_Falloff2)*_Falloff3,0,3);
             color.a = saturate(color.a);
             color.a = pow(color.a, _Falloff);
             color.a *= _Transparency;
             return color;
         }
         ENDCG
     }

 } 
//   Fallback "Legacy Shaders/VertexLit"
}
