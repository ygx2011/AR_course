// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Malbers/MWater2"
{
	Properties
	{
		_WaterColor("WaterColor", Color) = (0.2074582,0.2643323,0.3962264,1)
		_EdgeColor("Edge Color", Color) = (0.7768779,0.7869238,0.8113208,1)
		[Normal]_WaterNormal("Water Normal", 2D) = "bump" {}
		_NormalScale("Normal Scale", Float) = 0
		_Specular("Specular", Float) = 0
		_Smoothness("Smoothness", Float) = 0
		_Distortion("Distortion", Float) = 0.5
		_EdgeDistance("Edge Distance", Float) = 0.24
		_EdgeStrength("Edge Strength", Float) = 0.24
		_Wave1Tile("Wave1 Tile", Float) = 1
		_Wave2Tile("Wave2 Tile", Float) = 1
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		#pragma surface surf StandardSpecular keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float3 worldPos;
			float4 screenPos;
		};

		uniform sampler2D _WaterNormal;
		uniform float _Wave1Tile;
		uniform float _NormalScale;
		uniform float _Wave2Tile;
		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float _Distortion;
		uniform float4 _WaterColor;
		uniform float4 _EdgeColor;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _EdgeDistance;
		uniform float _EdgeStrength;
		uniform float _Specular;
		uniform float _Smoothness;


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float3 ase_worldPos = i.worldPos;
			float4 appendResult34 = (float4(ase_worldPos.x , ase_worldPos.z , 0.0 , 0.0));
			float2 panner4 = ( 1.0 * _Time.y * float2( -0.05,0 ) + ( _Wave1Tile * appendResult34 ).xy);
			float2 panner6 = ( 1.0 * _Time.y * float2( 0.04,0.04 ) + ( _Wave2Tile * appendResult34 ).xy);
			float3 temp_output_13_0 = BlendNormals( UnpackScaleNormal( tex2D( _WaterNormal, panner4 ), _NormalScale ) , UnpackScaleNormal( tex2D( _WaterNormal, panner6 ), _NormalScale ) );
			o.Normal = temp_output_13_0;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float4 screenColor27 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( float3( (ase_grabScreenPosNorm).xy ,  0.0 ) + ( temp_output_13_0 * _Distortion ) ).xy);
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth7 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth7 = abs( ( screenDepth7 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _EdgeDistance ) );
			float clampResult17 = clamp( ( ( 1.0 - distanceDepth7 ) * _EdgeStrength ) , 0.0 , 1.0 );
			float Edge21 = clampResult17;
			float4 lerpResult26 = lerp( _WaterColor , _EdgeColor , Edge21);
			o.Albedo = ( screenColor27 * lerpResult26 ).rgb;
			o.Emission = ( _EdgeColor * Edge21 ).rgb;
			float3 temp_cast_6 = (_Specular).xxx;
			o.Specular = temp_cast_6;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
