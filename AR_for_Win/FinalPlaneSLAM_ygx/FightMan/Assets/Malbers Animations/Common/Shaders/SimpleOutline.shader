// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Malbers/SimpleOutline"
{
	Properties
	{
		_ASEOutlineColor( "Outline Color", Color ) = (1,1,1,1)
		_ASEOutlineWidth( "Outline Width", Float ) = 0.025
		_Tint("Tint", Color) = (1,1,1,1)
		[NoScaleOffset][SingleLineTexture]_Albedo("Albedo", 2D) = "white" {}
		[NoScaleOffset][SingleLineTexture]_Normal("Normal", 2D) = "bump" {}
		_NormalPower("Normal Power", Float) = 1
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Smooth("Smooth", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		
		
		
		
		struct Input {
			half filler;
		};
		float4 _ASEOutlineColor;
		float _ASEOutlineWidth;
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += ( v.normal * _ASEOutlineWidth );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			o.Emission = _ASEOutlineColor.rgb;
			o.Alpha = 1;
		}
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal;
		uniform float _NormalPower;
		uniform sampler2D _Albedo;
		uniform float4 _Tint;
		uniform float _Metallic;
		uniform float _Smooth;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal2 = i.uv_texcoord;
			o.Normal = UnpackScaleNormal( tex2D( _Normal, uv_Normal2 ), _NormalPower );
			float2 uv_Albedo1 = i.uv_texcoord;
			o.Albedo = ( tex2D( _Albedo, uv_Albedo1 ) * _Tint ).rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smooth;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18702
1920;446;1920;822;1303;206;1;True;False
Node;AmplifyShaderEditor.ColorNode;4;-541.6617,8.790344;Inherit;False;Property;_Tint;Tint;0;0;Create;True;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-580,-187;Inherit;True;Property;_Albedo;Albedo;1;2;[NoScaleOffset];[SingleLineTexture];Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;7;-925,208;Inherit;False;Property;_NormalPower;Normal Power;3;0;Create;True;0;0;False;0;False;1;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-215.6617,-177.2097;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-332,405;Inherit;False;Property;_Smooth;Smooth;5;0;Create;True;0;0;False;0;False;0;0.3;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-661,194;Inherit;True;Property;_Normal;Normal;2;2;[NoScaleOffset];[SingleLineTexture];Create;True;0;0;False;0;False;-1;None;None;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;3;-338.6765,322.4194;Inherit;False;Property;_Metallic;Metallic;4;0;Create;True;0;0;False;0;False;0;0.3;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;210,-29;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Malbers/SimpleOutline;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;True;0.025;1,1,1,1;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;1;0
WireConnection;5;1;4;0
WireConnection;2;5;7;0
WireConnection;0;0;5;0
WireConnection;0;1;2;0
WireConnection;0;3;3;0
WireConnection;0;4;6;0
ASEEND*/
//CHKSM=5D334067F387C6CDB638F20FECF2CB4B1B14FBE6