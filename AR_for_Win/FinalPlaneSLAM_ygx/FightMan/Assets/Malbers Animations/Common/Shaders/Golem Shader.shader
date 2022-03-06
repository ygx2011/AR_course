// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Malbers/Golem PA"
{
	Properties
	{
		_EmissionMask("Emission Mask", 2D) = "white" {}
		_Color9("Color 1", Color) = (0.3773585,0.1940192,0.1940192,1)
		_Color4("Color 2", Color) = (0.2830189,0.2362941,0.2362941,1)
		_Color5("Color 3", Color) = (0.1803922,0.1254902,0.06666667,1)
		_Color7("Color 4", Color) = (0.2352941,0.1764706,0.1019608,1)
		_EmissionPower("Emission Power", Range( 0 , 10)) = 1.300526
		[HDR]_Emissive1("Emissive 1", Color) = (1,0.9011408,0,1)
		[HDR]_Emissive2("Emissive 2", Color) = (1,0,0,0)
		_Metallic("Metallic", Range( 0 , 1)) = 0.2
		_Smoothness("Smoothness", Range( 0 , 1)) = 0.2
		_ShadowColor("Shadow Color", Color) = (0.2075472,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Color9;
		uniform float4 _ShadowColor;
		uniform float4 _Color4;
		uniform float4 _Color5;
		uniform float4 _Color7;
		uniform float4 _Emissive2;
		uniform float4 _Emissive1;
		uniform sampler2D _EmissionMask;
		uniform float4 _EmissionMask_ST;
		uniform float _EmissionPower;
		uniform float _Metallic;
		uniform float _Smoothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 blendOpSrc132 = _Color9;
			float4 blendOpDest132 = _ShadowColor;
			float2 uv_TexCoord11 = i.uv_texcoord * float2( 1,3 ) + float2( 0,-1.5 );
			float4 lerpResult12 = lerp( _Color9 , ( saturate( 2.0f*blendOpDest132*blendOpSrc132 + blendOpDest132*blendOpDest132*(1.0f - 2.0f*blendOpSrc132) )) , uv_TexCoord11.y);
			float temp_output_3_0_g166 = 1.0;
			float temp_output_7_0_g166 = 2.0;
			float temp_output_9_0_g166 = 2.0;
			float temp_output_8_0_g166 = 2.0;
			float4 blendOpSrc131 = _Color4;
			float4 blendOpDest131 = _ShadowColor;
			float4 lerpResult36 = lerp( _Color4 , ( saturate( 2.0f*blendOpDest131*blendOpSrc131 + blendOpDest131*blendOpDest131*(1.0f - 2.0f*blendOpSrc131) )) , uv_TexCoord11.y);
			float temp_output_3_0_g165 = 2.0;
			float temp_output_7_0_g165 = 2.0;
			float temp_output_9_0_g165 = 2.0;
			float temp_output_8_0_g165 = 2.0;
			float4 blendOpSrc133 = _Color5;
			float4 blendOpDest133 = _ShadowColor;
			float2 uv_TexCoord45 = i.uv_texcoord * float2( 1,2 ) + float2( 0,0.2 );
			float4 lerpResult38 = lerp( _Color5 , ( saturate( 2.0f*blendOpDest133*blendOpSrc133 + blendOpDest133*blendOpDest133*(1.0f - 2.0f*blendOpSrc133) )) , uv_TexCoord45.y);
			float temp_output_3_0_g163 = 1.0;
			float temp_output_7_0_g163 = 2.0;
			float temp_output_9_0_g163 = 1.0;
			float temp_output_8_0_g163 = 2.0;
			float4 blendOpSrc134 = _Color7;
			float4 blendOpDest134 = _ShadowColor;
			float4 lerpResult41 = lerp( _Color7 , ( saturate( 2.0f*blendOpDest134*blendOpSrc134 + blendOpDest134*blendOpDest134*(1.0f - 2.0f*blendOpSrc134) )) , uv_TexCoord45.y);
			float temp_output_3_0_g164 = 2.0;
			float temp_output_7_0_g164 = 2.0;
			float temp_output_9_0_g164 = 1.0;
			float temp_output_8_0_g164 = 2.0;
			o.Albedo = ( ( lerpResult12 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g166 - 1.0 ) / temp_output_7_0_g166 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g166 / temp_output_7_0_g166 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g166 - 1.0 ) / temp_output_8_0_g166 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g166 / temp_output_8_0_g166 ) ) * 1.0 ) ) ) ) + ( lerpResult36 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g165 - 1.0 ) / temp_output_7_0_g165 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g165 / temp_output_7_0_g165 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g165 - 1.0 ) / temp_output_8_0_g165 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g165 / temp_output_8_0_g165 ) ) * 1.0 ) ) ) ) + ( lerpResult38 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g163 - 1.0 ) / temp_output_7_0_g163 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g163 / temp_output_7_0_g163 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g163 - 1.0 ) / temp_output_8_0_g163 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g163 / temp_output_8_0_g163 ) ) * 1.0 ) ) ) ) + ( lerpResult41 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g164 - 1.0 ) / temp_output_7_0_g164 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g164 / temp_output_7_0_g164 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g164 - 1.0 ) / temp_output_8_0_g164 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g164 / temp_output_8_0_g164 ) ) * 1.0 ) ) ) ) ).rgb;
			float4 color117 = IsGammaSpace() ? float4(0,0,0,1) : float4(0,0,0,1);
			float2 uv_TexCoord109 = i.uv_texcoord * float2( 1,3 ) + float2( 0,-0.85 );
			float4 lerpResult113 = lerp( _Emissive2 , _Emissive1 , uv_TexCoord109.y);
			float temp_output_3_0_g160 = 2.0;
			float temp_output_7_0_g160 = 2.0;
			float temp_output_9_0_g160 = 1.0;
			float temp_output_8_0_g160 = 2.0;
			float2 uv_TexCoord110 = i.uv_texcoord * float2( 1,3 ) + float2( 0,-2.3 );
			float4 lerpResult114 = lerp( _Emissive2 , _Emissive1 , uv_TexCoord110.y);
			float temp_output_3_0_g159 = 1.0;
			float temp_output_7_0_g159 = 1.0;
			float temp_output_9_0_g159 = 2.0;
			float temp_output_8_0_g159 = 2.0;
			float2 uv_EmissionMask = i.uv_texcoord * _EmissionMask_ST.xy + _EmissionMask_ST.zw;
			float4 lerpResult116 = lerp( color117 , ( ( lerpResult113 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g160 - 1.0 ) / temp_output_7_0_g160 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g160 / temp_output_7_0_g160 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g160 - 1.0 ) / temp_output_8_0_g160 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g160 / temp_output_8_0_g160 ) ) * 1.0 ) ) ) ) + ( lerpResult114 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g159 - 1.0 ) / temp_output_7_0_g159 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g159 / temp_output_7_0_g159 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g159 - 1.0 ) / temp_output_8_0_g159 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g159 / temp_output_8_0_g159 ) ) * 1.0 ) ) ) ) ) , tex2D( _EmissionMask, uv_EmissionMask ));
			o.Emission = ( lerpResult116 * _EmissionPower ).rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16200
