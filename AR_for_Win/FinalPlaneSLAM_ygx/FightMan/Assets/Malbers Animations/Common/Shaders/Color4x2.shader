// Upgrade NOTE: upgraded instancing buffer 'MalbersColor4x2' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Malbers/Color4x2"
{
	Properties
	{
		_Color1("Color 1", Color) = (1,0.1544118,0.1544118,0.397)
		_Color2("Color 2", Color) = (1,0.1544118,0.8017241,0.334)
		_Color3("Color 3", Color) = (0.2535501,0.1544118,1,0.228)
		_Color4("Color 4", Color) = (0.1544118,0.5451319,1,0.472)
		_Color5("Color 5", Color) = (0.9533468,1,0.1544118,0.353)
		_Color6("Color 6", Color) = (0.8483773,1,0.1544118,0.341)
		_Color7("Color 7", Color) = (0.1544118,0.6151115,1,0.316)
		_Color8("Color 8", Color) = (0.4849697,0.5008695,0.5073529,0.484)
		_Smoothness("Smoothness", Range( 0 , 1)) = 1
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_GradientColor("Gradient Color", Color) = (0,0,0,0)
		_GradientIntensity("Gradient Intensity", Range( 0 , 1)) = 0.75
		[Toggle(_USINGGRADIENT_ON)] _UsingGradient("Using Gradient", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma shader_feature _USINGGRADIENT_ON
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _GradientColor;
		uniform float _GradientIntensity;
		uniform float _Metallic;
		uniform float _Smoothness;

		UNITY_INSTANCING_BUFFER_START(MalbersColor4x2)
			UNITY_DEFINE_INSTANCED_PROP(float4, _Color1)
#define _Color1_arr MalbersColor4x2
			UNITY_DEFINE_INSTANCED_PROP(float4, _Color2)
#define _Color2_arr MalbersColor4x2
			UNITY_DEFINE_INSTANCED_PROP(float4, _Color3)
#define _Color3_arr MalbersColor4x2
			UNITY_DEFINE_INSTANCED_PROP(float4, _Color4)
#define _Color4_arr MalbersColor4x2
			UNITY_DEFINE_INSTANCED_PROP(float4, _Color5)
#define _Color5_arr MalbersColor4x2
			UNITY_DEFINE_INSTANCED_PROP(float4, _Color6)
#define _Color6_arr MalbersColor4x2
			UNITY_DEFINE_INSTANCED_PROP(float4, _Color7)
#define _Color7_arr MalbersColor4x2
			UNITY_DEFINE_INSTANCED_PROP(float4, _Color8)
#define _Color8_arr MalbersColor4x2
		UNITY_INSTANCING_BUFFER_END(MalbersColor4x2)

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 _Color1_Instance = UNITY_ACCESS_INSTANCED_PROP(_Color1_arr, _Color1);
			float temp_output_3_0_g202 = 1.0;
			float temp_output_7_0_g202 = 4.0;
			float temp_output_9_0_g202 = 2.0;
			float temp_output_8_0_g202 = 2.0;
			float4 _Color2_Instance = UNITY_ACCESS_INSTANCED_PROP(_Color2_arr, _Color2);
			float temp_output_3_0_g201 = 2.0;
			float temp_output_7_0_g201 = 4.0;
			float temp_output_9_0_g201 = 2.0;
			float temp_output_8_0_g201 = 2.0;
			float4 _Color3_Instance = UNITY_ACCESS_INSTANCED_PROP(_Color3_arr, _Color3);
			float temp_output_3_0_g205 = 3.0;
			float temp_output_7_0_g205 = 4.0;
			float temp_output_9_0_g205 = 2.0;
			float temp_output_8_0_g205 = 2.0;
			float4 _Color4_Instance = UNITY_ACCESS_INSTANCED_PROP(_Color4_arr, _Color4);
			float temp_output_3_0_g198 = 4.0;
			float temp_output_7_0_g198 = 4.0;
			float temp_output_9_0_g198 = 2.0;
			float temp_output_8_0_g198 = 2.0;
			float4 _Color5_Instance = UNITY_ACCESS_INSTANCED_PROP(_Color5_arr, _Color5);
			float temp_output_3_0_g204 = 1.0;
			float temp_output_7_0_g204 = 4.0;
			float temp_output_9_0_g204 = 1.0;
			float temp_output_8_0_g204 = 2.0;
			float4 _Color6_Instance = UNITY_ACCESS_INSTANCED_PROP(_Color6_arr, _Color6);
			float temp_output_3_0_g200 = 2.0;
			float temp_output_7_0_g200 = 4.0;
			float temp_output_9_0_g200 = 1.0;
			float temp_output_8_0_g200 = 2.0;
			float4 _Color7_Instance = UNITY_ACCESS_INSTANCED_PROP(_Color7_arr, _Color7);
			float temp_output_3_0_g203 = 3.0;
			float temp_output_7_0_g203 = 4.0;
			float temp_output_9_0_g203 = 1.0;
			float temp_output_8_0_g203 = 2.0;
			float4 _Color8_Instance = UNITY_ACCESS_INSTANCED_PROP(_Color8_arr, _Color8);
			float temp_output_3_0_g199 = 4.0;
			float temp_output_7_0_g199 = 4.0;
			float temp_output_9_0_g199 = 1.0;
			float temp_output_8_0_g199 = 2.0;
			float4 temp_output_155_0 = ( ( ( _Color1_Instance * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g202 - 1.0 ) / temp_output_7_0_g202 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g202 / temp_output_7_0_g202 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g202 - 1.0 ) / temp_output_8_0_g202 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g202 / temp_output_8_0_g202 ) ) * 1.0 ) ) ) ) + ( _Color2_Instance * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g201 - 1.0 ) / temp_output_7_0_g201 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g201 / temp_output_7_0_g201 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g201 - 1.0 ) / temp_output_8_0_g201 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g201 / temp_output_8_0_g201 ) ) * 1.0 ) ) ) ) + ( _Color3_Instance * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g205 - 1.0 ) / temp_output_7_0_g205 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g205 / temp_output_7_0_g205 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g205 - 1.0 ) / temp_output_8_0_g205 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g205 / temp_output_8_0_g205 ) ) * 1.0 ) ) ) ) + ( _Color4_Instance * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g198 - 1.0 ) / temp_output_7_0_g198 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g198 / temp_output_7_0_g198 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g198 - 1.0 ) / temp_output_8_0_g198 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g198 / temp_output_8_0_g198 ) ) * 1.0 ) ) ) ) ) + ( ( _Color5_Instance * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g204 - 1.0 ) / temp_output_7_0_g204 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g204 / temp_output_7_0_g204 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g204 - 1.0 ) / temp_output_8_0_g204 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g204 / temp_output_8_0_g204 ) ) * 1.0 ) ) ) ) + ( _Color6_Instance * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g200 - 1.0 ) / temp_output_7_0_g200 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g200 / temp_output_7_0_g200 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g200 - 1.0 ) / temp_output_8_0_g200 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g200 / temp_output_8_0_g200 ) ) * 1.0 ) ) ) ) + ( _Color7_Instance * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g203 - 1.0 ) / temp_output_7_0_g203 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g203 / temp_output_7_0_g203 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g203 - 1.0 ) / temp_output_8_0_g203 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g203 / temp_output_8_0_g203 ) ) * 1.0 ) ) ) ) + ( _Color8_Instance * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g199 - 1.0 ) / temp_output_7_0_g199 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g199 / temp_output_7_0_g199 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g199 - 1.0 ) / temp_output_8_0_g199 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g199 / temp_output_8_0_g199 ) ) * 1.0 ) ) ) ) ) );
			float4 ColorShart214 = temp_output_155_0;
			float4 temp_cast_0 = (1.0).xxxx;
			float4 temp_cast_1 = ((i.uv_texcoord.y*2.0 + -1.0)).xxxx;
			float temp_output_3_0_g190 = 1.0;
			float temp_output_7_0_g190 = 1.0;
			float temp_output_9_0_g190 = 2.0;
			float temp_output_8_0_g190 = 2.0;
			float4 temp_cast_2 = ((i.uv_texcoord.y*2.0 + 0.0)).xxxx;
			float temp_output_3_0_g191 = 1.0;
			float temp_output_7_0_g191 = 1.0;
			float temp_output_9_0_g191 = 1.0;
			float temp_output_8_0_g191 = 2.0;
			float4 clampResult224 = clamp( ( ( ( ( temp_cast_1 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g190 - 1.0 ) / temp_output_7_0_g190 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g190 / temp_output_7_0_g190 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g190 - 1.0 ) / temp_output_8_0_g190 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g190 / temp_output_8_0_g190 ) ) * 1.0 ) ) ) ) + ( temp_cast_2 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g191 - 1.0 ) / temp_output_7_0_g191 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g191 / temp_output_7_0_g191 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g191 - 1.0 ) / temp_output_8_0_g191 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g191 / temp_output_8_0_g191 ) ) * 1.0 ) ) ) ) ) + _GradientColor ) + ( 1.0 - _GradientIntensity ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			#ifdef _USINGGRADIENT_ON
				float4 staticSwitch228 = clampResult224;
			#else
				float4 staticSwitch228 = temp_cast_0;
			#endif
			o.Albedo = ( ColorShart214 * staticSwitch228 ).rgb;
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
Version=18100
14;46;1372;789;-120.304;-505.2512;1.3;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;181;760.072,954.4623;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScaleAndOffsetNode;197;1054.585,1107.109;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;2;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;196;1059.348,878.5837;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;2;False;2;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;159;-187.9672,688.0273;Float;False;InstancedProperty;_Color5;Color 5;4;0;Create;True;0;0;False;0;False;0.9533468,1,0.1544118,0.353;0.9533468,1,0.1544118,0.353;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;150;-207.7412,-66.93771;Float;False;InstancedProperty;_Color2;Color 2;1;0;Create;True;0;0;False;0;False;1,0.1544118,0.8017241,0.334;1,0.1544118,0.8017241,0.334;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;23;-199.8005,-326.2955;Float;False;InstancedProperty;_Color1;Color 1;0;0;Create;True;0;0;False;0;False;1,0.1544118,0.1544118,0.397;1,0.1544118,0.1544118,0.397;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;157;-182.3802,1181.25;Float;False;InstancedProperty;_Color7;Color 7;6;0;Create;True;0;0;False;0;False;0.1544118,0.6151115,1,0.316;0.1544118,0.6151115,1,0.316;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;156;-195.9079,947.3851;Float;False;InstancedProperty;_Color6;Color 6;5;0;Create;True;0;0;False;0;False;0.8483773,1,0.1544118,0.341;0.8483773,1,0.1544118,0.341;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;195;1328.392,1096.723;Inherit;True;ColorShartSlot;-1;;191;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;1;False;9;FLOAT;1;False;7;FLOAT;1;False;8;FLOAT;2;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;193;1327.674,876.1544;Inherit;True;ColorShartSlot;-1;;190;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;1;False;9;FLOAT;2;False;7;FLOAT;1;False;8;FLOAT;2;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;158;-183.7895,1424.406;Float;False;InstancedProperty;_Color8;Color 8;7;0;Create;True;0;0;False;0;False;0.4849697,0.5008695,0.5073529,0.484;0.4849697,0.5008695,0.5073529,0.484;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;152;-194.2135,166.9271;Float;False;InstancedProperty;_Color3;Color 3;2;0;Create;True;0;0;False;0;False;0.2535501,0.1544118,1,0.228;0.2535501,0.1544118,1,0.228;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;154;-195.6228,411.2479;Float;False;InstancedProperty;_Color4;Color 4;3;0;Create;True;0;0;False;0;False;0.1544118,0.5451319,1,0.472;0.1544118,0.5451319,1,0.472;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;201;1697.634,987.8407;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;162;133.8517,1424.481;Inherit;True;ColorShartSlot;-1;;199;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;4;False;9;FLOAT;1;False;7;FLOAT;4;False;8;FLOAT;2;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;205;1722.708,1223.28;Float;False;Property;_GradientColor;Gradient Color;10;0;Create;True;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;202;1672.228,1423.309;Float;False;Property;_GradientIntensity;Gradient Intensity;11;0;Create;True;0;0;False;0;False;0.75;0.75;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;160;119.8096,947.4603;Inherit;True;ColorShartSlot;-1;;200;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;2;False;9;FLOAT;1;False;7;FLOAT;4;False;8;FLOAT;2;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;145;115.9171,-326.2204;Inherit;True;ColorShartSlot;-1;;202;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;1;False;9;FLOAT;2;False;7;FLOAT;4;False;8;FLOAT;2;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;149;107.9764,-66.86263;Inherit;True;ColorShartSlot;-1;;201;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;2;False;9;FLOAT;2;False;7;FLOAT;4;False;8;FLOAT;2;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;153;122.0185,410.1585;Inherit;True;ColorShartSlot;-1;;198;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;4;False;9;FLOAT;2;False;7;FLOAT;4;False;8;FLOAT;2;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;163;127.7504,688.1025;Inherit;True;ColorShartSlot;-1;;204;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;1;False;9;FLOAT;1;False;7;FLOAT;4;False;8;FLOAT;2;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;161;133.3375,1181.325;Inherit;True;ColorShartSlot;-1;;203;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;3;False;9;FLOAT;1;False;7;FLOAT;4;False;8;FLOAT;2;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;151;121.5042,167.0022;Inherit;True;ColorShartSlot;-1;;205;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;3;False;9;FLOAT;2;False;7;FLOAT;4;False;8;FLOAT;2;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;146;1124.026,-170.6852;Inherit;True;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;223;2038.081,1279.951;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;225;1996.289,906.7085;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;164;1130.732,57.40811;Inherit;True;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;216;2173.958,1015.755;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;155;1392.04,-26.9957;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;224;2430.454,1001.557;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;227;1941.604,706.101;Float;False;Constant;_Float0;Float 0;13;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;214;2011.677,12.06882;Float;False;ColorShart;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;179;1786.961,259.5521;Inherit;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;166;1287.072,404.6109;Float;False;Property;_Smoothness;Smoothness;8;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;215;2230.119,542.547;Inherit;False;214;ColorShart;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;228;2179.5,767.852;Float;False;Property;_UsingGradient;Using Gradient;12;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;203;2475.923,555.3262;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;165;2015.057,193.266;Float;False;Property;_Metallic;Metallic;9;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;178;2063.372,279.3538;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2856.367,138.5725;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Malbers/Color4x2;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;197;0;181;2
WireConnection;196;0;181;2
WireConnection;195;38;197;0
WireConnection;193;38;196;0
WireConnection;201;0;193;0
WireConnection;201;1;195;0
WireConnection;162;38;158;0
WireConnection;160;38;156;0
WireConnection;145;38;23;0
WireConnection;149;38;150;0
WireConnection;153;38;154;0
WireConnection;163;38;159;0
WireConnection;161;38;157;0
WireConnection;151;38;152;0
WireConnection;146;0;145;0
WireConnection;146;1;149;0
WireConnection;146;2;151;0
WireConnection;146;3;153;0
WireConnection;223;0;202;0
WireConnection;225;0;201;0
WireConnection;225;1;205;0
WireConnection;164;0;163;0
WireConnection;164;1;160;0
WireConnection;164;2;161;0
WireConnection;164;3;162;0
WireConnection;216;0;225;0
WireConnection;216;1;223;0
WireConnection;155;0;146;0
WireConnection;155;1;164;0
WireConnection;224;0;216;0
WireConnection;214;0;155;0
WireConnection;179;0;155;0
WireConnection;228;1;227;0
WireConnection;228;0;224;0
WireConnection;203;0;215;0
WireConnection;203;1;228;0
WireConnection;178;0;179;0
WireConnection;178;1;166;0
WireConnection;0;0;203;0
WireConnection;0;3;165;0
WireConnection;0;4;178;0
ASEEND*/
//CHKSM=6077737B7284372ECA7DCB39FBEBF76DBF937B5E