0;18;1906;1011;3392.329;897.2728;1.897045;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;33;-2752.615,-113.9628;Float;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;36;-2354.199,-312.817;Float;False;Property;_Wave2Tile;Wave2 Tile;10;0;Create;True;0;0;0;False;0;False;1;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;34;-2463.205,-119.1641;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-2359.837,-406.33;Float;False;Property;_Wave1Tile;Wave1 Tile;9;0;Create;True;0;0;0;False;0;False;1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-2074.021,-211.0965;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;1;-2102.096,-131.6132;Inherit;False;1281.603;457.1994;Normals;6;13;9;8;6;5;4;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-2142.254,-391.7381;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-83.84126,694.1061;Float;False;Property;_EdgeDistance;Edge Distance;7;0;Create;True;0;0;0;False;0;False;0.24;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;7;164.2153,617.1523;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;6;-1777.097,30.88586;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.04,0.04;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;4;-1779.397,-81.61322;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.05,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1753.135,158.0623;Float;False;Property;_NormalScale;Normal Scale;3;0;Create;True;0;0;0;False;0;False;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;8;-1436.089,132.5861;Inherit;True;Property;_WaterNormal;Water Normal;2;1;[Normal];Create;True;0;0;0;False;0;False;-1;ac4ce49f59542d342813ea776cbce7c3;9208831ffb1fd9340ab25826a5f30e66;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;9;-1435.397,-73.31427;Inherit;True;Property;_Normal2;Normal2;2;0;Create;True;0;0;0;False;0;False;-1;9208831ffb1fd9340ab25826a5f30e66;ac4ce49f59542d342813ea776cbce7c3;True;0;True;bump;Auto;True;Instance;8;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;11;413.5806,697.3409;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-73.22799,790.1958;Float;False;Property;_EdgeStrength;Edge Strength;8;0;Create;True;0;0;0;False;0;False;0.24;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;13;-1082.052,0.3420358;Inherit;True;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;615.6423,746.6452;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;14;-698.995,-550.9122;Inherit;False;985.6011;418.6005;Distorsion;7;27;22;20;19;18;16;15;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WireNode;16;-678.6964,-238.0112;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-456.0947,-252.3122;Float;False;Property;_Distortion;Distortion;6;0;Create;True;0;0;0;False;0;False;0.5;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;17;928.3574,778.2992;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;18;-654.8881,-492.5992;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-277.9933,-328.9122;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;20;-351.5403,-434.4202;Inherit;False;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;21;1153.751,707.7537;Float;False;Edge;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;24;66.84033,253.8422;Inherit;False;21;Edge;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;25;-32.01868,50.61902;Float;False;Property;_EdgeColor;Edge Color;1;0;Create;True;0;0;0;False;0;False;0.7768779,0.7869238,0.8113208,1;0.5410733,0.5857037,0.5943396,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-124.8947,-395.8122;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;23;-37.30762,-125.5352;Float;False;Property;_WaterColor;WaterColor;0;0;Create;True;0;0;0;False;0;False;0.2074582,0.2643323,0.3962264,1;0.4773941,0.5362675,0.6792453,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;27;66.60632,-399.6122;Float;False;Global;_WaterGrab;WaterGrab;-1;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;26;302.9023,-80.67218;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;28;356.9183,251.926;Float;False;Property;_Smoothness;Smoothness;5;0;Create;True;0;0;0;False;0;False;0;0.85;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;372.0723,174.0415;Float;False;Property;_Specular;Specular;4;0;Create;True;0;0;0;False;0;False;0;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;348.6603,387.2349;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;553.6364,-237.5832;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;816.344,-192.5896;Float;False;True;-1;2;ASEMaterialInspector;0;0;StandardSpecular;Malbers/MWater2;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Translucent;0.5;True;True;0;False;Opaque;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;2;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;34;0;33;1
WireConnection;34;1;33;3
WireConnection;37;0;36;0
WireConnection;37;1;34;0
WireConnection;35;0;32;0
WireConnection;35;1;34;0
WireConnection;7;0;3;0
WireConnection;6;0;37;0
WireConnection;4;0;35;0
WireConnection;8;1;6;0
WireConnection;8;5;5;0
WireConnection;9;1;4;0
WireConnection;9;5;5;0
WireConnection;11;0;7;0
WireConnection;13;0;9;0
WireConnection;13;1;8;0
WireConnection;12;0;11;0
WireConnection;12;1;10;0
WireConnection;16;0;13;0
WireConnection;17;0;12;0
WireConnection;19;0;16;0
WireConnection;19;1;15;0
WireConnection;20;0;18;0
WireConnection;21;0;17;0
WireConnection;22;0;20;0
WireConnection;22;1;19;0
WireConnection;27;0;22;0
WireConnection;26;0;23;0
WireConnection;26;1;25;0
WireConnection;26;2;24;0
WireConnection;31;0;25;0
WireConnection;31;1;24;0
WireConnection;30;0;27;0
WireConnection;30;1;26;0
WireConnection;0;0;30;0
WireConnection;0;1;13;0
WireConnection;0;2;31;0
WireConnection;0;3;29;0
WireConnection;0;4;28;0
ASEEND*/
//CHKSM=C38966BBA81C9E8885EC6513D058CEB11D7B847F