// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Malbers/DragonEggs"
{
	Properties
	{
		[NoScaleOffset]_DiffuseMap("Diffuse Map", 2D) = "white" {}
		_ColorUP("Color UP", Color) = (1,0,0,0)
		_ColorBotton("Color Botton", Color) = (0,0.5862069,1,0)
		[NoScaleOffset][Normal]_NormalMap("Normal Map", 2D) = "bump" {}
		_Normal("Normal", Range( -3 , 3)) = 0
		[NoScaleOffset]_RoughnessMap("Roughness Map", 2D) = "white" {}
		_Rough("Rough", Range( 0 , 1)) = 1
		[Enum(UnityEngine.Rendering.BlendMode)]_Metallic("Metallic", Range( 0 , 1)) = 0
		[NoScaleOffset]_EmmisionMap("Emmision Map", 2D) = "white" {}
		[HDR]_Emission("Emission", Color) = (1,0.03676468,0.03676468,0)
		_EmmisionPower("Emmision Power", Range( 0 , 10)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _Normal;
		uniform sampler2D _NormalMap;
		uniform float4 _ColorUP;
		uniform float4 _ColorBotton;
		uniform sampler2D _DiffuseMap;
		uniform float4 _Emission;
		uniform sampler2D _EmmisionMap;
		uniform float _EmmisionPower;
		uniform float _Metallic;
		uniform sampler2D _RoughnessMap;
		uniform float _Rough;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_NormalMap3 = i.uv_texcoord;
			o.Normal = UnpackScaleNormal( tex2D( _NormalMap, uv_NormalMap3 ) ,_Normal );
			float4 lerpResult26 = lerp( _ColorUP , _ColorBotton , i.uv_texcoord.y);
			float2 uv_DiffuseMap1 = i.uv_texcoord;
			float4 blendOpSrc8 = lerpResult26;
			float4 blendOpDest8 = tex2D( _DiffuseMap, uv_DiffuseMap1 );
			o.Albedo = ( saturate( 2.0f*blendOpDest8*blendOpSrc8 + blendOpDest8*blendOpDest8*(1.0f - 2.0f*blendOpSrc8) )).rgb;
			float2 uv_EmmisionMap2 = i.uv_texcoord;
			float4 blendOpSrc10 = _Emission;
			float4 blendOpDest10 = tex2D( _EmmisionMap, uv_EmmisionMap2 );
			o.Emission = ( ( saturate( (( blendOpDest10 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpDest10 - 0.5 ) ) * ( 1.0 - blendOpSrc10 ) ) : ( 2.0 * blendOpDest10 * blendOpSrc10 ) ) )) * _EmmisionPower ).rgb;
			o.Metallic = _Metallic;
			float2 uv_RoughnessMap6 = i.uv_texcoord;
			o.Smoothness = ( ( 1.0 - tex2D( _RoughnessMap, uv_RoughnessMap6 ) ) * _Rough ).r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15301
157;141;1666;825;1404.832;895.5303;1;True;False
Node;AmplifyShaderEditor.ColorNode;24;-984.7195,-781.4845;Float;False;Property;_ColorBotton;Color Botton;3;0;Create;True;0;0;False;0;0,0.5862069,1,0;0.7379313,0,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;19;-992.496,-603.9189;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;6;-434.1115,72.71856;Float;True;Property;_RoughnessMap;Roughness Map;6;1;[NoScaleOffset];Create;True;0;0;False;0;4086b8dcbe0f5e047b4196c8af0ec18e;032041785d709854689604b2da519739;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;23;-994.1869,-964.9598;Float;False;Property;_ColorUP;Color UP;2;0;Create;True;0;0;False;0;1,0,0,0;1,0.3602941,0.3602941,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-401.722,448.6696;Float;True;Property;_EmmisionMap;Emmision Map;9;1;[NoScaleOffset];Create;True;0;0;False;0;fde12f945b57b1447a494da606565cb6;5ca746b4824c6ad42adbfbb2c73238e3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;9;-386.342,270.701;Float;False;Property;_Emission;Emission;10;1;[HDR];Create;True;0;0;False;0;1,0.03676468,0.03676468,0;1,0,0,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;16;-593.7974,-36.97682;Float;False;Property;_Rough;Rough;7;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;26;-587.3912,-804.9417;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;-575.342,-546.5634;Float;True;Property;_DiffuseMap;Diffuse Map;1;1;[NoScaleOffset];Create;True;0;0;False;0;48296887d9e698e4e8b34ae292b3d738;633ae744fc914834c900da9a9fb235d7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;14;-78.51289,96.83257;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;10;-66.86858,287.8523;Float;True;Overlay;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;46;172.1683,566.4698;Float;False;Property;_EmmisionPower;Emmision Power;11;0;Create;True;0;0;False;0;1;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-869.5459,-225.9018;Float;False;Property;_Normal;Normal;5;0;Create;True;0;0;False;0;0;0.6811765;-3;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;244.1683,339.4698;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;8;-140.8638,-635.801;Float;True;SoftLight;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;90.10147,-50.59646;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;3;-503.6438,-313.3155;Float;True;Property;_NormalMap;Normal Map;4;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;61a227fe1b2fc3444a5190b3f76343af;dbe6103765b532e4481c64b1c6db6ee9;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;11;-587.4472,-112.4286;Float;False;Property;_Metallic;Metallic;8;1;[Enum];Create;True;1;Option1;0;1;UnityEngine.Rendering.BlendMode;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;546.0457,-187.1837;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;Malbers/DragonEggs;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;0;False;0;Custom;0.5;True;True;0;True;Opaque;;AlphaTest;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;26;0;23;0
WireConnection;26;1;24;0
WireConnection;26;2;19;2
WireConnection;14;0;6;0
WireConnection;10;0;9;0
WireConnection;10;1;2;0
WireConnection;47;0;10;0
WireConnection;47;1;46;0
WireConnection;8;0;26;0
WireConnection;8;1;1;0
WireConnection;17;0;14;0
WireConnection;17;1;16;0
WireConnection;3;5;45;0
WireConnection;0;0;8;0
WireConnection;0;1;3;0
WireConnection;0;2;47;0
WireConnection;0;3;11;0
WireConnection;0;4;17;0
ASEEND*/
//CHKSM=F8546462BAF214F12052303BD64A49598E1772E5