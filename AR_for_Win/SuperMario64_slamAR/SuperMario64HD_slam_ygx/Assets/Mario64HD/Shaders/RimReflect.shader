Shader "Rim/Reflective" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess ("Shininess", Range (0.01, 1)) = 0.078125
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Cube ("Reflection Cubemap", Cube) = "_Skybox" { }
		_RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
		_RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 400
		
		CGPROGRAM
		#pragma surface surf BlinnPhong

		sampler2D _MainTex;
		samplerCUBE _Cube;
		float4 _RimColor;
		float _RimPower;
		fixed4 _Color;
		fixed4 _ReflectColor;
		half _Shininess;

		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
			float3 worldRefl;
			INTERNAL_DATA
		};

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			float3 worldRefl = WorldReflectionVector (IN, o.Normal);
			fixed4 reflcol = texCUBE (_Cube, worldRefl);
			
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			
			o.Albedo = c.rgb * _Color.rgb;
			o.Gloss = tex.a;
			
			half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
			
			o.Emission = _RimColor.rgb * pow (rim, _RimPower) + reflcol.rgb * _ReflectColor.rgb;
			o.Specular = _Shininess;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