250;158;1189;592;5445.67;167.3775;1.748342;True;True
Node;AmplifyShaderEditor.CommentaryNode;123;-5461.668,-146.388;Float;False;2676.736;1223;Emission Color;14;4;117;115;112;110;109;107;116;111;114;113;93;90;89;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;107;-5063.803,278.551;Float;False;Property;_Emissive2;Emissive 2;7;1;[HDR];Create;True;0;0;False;0;1,0,0,0;0,1,0.05882359,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;122;-5506.075,-2539.248;Float;False;2185.29;2302.507;Color Change;19;34;28;32;30;41;36;12;38;11;45;133;132;134;131;130;42;124;39;37;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;93;-5075.194,-107.0081;Float;False;Property;_Emissive1;Emissive 1;6;1;[HDR];Create;True;0;0;False;0;1,0.9011408,0,1;0.9716981,0.8475578,0.2429245,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;109;-5396.666,60.05534;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,3;False;1;FLOAT2;0,-0.85;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;110;-5004.979,529.7918;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,3;False;1;FLOAT2;0,-2.3;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;42;-5133.027,-542.8691;Float;False;Property;_Color7;Color 4;4;0;Create;False;0;0;False;0;0.2352941,0.1764706,0.1019608,1;0.04647709,0.09433961,0.03604485,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;130;-5440.474,-1466.613;Float;False;Property;_ShadowColor;Shadow Color;10;0;Create;True;0;0;False;0;0.2075472,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;37;-4955.637,-1757.225;Float;False;Property;_Color4;Color 2;2;0;Create;False;0;0;False;0;0.2830189,0.2362941,0.2362941,1;0.3113208,0.2346794,0.1101371,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;39;-4849.5,-1346.411;Float;False;Property;_Color5;Color 3;3;0;Create;False;0;0;False;0;0.1803922,0.1254902,0.06666667,1;0.3018868,0.2765508,0.1067996,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;114;-4646.32,524.4437;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;113;-4595.292,103.4534;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;124;-5329.247,-2423.364;Float;False;Property;_Color9;Color 1;1;0;Create;False;0;0;False;0;0.3773585,0.1940192,0.1940192,1;0.5566038,0.4024955,0.1654058,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;111;-4287.597,152.9065;Float;True;ColorShartSlot;-1;;160;8287b10e189ac1e4f80ef7e89903de17;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;2;False;9;FLOAT;1;False;7;FLOAT;2;False;8;FLOAT;2;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;132;-5009.346,-2204.656;Float;False;SoftLight;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;133;-4643.902,-1053.423;Float;False;SoftLight;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;134;-4740.105,-593.6027;Float;False;SoftLight;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-4592.587,-2029.296;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,3;False;1;FLOAT2;0,-1.5;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;131;-4547.071,-1650.359;Float;False;SoftLight;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;112;-4286.345,452.8255;Float;True;ColorShartSlot;-1;;159;8287b10e189ac1e4f80ef7e89903de17;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;1;False;9;FLOAT;2;False;7;FLOAT;1;False;8;FLOAT;2;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;45;-4437.104,-812.1178;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,2;False;1;FLOAT2;0,0.2;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;41;-4155.988,-529.4528;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;38;-4175.363,-1161.572;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;117;-3538.094,219.3647;Float;False;Constant;_Back_color;Back_color;11;0;Create;True;0;0;False;0;0,0,0,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;36;-4247.949,-1767.308;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;115;-3894.661,254.8818;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;12;-4313.527,-2287.664;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;4;-3711.038,655.5321;Float;True;Property;_EmissionMask;Emission Mask;0;0;Create;True;0;0;False;0;0e90178a9c0b464408857a81b53fc7ac;0e90178a9c0b464408857a81b53fc7ac;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;32;-3666.441,-1221.775;Float;True;ColorShartSlot;-1;;163;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;1;False;9;FLOAT;1;False;7;FLOAT;2;False;8;FLOAT;2;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;28;-3891.893,-2393.558;Float;True;ColorShartSlot;-1;;166;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;1;False;9;FLOAT;2;False;7;FLOAT;2;False;8;FLOAT;2;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;30;-3907.104,-1760.635;Float;True;ColorShartSlot;-1;;165;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;2;False;9;FLOAT;2;False;7;FLOAT;2;False;8;FLOAT;2;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;90;-3365.014,859.0638;Float;False;Property;_EmissionPower;Emission Power;5;0;Create;True;0;0;False;0;1.300526;2;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;34;-3661.704,-522.8755;Float;True;ColorShartSlot;-1;;164;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;2;False;9;FLOAT;1;False;7;FLOAT;2;False;8;FLOAT;2;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;116;-3321.71,604.5768;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;120;-2929.028,-855.2371;Float;False;Property;_Smoothness;Smoothness;9;0;Create;True;0;0;False;0;0.2;0.479;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;135;-2925.677,-942.8919;Float;False;Property;_Metallic;Metallic;8;0;Create;True;0;0;False;0;0.2;0.049;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;26;-3006.768,-1173.486;Float;True;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;89;-3011.971,844.848;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;2;-2558.082,-1216.606;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Malbers/Golem PA;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;114;0;107;0
WireConnection;114;1;93;0
WireConnection;114;2;110;2
WireConnection;113;0;107;0
WireConnection;113;1;93;0
WireConnection;113;2;109;2
WireConnection;111;38;113;0
WireConnection;132;0;124;0
WireConnection;132;1;130;0
WireConnection;133;0;39;0
WireConnection;133;1;130;0
WireConnection;134;0;42;0
WireConnection;134;1;130;0
WireConnection;131;0;37;0
WireConnection;131;1;130;0
WireConnection;112;38;114;0
WireConnection;41;0;42;0
WireConnection;41;1;134;0
WireConnection;41;2;45;2
WireConnection;38;0;39;0
WireConnection;38;1;133;0
WireConnection;38;2;45;2
WireConnection;36;0;37;0
WireConnection;36;1;131;0
WireConnection;36;2;11;2
WireConnection;115;0;111;0
WireConnection;115;1;112;0
WireConnection;12;0;124;0
WireConnection;12;1;132;0
WireConnection;12;2;11;2
WireConnection;32;38;38;0
WireConnection;28;38;12;0
WireConnection;30;38;36;0
WireConnection;34;38;41;0
WireConnection;116;0;117;0
WireConnection;116;1;115;0
WireConnection;116;2;4;0
WireConnection;26;0;28;0
WireConnection;26;1;30;0
WireConnection;26;2;32;0
WireConnection;26;3;34;0
WireConnection;89;0;116;0
WireConnection;89;1;90;0
WireConnection;2;0;26;0
WireConnection;2;2;89;0
WireConnection;2;3;135;0
WireConnection;2;4;120;0
ASEEND*/
//CHKSM=F65326A597FBA7465344310B4D0455DF5C871C62