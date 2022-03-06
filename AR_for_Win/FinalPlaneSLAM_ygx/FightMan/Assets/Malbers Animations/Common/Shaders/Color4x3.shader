// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Malbers/Color4x3"
{
	Properties
	{
		[Header(Row 1)]_Color1("Color 1", Color) = (1,0.1544118,0.1544118,0.397)
		_Color2("Color 2", Color) = (1,0.1544118,0.8017241,0.334)
		_Color3("Color 3", Color) = (0.2535501,0.1544118,1,0.228)
		_Color4("Color 4", Color) = (0.1544118,0.5451319,1,0.472)
		[Header(Row 2)]_Color5("Color 5", Color) = (0.9533468,1,0.1544118,0.353)
		_Color6("Color 6", Color) = (0.2669384,0.3207547,0.0226949,0.341)
		_Color7("Color 7", Color) = (0.1544118,0.6151115,1,0.316)
		_Color8("Color 8", Color) = (0.4849697,0.5008695,0.5073529,0.484)
		[Header(Row 3)]_Color9("Color 9", Color) = (0.9099331,0.9264706,0.6267301,0.353)
		_Color10("Color 10", Color) = (0.1544118,0.1602434,1,0.341)
		_Color11("Color 11", Color) = (1,0.1544118,0.381846,0.316)
		_Color12("Color 12", Color) = (0.02270761,0.1632713,0.2205882,0.484)
		[Header(Smoothness (Alphas))]_Smoothness("Smoothness", Range( 0 , 1)) = 1
		_Metallic("Metallic", Range( 0 , 1)) = 0
		[HDR][Header(Emmision)]_Color11Emmision("Color 11 Emmision", Color) = (0,0,0,1)
		[HDR]_Color12Emmision("Color 12 Emmision", Color) = (0,0,0,1)
		[Header(Gradient)]_Gradient("Gradient", 2D) = "white" {}
		_GradientIntensity("Gradient Intensity", Range( 0 , 1)) = 0.75
		_GradientColor("Gradient Color", Color) = (0,0,0,0)
		_GradientScale("Gradient Scale", Float) = 1
		_GradientOffset("Gradient Offset", Float) = 0
		_GradientPower("Gradient Power", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Color1;
		uniform float4 _Color2;
		uniform float4 _Color3;
		uniform float4 _Color4;
		uniform float4 _Color5;
		uniform float4 _Color6;
		uniform float4 _Color7;
		uniform float4 _Color8;
		uniform float4 _Color9;
		uniform float4 _Color10;
		uniform float4 _Color11;
		uniform float4 _Color12;
		uniform sampler2D _Gradient;
		uniform float4 _Gradient_ST;
		uniform float4 _GradientColor;
		uniform float _GradientIntensity;
		uniform float _GradientScale;
		uniform float _GradientOffset;
		uniform float _GradientPower;
		uniform float4 _Color11Emmision;
		uniform float4 _Color12Emmision;
		uniform float _Metallic;
		uniform float _Smoothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float temp_output_3_0_g273 = 1.0;
			float temp_output_7_0_g273 = 4.0;
			float temp_output_9_0_g273 = 3.0;
			float temp_output_8_0_g273 = 3.0;
			float temp_output_3_0_g268 = 2.0;
			float temp_output_7_0_g268 = 4.0;
			float temp_output_9_0_g268 = 3.0;
			float temp_output_8_0_g268 = 3.0;
			float temp_output_3_0_g275 = 3.0;
			float temp_output_7_0_g275 = 4.0;
			float temp_output_9_0_g275 = 3.0;
			float temp_output_8_0_g275 = 3.0;
			float temp_output_3_0_g271 = 4.0;
			float temp_output_7_0_g271 = 4.0;
			float temp_output_9_0_g271 = 3.0;
			float temp_output_8_0_g271 = 3.0;
			float temp_output_3_0_g269 = 1.0;
			float temp_output_7_0_g269 = 4.0;
			float temp_output_9_0_g269 = 2.0;
			float temp_output_8_0_g269 = 3.0;
			float temp_output_3_0_g272 = 2.0;
			float temp_output_7_0_g272 = 4.0;
			float temp_output_9_0_g272 = 2.0;
			float temp_output_8_0_g272 = 3.0;
			float temp_output_3_0_g276 = 3.0;
			float temp_output_7_0_g276 = 4.0;
			float temp_output_9_0_g276 = 2.0;
			float temp_output_8_0_g276 = 3.0;
			float temp_output_3_0_g274 = 4.0;
			float temp_output_7_0_g274 = 4.0;
			float temp_output_9_0_g274 = 2.0;
			float temp_output_8_0_g274 = 3.0;
			float temp_output_3_0_g270 = 1.0;
			float temp_output_7_0_g270 = 4.0;
			float temp_output_9_0_g270 = 1.0;
			float temp_output_8_0_g270 = 3.0;
			float temp_output_3_0_g277 = 2.0;
			float temp_output_7_0_g277 = 4.0;
			float temp_output_9_0_g277 = 1.0;
			float temp_output_8_0_g277 = 3.0;
			float temp_output_3_0_g238 = 3.0;
			float temp_output_7_0_g238 = 4.0;
			float temp_output_9_0_g238 = 1.0;
			float temp_output_8_0_g238 = 3.0;
			float4 temp_output_241_0 = ( float4( 1,1,1,1 ) * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g238 - 1.0 ) / temp_output_7_0_g238 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g238 / temp_output_7_0_g238 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g238 - 1.0 ) / temp_output_8_0_g238 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g238 / temp_output_8_0_g238 ) ) * 1.0 ) ) ) );
			float temp_output_3_0_g239 = 4.0;
			float temp_output_7_0_g239 = 4.0;
			float temp_output_9_0_g239 = 1.0;
			float temp_output_8_0_g239 = 3.0;
			float4 temp_output_230_0 = ( float4( 1,1,1,1 ) * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g239 - 1.0 ) / temp_output_7_0_g239 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g239 / temp_output_7_0_g239 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g239 - 1.0 ) / temp_output_8_0_g239 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g239 / temp_output_8_0_g239 ) ) * 1.0 ) ) ) );
			float4 temp_output_155_0 = ( ( ( _Color1 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g273 - 1.0 ) / temp_output_7_0_g273 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g273 / temp_output_7_0_g273 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g273 - 1.0 ) / temp_output_8_0_g273 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g273 / temp_output_8_0_g273 ) ) * 1.0 ) ) ) ) + ( _Color2 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g268 - 1.0 ) / temp_output_7_0_g268 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g268 / temp_output_7_0_g268 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g268 - 1.0 ) / temp_output_8_0_g268 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g268 / temp_output_8_0_g268 ) ) * 1.0 ) ) ) ) + ( _Color3 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g275 - 1.0 ) / temp_output_7_0_g275 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g275 / temp_output_7_0_g275 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g275 - 1.0 ) / temp_output_8_0_g275 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g275 / temp_output_8_0_g275 ) ) * 1.0 ) ) ) ) + ( _Color4 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g271 - 1.0 ) / temp_output_7_0_g271 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g271 / temp_output_7_0_g271 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g271 - 1.0 ) / temp_output_8_0_g271 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g271 / temp_output_8_0_g271 ) ) * 1.0 ) ) ) ) ) + ( ( _Color5 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g269 - 1.0 ) / temp_output_7_0_g269 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g269 / temp_output_7_0_g269 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g269 - 1.0 ) / temp_output_8_0_g269 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g269 / temp_output_8_0_g269 ) ) * 1.0 ) ) ) ) + ( _Color6 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g272 - 1.0 ) / temp_output_7_0_g272 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g272 / temp_output_7_0_g272 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g272 - 1.0 ) / temp_output_8_0_g272 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g272 / temp_output_8_0_g272 ) ) * 1.0 ) ) ) ) + ( _Color7 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g276 - 1.0 ) / temp_output_7_0_g276 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g276 / temp_output_7_0_g276 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g276 - 1.0 ) / temp_output_8_0_g276 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g276 / temp_output_8_0_g276 ) ) * 1.0 ) ) ) ) + ( _Color8 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g274 - 1.0 ) / temp_output_7_0_g274 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g274 / temp_output_7_0_g274 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g274 - 1.0 ) / temp_output_8_0_g274 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g274 / temp_output_8_0_g274 ) ) * 1.0 ) ) ) ) ) + ( ( _Color9 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g270 - 1.0 ) / temp_output_7_0_g270 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g270 / temp_output_7_0_g270 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g270 - 1.0 ) / temp_output_8_0_g270 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g270 / temp_output_8_0_g270 ) ) * 1.0 ) ) ) ) + ( _Color10 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g277 - 1.0 ) / temp_output_7_0_g277 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g277 / temp_output_7_0_g277 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g277 - 1.0 ) / temp_output_8_0_g277 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g277 / temp_output_8_0_g277 ) ) * 1.0 ) ) ) ) + ( temp_output_241_0 * _Color11 ) + ( temp_output_230_0 * _Color12 ) ) );
			float2 uv_Gradient = i.uv_texcoord * _Gradient_ST.xy + _Gradient_ST.zw;
			float4 clampResult206 = clamp( ( ( tex2D( _Gradient, uv_Gradient ) + _GradientColor ) + ( 1.0 - _GradientIntensity ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 temp_cast_0 = (_GradientPower).xxxx;
			float4 clampResult255 = clamp( pow( (clampResult206*_GradientScale + _GradientOffset) , temp_cast_0 ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
			o.Albedo = ( temp_output_155_0 * clampResult255 ).rgb;
			o.Emission = ( ( _Color11Emmision * temp_output_241_0 ) + ( _Color12Emmision * temp_output_230_0 ) ).rgb;
			o.Metallic = _Metallic;
			o.Smoothness = ( (temp_output_155_0).a * _Smoothness );
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16200
250;158;1189;592;569.5203;1314.413;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;201;-43.05084,-721.7265;Float;False;Property;_GradientIntensity;Gradient Intensity;17;0;Create;True;0;0;False;0;0.75;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;202;-41.02644,-1165.228;Float;True;Property;_Gradient;Gradient;16;0;Create;True;0;0;False;1;Header(Gradient);0f424a347039ef447a763d3d4b4782b0;0f424a347039ef447a763d3d4b4782b0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;200;-5.396437,-926.7093;Float;False;Property;_GradientColor;Gradient Color;18;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;204;301.5615,-792.5283;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;203;328.2687,-922.1614;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;180;-168.6213,2174.434;Float;False;Property;_Color11;Color 11;10;0;Create;True;0;0;False;0;1,0.1544118,0.381846,0.316;1,0.1544118,0.381846,0.316;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;181;-202.6826,1954.387;Float;False;Property;_Color10;Color 10;9;0;Create;True;0;0;False;0;0.1544118,0.1602434,1,0.341;0.1544118,0.1602434,1,0.341;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;154;-195.6228,411.2479;Float;False;Property;_Color4;Color 4;3;0;Create;True;0;0;False;0;0.1544118,0.5451319,1,0.472;0.1544118,0.5451319,1,0.472;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;157;-182.3802,1181.25;Float;False;Property;_Color7;Color 7;6;0;Create;True;0;0;False;0;0.1544118,0.6151115,1,0.316;0.1544118,0.6151115,1,0.316;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;183;-194.742,1695.03;Float;False;Property;_Color9;Color 9;8;0;Create;True;0;0;False;1;Header(Row 3);0.9099331,0.9264706,0.6267301,0.353;0.9099331,0.9264706,0.6267301,0.353;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;230;-239.2491,2921.615;Float;True;ColorShartSlot;-1;;239;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;4;False;9;FLOAT;1;False;7;FLOAT;4;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;241;-237.0968,2697.951;Float;True;ColorShartSlot;-1;;238;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;3;False;9;FLOAT;1;False;7;FLOAT;4;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;205;508.7686,-952.5815;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;150;-207.7412,-66.93771;Float;False;Property;_Color2;Color 2;1;0;Create;True;0;0;False;0;1,0.1544118,0.8017241,0.334;1,0.1544118,0.8017241,0.334;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;182;-183.4034,2422.875;Float;False;Property;_Color12;Color 12;11;0;Create;True;0;0;False;0;0.02270761,0.1632713,0.2205882,0.484;0.02270761,0.1632713,0.2205882,0.484;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;23;-199.8005,-326.2955;Float;False;Property;_Color1;Color 1;0;0;Create;True;0;0;False;1;Header(Row 1);1,0.1544118,0.1544118,0.397;1,0.1544118,0.1544118,0.397;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;156;-195.9079,947.3851;Float;False;Property;_Color6;Color 6;5;0;Create;True;0;0;False;0;0.2669384,0.3207547,0.0226949,0.341;0.8483773,1,0.1544118,0.341;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;158;-183.7895,1424.406;Float;False;Property;_Color8;Color 8;7;0;Create;True;0;0;False;0;0.4849697,0.5008695,0.5073529,0.484;0.4849697,0.5008695,0.5073529,0.484;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;159;-187.9672,688.0273;Float;False;Property;_Color5;Color 5;4;0;Create;True;0;0;False;1;Header(Row 2);0.9533468,1,0.1544118,0.353;0.9533468,1,0.1544118,0.353;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;152;-194.2135,166.9271;Float;False;Property;_Color3;Color 3;2;0;Create;True;0;0;False;0;0.2535501,0.1544118,1,0.228;0.2535501,0.1544118,1,0.228;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;236;107.9764,-66.86263;Float;True;ColorShartSlot;-1;;268;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;2;False;9;FLOAT;3;False;7;FLOAT;4;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;207;585.6387,-538.9446;Float;False;Property;_GradientScale;Gradient Scale;19;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;208;591.5417,-443.1692;Float;False;Property;_GradientOffset;Gradient Offset;20;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;206;793.5166,-914.7413;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;233;127.7504,688.1025;Float;True;ColorShartSlot;-1;;269;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;1;False;9;FLOAT;2;False;7;FLOAT;4;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;235;194.8606,1678.942;Float;True;ColorShartSlot;-1;;270;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;1;False;9;FLOAT;1;False;7;FLOAT;4;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;212;226.7271,2394.801;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;211;217.2,2168.927;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;239;115.9171,-326.2204;Float;True;ColorShartSlot;-1;;273;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;1;False;9;FLOAT;3;False;7;FLOAT;4;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;234;133.8517,1424.481;Float;True;ColorShartSlot;-1;;274;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;4;False;9;FLOAT;2;False;7;FLOAT;4;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;238;121.5042,162.4284;Float;True;ColorShartSlot;-1;;275;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;3;False;9;FLOAT;3;False;7;FLOAT;4;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;240;133.3375,1181.325;Float;True;ColorShartSlot;-1;;276;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;3;False;9;FLOAT;2;False;7;FLOAT;4;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;232;186.9198,1938.3;Float;True;ColorShartSlot;-1;;277;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;2;False;9;FLOAT;1;False;7;FLOAT;4;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;231;119.8096,945.1734;Float;True;ColorShartSlot;-1;;272;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;2;False;9;FLOAT;2;False;7;FLOAT;4;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;237;122.0185,410.1585;Float;True;ColorShartSlot;-1;;271;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;4;False;9;FLOAT;3;False;7;FLOAT;4;False;8;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;193;1126.266,334.7972;Float;True;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;146;1124.026,-170.6852;Float;True;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;164;1130.732,57.40811;Float;True;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;209;1091.96,-605.7403;Float;True;3;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;253;1474.609,-467.7142;Float;False;Property;_GradientPower;Gradient Power;21;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;195;1135.674,583.2192;Float;False;Property;_Color11Emmision;Color 11 Emmision;14;1;[HDR];Create;True;0;0;False;1;Header(Emmision);0,0,0,1;0,0,0,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;217;1070.367,2661.439;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;216;925.6725,2423.552;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;155;1378.894,-29.6249;Float;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;196;1143.088,753.1885;Float;False;Property;_Color12Emmision;Color 12 Emmision;15;1;[HDR];Create;True;0;0;False;0;0,0,0,1;0,0,0,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;254;1704.252,-572.7532;Float;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;166;1413.004,514.6777;Float;False;Property;_Smoothness;Smoothness;12;0;Create;True;0;0;False;1;Header(Smoothness (Alphas));1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;214;1476.114,845.3589;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;213;1470.311,715.3326;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;194;1516.74,353.2244;Float;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;255;2050.583,-493.1835;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;165;1691.967,238.6589;Float;False;Property;_Metallic;Metallic;13;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;210;1843.751,-118.5323;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;178;1752.065,420.4065;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;199;1775.644,656.0661;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2076.697,169.3291;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Malbers/Color4x3;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;204;0;201;0
WireConnection;203;0;202;0
WireConnection;203;1;200;0
WireConnection;205;0;203;0
WireConnection;205;1;204;0
WireConnection;236;38;150;0
WireConnection;206;0;205;0
WireConnection;233;38;159;0
WireConnection;235;38;183;0
WireConnection;212;0;230;0
WireConnection;212;1;182;0
WireConnection;211;0;241;0
WireConnection;211;1;180;0
WireConnection;239;38;23;0
WireConnection;234;38;158;0
WireConnection;238;38;152;0
WireConnection;240;38;157;0
WireConnection;232;38;181;0
WireConnection;231;38;156;0
WireConnection;237;38;154;0
WireConnection;193;0;235;0
WireConnection;193;1;232;0
WireConnection;193;2;211;0
WireConnection;193;3;212;0
WireConnection;146;0;239;0
WireConnection;146;1;236;0
WireConnection;146;2;238;0
WireConnection;146;3;237;0
WireConnection;164;0;233;0
WireConnection;164;1;231;0
WireConnection;164;2;240;0
WireConnection;164;3;234;0
WireConnection;209;0;206;0
WireConnection;209;1;207;0
WireConnection;209;2;208;0
WireConnection;217;0;230;0
WireConnection;216;0;241;0
WireConnection;155;0;146;0
WireConnection;155;1;164;0
WireConnection;155;2;193;0
WireConnection;254;0;209;0
WireConnection;254;1;253;0
WireConnection;214;0;196;0
WireConnection;214;1;217;0
WireConnection;213;0;195;0
WireConnection;213;1;216;0
WireConnection;194;0;155;0
WireConnection;255;0;254;0
WireConnection;210;0;155;0
WireConnection;210;1;255;0
WireConnection;178;0;194;0
WireConnection;178;1;166;0
WireConnection;199;0;213;0
WireConnection;199;1;214;0
WireConnection;0;0;210;0
WireConnection;0;2;199;0
WireConnection;0;3;165;0
WireConnection;0;4;178;0
ASEEND*/
//CHKSM=F2C43DF2D707A2E02E5FFF74CF73765ECE1334A9