// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Malbers/Color3x3"
{
	Properties
	{
		[Header(Albedo (Alpha  Gradient Power))]_Tint("Tint", Color) = (1,1,1,1)
		[Space(10)]_Color1("Color 1", Color) = (1,0.1544118,0.1544118,0)
		_Color2("Color 2", Color) = (1,0.1544118,0.8017241,1)
		_Color3("Color 3", Color) = (0.2535501,0.1544118,1,1)
		[Space(10)]_Color4("Color 4", Color) = (0.9533468,1,0.1544118,1)
		_Color5("Color 5", Color) = (0.2669384,0.3207547,0.0226949,1)
		_Color6("Color 6", Color) = (1,0.4519259,0.1529412,1)
		[Space(10)]_Color7("Color 7", Color) = (0.9099331,0.9264706,0.6267301,1)
		_Color8("Color 8", Color) = (0.1544118,0.1602434,1,1)
		_Color9("Color 9", Color) = (0.1529412,0.9929401,1,1)
		[Header(Metallic(R) Rough(G) Emmission(B))]_MRE1("MRE 1", Color) = (0,1,0,0)
		_MRE2("MRE 2", Color) = (0,1,0,0)
		_MRE3("MRE 3", Color) = (0,1,0,0)
		[Space(10)]_MRE4("MRE 4", Color) = (0,1,0,0)
		_MRE5("MRE 5", Color) = (0,1,0,0)
		_MRE6("MRE 6", Color) = (0,1,0,0)
		[Space()]_MRE7("MRE 7", Color) = (0,1,0,0)
		_MRE8("MRE 8", Color) = (0,1,0,0)
		_MRE9("MRE 9", Color) = (0,1,0,0)
		[Header(Emission)]_EmissionPower("Emission Power", Float) = 1
		[HDR]_EmissionColor("EmissionColor", Color) = (0,0,0,0)
		[Header(Detail Texture (UV2))]_DetailsUV2("Details (UV2)", 2D) = "white" {}
		_DetailOpacity("Opacity", Range( 0 , 1)) = 0
		[Header(Gradient Properties)][SingleLineTexture][Space(10)]_Gradient("Gradient", 2D) = "white" {}
		_GradientIntensity("Gradient Intensity", Range( 0 , 1)) = 0.75
		_GradientColor("Gradient Color", Color) = (0,0,0,0)
		_GradientScale("Gradient Scale", Float) = 1
		_GradientOffset("Gradient Offset", Float) = 0
		_GradientPower("Gradient Power", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#pragma target 5.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv2_texcoord2;
			float2 uv_texcoord;
		};

		uniform sampler2D _DetailsUV2;
		uniform float4 _DetailsUV2_ST;
		uniform float _DetailOpacity;
		uniform sampler2D _Gradient;
		uniform float4 _GradientColor;
		uniform float _GradientIntensity;
		uniform float _GradientScale;
		uniform float _GradientOffset;
		uniform float _GradientPower;
		uniform float4 _Color1;
		uniform float4 _Color2;
		uniform float4 _Color3;
		uniform float4 _Color4;
		uniform float4 _Color5;
		uniform float4 _Color6;
		uniform float4 _Color7;
		uniform float4 _Color8;
		uniform float4 _Color9;
		uniform float4 _Tint;
		uniform float _EmissionPower;
		uniform float4 _MRE1;
		uniform float4 _MRE2;
		uniform float4 _MRE3;
		uniform float4 _MRE4;
		uniform float4 _MRE5;
		uniform float4 _MRE6;
		uniform float4 _MRE7;
		uniform float4 _MRE8;
		uniform float4 _MRE9;
		uniform float4 _EmissionColor;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv2_DetailsUV2 = i.uv2_texcoord2 * _DetailsUV2_ST.xy + _DetailsUV2_ST.zw;
			float4 clampResult310 = clamp( ( tex2D( _DetailsUV2, uv2_DetailsUV2 ) + ( 1.0 - _DetailOpacity ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float2 uv_TexCoord258 = i.uv_texcoord * float2( 3,3 );
			float4 clampResult206 = clamp( ( ( tex2D( _Gradient, uv_TexCoord258 ) + _GradientColor ) + ( 1.0 - _GradientIntensity ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
			float4 saferPower254 = abs( (clampResult206*_GradientScale + _GradientOffset) );
			float4 temp_cast_0 = (_GradientPower).xxxx;
			float temp_output_3_0_g444 = 1.0;
			float temp_output_7_0_g444 = 3.0;
			float temp_output_9_0_g444 = 3.0;
			float temp_output_8_0_g444 = 3.0;
			float temp_output_3_0_g447 = 2.0;
			float temp_output_7_0_g447 = 3.0;
			float temp_output_9_0_g447 = 3.0;
			float temp_output_8_0_g447 = 3.0;
			float temp_output_3_0_g450 = 3.0;
			float temp_output_7_0_g450 = 3.0;
			float temp_output_9_0_g450 = 3.0;
			float temp_output_8_0_g450 = 3.0;
			float temp_output_3_0_g443 = 1.0;
			float temp_output_7_0_g443 = 3.0;
			float temp_output_9_0_g443 = 2.0;
			float temp_output_8_0_g443 = 3.0;
			float temp_output_3_0_g448 = 2.0;
			float temp_output_7_0_g448 = 3.0;
			float temp_output_9_0_g448 = 2.0;
			float temp_output_8_0_g448 = 3.0;
			float temp_output_3_0_g441 = 3.0;
			float temp_output_7_0_g441 = 3.0;
			float temp_output_9_0_g441 = 2.0;
			float temp_output_8_0_g441 = 3.0;
			float temp_output_3_0_g446 = 1.0;
			float temp_output_7_0_g446 = 3.0;
			float temp_output_9_0_g446 = 1.0;
			float temp_output_8_0_g446 = 3.0;
			float temp_output_3_0_g445 = 2.0;
			float temp_output_7_0_g445 = 3.0;
			float temp_output_9_0_g445 = 1.0;
			float temp_output_8_0_g445 = 3.0;
			float temp_output_3_0_g449 = 3.0;
			float temp_output_7_0_g449 = 3.0;
			float temp_output_9_0_g449 = 1.0;
			float temp_output_8_0_g449 = 3.0;
			float4 temp_output_155_0 = ( ( ( _Color1 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g444 - 1.0 ) / temp_output_7_0_g444 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g444 / temp_output_7_0_g444 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g444 - 1.0 ) / temp_output_8_0_g444 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g444 / temp_output_8_0_g444 ) ) * 1.0 ) ) ) ) + ( _Color2 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g447 - 1.0 ) / temp_output_7_0_g447 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g447 / temp_output_7_0_g447 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g447 - 1.0 ) / temp_output_8_0_g447 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g447 / temp_output_8_0_g447 ) ) * 1.0 ) ) ) ) + ( _Color3 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g450 - 1.0 ) / temp_output_7_0_g450 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g450 / temp_output_7_0_g450 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g450 - 1.0 ) / temp_output_8_0_g450 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g450 / temp_output_8_0_g450 ) ) * 1.0 ) ) ) ) ) + ( ( _Color4 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g443 - 1.0 ) / temp_output_7_0_g443 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g443 / temp_output_7_0_g443 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g443 - 1.0 ) / temp_output_8_0_g443 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g443 / temp_output_8_0_g443 ) ) * 1.0 ) ) ) ) + ( _Color5 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g448 - 1.0 ) / temp_output_7_0_g448 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g448 / temp_output_7_0_g448 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g448 - 1.0 ) / temp_output_8_0_g448 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g448 / temp_output_8_0_g448 ) ) * 1.0 ) ) ) ) + ( _Color6 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g441 - 1.0 ) / temp_output_7_0_g441 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g441 / temp_output_7_0_g441 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g441 - 1.0 ) / temp_output_8_0_g441 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g441 / temp_output_8_0_g441 ) ) * 1.0 ) ) ) ) ) + ( ( _Color7 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g446 - 1.0 ) / temp_output_7_0_g446 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g446 / temp_output_7_0_g446 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g446 - 1.0 ) / temp_output_8_0_g446 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g446 / temp_output_8_0_g446 ) ) * 1.0 ) ) ) ) + ( _Color8 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g445 - 1.0 ) / temp_output_7_0_g445 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g445 / temp_output_7_0_g445 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g445 - 1.0 ) / temp_output_8_0_g445 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g445 / temp_output_8_0_g445 ) ) * 1.0 ) ) ) ) + ( _Color9 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g449 - 1.0 ) / temp_output_7_0_g449 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g449 / temp_output_7_0_g449 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g449 - 1.0 ) / temp_output_8_0_g449 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g449 / temp_output_8_0_g449 ) ) * 1.0 ) ) ) ) ) );
			float4 clampResult255 = clamp( ( pow( saferPower254 , temp_cast_0 ) + ( 1.0 - (temp_output_155_0).a ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
			o.Albedo = ( clampResult310 * ( ( clampResult255 * temp_output_155_0 ) * _Tint ) ).rgb;
			float temp_output_3_0_g464 = 1.0;
			float temp_output_7_0_g464 = 3.0;
			float temp_output_9_0_g464 = 3.0;
			float temp_output_8_0_g464 = 3.0;
			float temp_output_3_0_g462 = 2.0;
			float temp_output_7_0_g462 = 3.0;
			float temp_output_9_0_g462 = 3.0;
			float temp_output_8_0_g462 = 3.0;
			float temp_output_3_0_g463 = 3.0;
			float temp_output_7_0_g463 = 3.0;
			float temp_output_9_0_g463 = 3.0;
			float temp_output_8_0_g463 = 3.0;
			float temp_output_3_0_g459 = 1.0;
			float temp_output_7_0_g459 = 3.0;
			float temp_output_9_0_g459 = 2.0;
			float temp_output_8_0_g459 = 3.0;
			float temp_output_3_0_g460 = 2.0;
			float temp_output_7_0_g460 = 3.0;
			float temp_output_9_0_g460 = 2.0;
			float temp_output_8_0_g460 = 3.0;
			float temp_output_3_0_g458 = 3.0;
			float temp_output_7_0_g458 = 3.0;
			float temp_output_9_0_g458 = 2.0;
			float temp_output_8_0_g458 = 3.0;
			float temp_output_3_0_g456 = 1.0;
			float temp_output_7_0_g456 = 3.0;
			float temp_output_9_0_g456 = 1.0;
			float temp_output_8_0_g456 = 3.0;
			float temp_output_3_0_g461 = 2.0;
			float temp_output_7_0_g461 = 3.0;
			float temp_output_9_0_g461 = 1.0;
			float temp_output_8_0_g461 = 3.0;
			float temp_output_3_0_g465 = 3.0;
			float temp_output_7_0_g465 = 3.0;
			float temp_output_9_0_g465 = 1.0;
			float temp_output_8_0_g465 = 3.0;
			float4 temp_output_263_0 = ( ( ( _MRE1 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g464 - 1.0 ) / temp_output_7_0_g464 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g464 / temp_output_7_0_g464 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g464 - 1.0 ) / temp_output_8_0_g464 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g464 / temp_output_8_0_g464 ) ) * 1.0 ) ) ) ) + ( _MRE2 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g462 - 1.0 ) / temp_output_7_0_g462 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g462 / temp_output_7_0_g462 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g462 - 1.0 ) / temp_output_8_0_g462 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g462 / temp_output_8_0_g462 ) ) * 1.0 ) ) ) ) + ( _MRE3 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g463 - 1.0 ) / temp_output_7_0_g463 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g463 / temp_output_7_0_g463 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g463 - 1.0 ) / temp_output_8_0_g463 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g463 / temp_output_8_0_g463 ) ) * 1.0 ) ) ) ) ) + ( ( _MRE4 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g459 - 1.0 ) / temp_output_7_0_g459 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g459 / temp_output_7_0_g459 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g459 - 1.0 ) / temp_output_8_0_g459 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g459 / temp_output_8_0_g459 ) ) * 1.0 ) ) ) ) + ( _MRE5 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g460 - 1.0 ) / temp_output_7_0_g460 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g460 / temp_output_7_0_g460 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g460 - 1.0 ) / temp_output_8_0_g460 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g460 / temp_output_8_0_g460 ) ) * 1.0 ) ) ) ) + ( _MRE6 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g458 - 1.0 ) / temp_output_7_0_g458 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g458 / temp_output_7_0_g458 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g458 - 1.0 ) / temp_output_8_0_g458 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g458 / temp_output_8_0_g458 ) ) * 1.0 ) ) ) ) ) + ( ( _MRE7 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g456 - 1.0 ) / temp_output_7_0_g456 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g456 / temp_output_7_0_g456 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g456 - 1.0 ) / temp_output_8_0_g456 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g456 / temp_output_8_0_g456 ) ) * 1.0 ) ) ) ) + ( _MRE8 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g461 - 1.0 ) / temp_output_7_0_g461 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g461 / temp_output_7_0_g461 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g461 - 1.0 ) / temp_output_8_0_g461 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g461 / temp_output_8_0_g461 ) ) * 1.0 ) ) ) ) + ( _MRE9 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g465 - 1.0 ) / temp_output_7_0_g465 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g465 / temp_output_7_0_g465 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g465 - 1.0 ) / temp_output_8_0_g465 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g465 / temp_output_8_0_g465 ) ) * 1.0 ) ) ) ) ) );
			o.Emission = ( ( temp_output_155_0 * ( _EmissionPower * (temp_output_263_0).b ) ) + _EmissionColor ).rgb;
			o.Metallic = (temp_output_263_0).r;
			o.Smoothness = ( 1.0 - (temp_output_263_0).g );
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18921
121;220;1303;706;-1855.614;1969.929;1;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;258;-840.2042,-1312.028;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;3,3;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;201;-615.6998,-887.8829;Float;False;Property;_GradientIntensity;Gradient Intensity;24;0;Create;True;0;0;0;False;0;False;0.75;0.75;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;202;-586.4118,-1313.797;Inherit;True;Property;_Gradient;Gradient;23;2;[Header];[SingleLineTexture];Create;True;1;Gradient Properties;0;0;False;1;Space(10);False;-1;0f424a347039ef447a763d3d4b4782b0;0f424a347039ef447a763d3d4b4782b0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;200;-606.7432,-1071.044;Float;False;Property;_GradientColor;Gradient Color;25;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;159;-367.2498,538.3683;Float;False;Property;_Color4;Color 4;4;0;Create;True;0;0;0;False;1;Space(10);False;0.9533468,1,0.1544118,1;0.9533468,1,0.1544118,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;256;-244.6775,1818.924;Float;False;Property;_Color9;Color 9;9;0;Create;True;0;0;0;False;0;False;0.1529412,0.9929401,1,1;0.1529412,0.9929401,1,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;150;-391.0649,27.18103;Float;False;Property;_Color2;Color 2;2;0;Create;True;0;0;0;False;0;False;1,0.1544118,0.8017241,1;1,0.1544118,0.8017241,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;181;-243.7083,1591.022;Float;False;Property;_Color8;Color 8;8;0;Create;True;0;0;0;False;0;False;0.1544118,0.1602434,1,1;0.1544118,0.1602434,1,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;157;-235.251,1079.311;Float;False;Property;_Color6;Color 6;6;0;Create;True;0;0;0;False;0;False;1,0.4519259,0.1529412,1;1,0.4519259,0.1529412,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;23;-380.4475,-229.5;Float;False;Property;_Color1;Color 1;1;0;Create;True;0;0;0;False;1;Space(10);False;1,0.1544118,0.1544118,0;1,0.1544118,0.1544118,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;183;-251.6285,1359.862;Float;False;Property;_Color7;Color 7;7;0;Create;True;0;0;0;False;1;Space(10);False;0.9099331,0.9264706,0.6267301,1;0.9099331,0.9264706,0.6267301,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;152;-377.5372,262.0459;Float;False;Property;_Color3;Color 3;3;0;Create;True;0;0;0;False;0;False;0.2535501,0.1544118,1,1;0.2535501,0.1544118,1,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;156;-369.1905,827.4952;Float;False;Property;_Color5;Color 5;5;0;Create;True;0;0;0;False;0;False;0.2669384,0.3207547,0.0226949,1;0.2669384,0.3207547,0.0226949,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;238;2.790063,246.9754;Inherit;True;ColorShartSlot;-1;;450;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;3;False;9;FLOAT;3;False;7;FLOAT;3;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;204;-244.8239,-942.0975;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;240;14.66442,1076.863;Inherit;True;ColorShartSlot;-1;;441;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;3;False;9;FLOAT;2;False;7;FLOAT;3;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;236;-10.73773,16.68434;Inherit;True;ColorShartSlot;-1;;447;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;2;False;9;FLOAT;3;False;7;FLOAT;3;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;239;-2.797049,-241.6734;Inherit;True;ColorShartSlot;-1;;444;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;1;False;9;FLOAT;3;False;7;FLOAT;3;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;257;28.45395,1819.095;Inherit;True;ColorShartSlot;-1;;449;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;3;False;9;FLOAT;1;False;7;FLOAT;3;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;233;13.07732,530.6414;Inherit;True;ColorShartSlot;-1;;443;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;1;False;9;FLOAT;2;False;7;FLOAT;3;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;235;25.18534,1368.447;Inherit;True;ColorShartSlot;-1;;446;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;1;False;9;FLOAT;1;False;7;FLOAT;3;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;231;11.13652,815.7118;Inherit;True;ColorShartSlot;-1;;448;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;2;False;9;FLOAT;2;False;7;FLOAT;3;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;232;25.81848,1594.321;Inherit;True;ColorShartSlot;-1;;445;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;2;False;9;FLOAT;1;False;7;FLOAT;3;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;203;-218.1167,-1071.731;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;205;-37.61683,-1102.151;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;146;636.8021,241.9187;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;193;639.0421,747.4011;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;164;643.5082,470.012;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;155;891.6702,382.979;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;272;756.025,2958.429;Float;False;Property;_MRE9;MRE 9;18;0;Create;True;0;0;0;False;0;False;0,1,0,0;0,1,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;268;790.1596,1258.979;Float;False;Property;_MRE1;MRE 1;10;1;[Header];Create;True;1;Metallic(R) Rough(G) Emmission(B);0;0;False;0;False;0,1,0,0;0,1,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;206;212.3874,-1105.371;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;269;794.9661,1683.904;Float;False;Property;_MRE3;MRE 3;12;0;Create;True;0;0;0;False;0;False;0,1,0,0;0,1,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;270;762.3789,2542.473;Float;False;Property;_MRE7;MRE 7;16;0;Create;True;0;0;0;False;1;Space();False;0,1,0,0;0,1,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;207;187.0312,-851.6188;Float;False;Property;_GradientScale;Gradient Scale;26;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;267;797.8815,1892.403;Float;False;Property;_MRE4;MRE 4;13;0;Create;True;0;0;0;False;1;Space(10);False;0,1,0,0;0,1,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;271;760.3356,2757.597;Float;False;Property;_MRE8;MRE 8;17;0;Create;True;0;0;0;False;0;False;0,1,0,0;0,1,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;264;800.6055,1472.631;Float;False;Property;_MRE2;MRE 2;11;0;Create;True;0;0;0;False;0;False;0,1,0,0;0,1,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;265;762.6077,2329.814;Float;False;Property;_MRE6;MRE 6;15;0;Create;True;0;0;0;False;0;False;0,1,0,0;0,1,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;266;764.9778,2103.387;Float;False;Property;_MRE5;MRE 5;14;0;Create;True;0;0;0;False;0;False;0,1,0,0;0,1,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;208;185.9342,-778.8436;Float;False;Property;_GradientOffset;Gradient Offset;27;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;289;807.1936,-766.9284;Inherit;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;253;560.5085,-873.6618;Float;False;Property;_GradientPower;Gradient Power;28;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;280;1072.05,1897.946;Inherit;True;ColorShartSlot;-1;;459;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;1;False;9;FLOAT;2;False;7;FLOAT;3;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;277;1068.635,2106.349;Inherit;True;ColorShartSlot;-1;;460;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;2;False;9;FLOAT;2;False;7;FLOAT;3;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;281;1066.266,2314.42;Inherit;True;ColorShartSlot;-1;;458;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;3;False;9;FLOAT;2;False;7;FLOAT;3;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;209;539.2196,-1097.01;Inherit;True;3;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;279;1072.083,1685.052;Inherit;True;ColorShartSlot;-1;;463;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;3;False;9;FLOAT;3;False;7;FLOAT;3;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;275;1074.009,1474.637;Inherit;True;ColorShartSlot;-1;;462;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;2;False;9;FLOAT;3;False;7;FLOAT;3;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;276;1067.94,2529.334;Inherit;True;ColorShartSlot;-1;;456;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;1;False;9;FLOAT;1;False;7;FLOAT;3;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;273;1065.782,2963.45;Inherit;True;ColorShartSlot;-1;;465;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;3;False;9;FLOAT;1;False;7;FLOAT;3;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;274;1063.897,2751.832;Inherit;True;ColorShartSlot;-1;;461;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;2;False;9;FLOAT;1;False;7;FLOAT;3;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;278;1073.824,1263.506;Inherit;True;ColorShartSlot;-1;;464;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;1;False;9;FLOAT;3;False;7;FLOAT;3;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;260;1506.911,1450.623;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;254;851.5485,-1096.463;Inherit;True;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;262;1509.151,1956.105;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;294;993.1301,-772.0289;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;261;1513.617,1678.717;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;292;1144.19,-1097.765;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;307;2745.48,-894.4118;Inherit;False;Property;_DetailOpacity;Opacity;22;0;Create;False;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;263;1761.779,1591.684;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;309;3011.915,-891.5721;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;285;2191.846,574.1987;Inherit;False;Property;_EmissionPower;Emission Power;19;1;[Header];Create;False;1;Emission;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;302;2848.933,-1096.259;Inherit;True;Property;_DetailsUV2;Details (UV2);21;1;[Header];Create;True;1;Detail Texture (UV2);0;0;False;0;False;-1;None;None;True;1;False;white;LockedToTexture2D;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;255;1410.724,-1100.765;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;286;2204.463,687.576;Inherit;True;False;False;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;295;2212.28,-359.0309;Inherit;False;Property;_Tint;Tint;0;1;[Header];Create;True;1;Albedo (Alpha  Gradient Power);0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;308;3325.422,-1050.786;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;210;2216.373,-635.7195;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;287;2523.534,562.9523;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;291;1651.518,502.5378;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;288;2835.367,473.9925;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;296;2551.944,-583.7661;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;315;2865.045,707.1369;Inherit;False;Property;_EmissionColor;EmissionColor;20;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;283;2193.479,270.7187;Inherit;True;False;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;310;3598.422,-1078.786;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;290;2187.83,1603.08;Inherit;True;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;284;2862.686,251.0539;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;306;3671.715,-947.4663;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;282;2187.501,72.77197;Inherit;True;True;False;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;317;3248.402,386.0329;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3622.263,125.6268;Float;False;True;-1;7;ASEMaterialInspector;0;0;Standard;Malbers/Color3x3;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;1;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;202;1;258;0
WireConnection;238;38;152;0
WireConnection;204;0;201;0
WireConnection;240;38;157;0
WireConnection;236;38;150;0
WireConnection;239;38;23;0
WireConnection;257;38;256;0
WireConnection;233;38;159;0
WireConnection;235;38;183;0
WireConnection;231;38;156;0
WireConnection;232;38;181;0
WireConnection;203;0;202;0
WireConnection;203;1;200;0
WireConnection;205;0;203;0
WireConnection;205;1;204;0
WireConnection;146;0;239;0
WireConnection;146;1;236;0
WireConnection;146;2;238;0
WireConnection;193;0;235;0
WireConnection;193;1;232;0
WireConnection;193;2;257;0
WireConnection;164;0;233;0
WireConnection;164;1;231;0
WireConnection;164;2;240;0
WireConnection;155;0;146;0
WireConnection;155;1;164;0
WireConnection;155;2;193;0
WireConnection;206;0;205;0
WireConnection;289;0;155;0
WireConnection;280;38;267;0
WireConnection;277;38;266;0
WireConnection;281;38;265;0
WireConnection;209;0;206;0
WireConnection;209;1;207;0
WireConnection;209;2;208;0
WireConnection;279;38;269;0
WireConnection;275;38;264;0
WireConnection;276;38;270;0
WireConnection;273;38;272;0
WireConnection;274;38;271;0
WireConnection;278;38;268;0
WireConnection;260;0;278;0
WireConnection;260;1;275;0
WireConnection;260;2;279;0
WireConnection;254;0;209;0
WireConnection;254;1;253;0
WireConnection;262;0;276;0
WireConnection;262;1;274;0
WireConnection;262;2;273;0
WireConnection;294;0;289;0
WireConnection;261;0;280;0
WireConnection;261;1;277;0
WireConnection;261;2;281;0
WireConnection;292;0;254;0
WireConnection;292;1;294;0
WireConnection;263;0;260;0
WireConnection;263;1;261;0
WireConnection;263;2;262;0
WireConnection;309;0;307;0
WireConnection;255;0;292;0
WireConnection;286;0;263;0
WireConnection;308;0;302;0
WireConnection;308;1;309;0
WireConnection;210;0;255;0
WireConnection;210;1;155;0
WireConnection;287;0;285;0
WireConnection;287;1;286;0
WireConnection;291;0;155;0
WireConnection;288;0;291;0
WireConnection;288;1;287;0
WireConnection;296;0;210;0
WireConnection;296;1;295;0
WireConnection;283;0;263;0
WireConnection;310;0;308;0
WireConnection;290;0;263;0
WireConnection;284;0;283;0
WireConnection;306;0;310;0
WireConnection;306;1;296;0
WireConnection;282;0;263;0
WireConnection;317;0;288;0
WireConnection;317;1;315;0
WireConnection;0;0;306;0
WireConnection;0;2;317;0
WireConnection;0;3;282;0
WireConnection;0;4;284;0
ASEEND*/
//CHKSM=2FFAE8EA75D35A9611016EAAF1C8E9288091B13C