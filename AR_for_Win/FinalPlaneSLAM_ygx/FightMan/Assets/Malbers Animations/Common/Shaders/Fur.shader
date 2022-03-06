// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Malbers/Fur"
{
	Properties
	{
		_AlphaClip("Alpha Clip", Range( 0 , 1)) = 0.5
		[NoScaleOffset][SingleLineTexture]_Albedo("Albedo", 2D) = "white" {}
		_Tint("Tint", Color) = (1,1,1,1)
		_AO("AO", Color) = (1,1,1,1)
		_AOPower("AO Power", Range( 0 , 1)) = 0
		_AOExp("AO Exp", Float) = 0
		[NoScaleOffset][SingleLineTexture]_Emission("Emission", 2D) = "white" {}
		[HDR]_EmissionColor("Emission Color", Color) = (1,1,1,1)
		[NoScaleOffset][Normal][SingleLineTexture]_BodyNormal("Body Normal", 2D) = "bump" {}
		[NoScaleOffset][Normal][SingleLineTexture]_FurNormal("Fur Normal", 2D) = "bump" {}
		_NormalPowerBody("Normal Power Body", Range( -3 , 3)) = 0
		_NormalPowerFur("Normal Power Fur", Range( -3 , 3)) = 0
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_SpecularPower("Specular Power", Range( 0 , 1)) = 0
		[NoScaleOffset][SingleLineTexture]_FurMap1("Fur Map1 (A Height ID Root)", 2D) = "white" {}
		[NoScaleOffset][SingleLineTexture]_FurMap2("Fur Map1 (Spec AO Trans)", 2D) = "white" {}
		[Header(Anisotropy)]_AnisotropyFalloff("Anisotropy Falloff", Range( 1 , 256)) = 64
		_AnisotropyOffset("Anisotropy Offset", Range( -1 , 1)) = -1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#if defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (defined(SHADER_TARGET_SURFACE_ANALYSIS) && !defined(SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))//ASE Sampler Macros
		#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex.Sample(samplerTex,coord)
		#else//ASE Sampling Macros
		#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex2D(tex,coord)
		#endif//ASE Sampling Macros

		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv2_texcoord2;
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			float3 worldNormal;
			INTERNAL_DATA
			float3 worldPos;
		};

		UNITY_DECLARE_TEX2D_NOSAMPLER(_FurNormal);
		SamplerState sampler_FurNormal;
		uniform float _NormalPowerFur;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_BodyNormal);
		SamplerState sampler_BodyNormal;
		uniform float _NormalPowerBody;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_Albedo);
		SamplerState sampler_Albedo;
		uniform float4 _Tint;
		uniform float4 _AO;
		uniform float _AOPower;
		uniform float _AOExp;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_Emission);
		SamplerState sampler_Emission;
		uniform float4 _EmissionColor;
		uniform float _Metallic;
		uniform float _SpecularPower;
		uniform float _AnisotropyOffset;
		uniform float _AnisotropyFalloff;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_FurMap2);
		SamplerState sampler_FurMap2;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_FurMap1);
		SamplerState sampler_FurMap1;
		uniform float _AlphaClip;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv2_FurNormal72 = i.uv2_texcoord2;
			float3 tex2DNode72 = UnpackScaleNormal( SAMPLE_TEXTURE2D( _FurNormal, sampler_FurNormal, uv2_FurNormal72 ), _NormalPowerFur );
			float2 uv_BodyNormal5 = i.uv_texcoord;
			float3 NormalMap63 = UnpackScaleNormal( SAMPLE_TEXTURE2D( _BodyNormal, sampler_BodyNormal, uv_BodyNormal5 ), _NormalPowerBody );
			float3 lerpResult74 = lerp( tex2DNode72 , NormalMap63 , float3( 0.5,0.5,0.5 ));
			o.Normal = lerpResult74;
			float2 uv_Albedo1 = i.uv_texcoord;
			float4 temp_output_3_0 = ( SAMPLE_TEXTURE2D( _Albedo, sampler_Albedo, uv_Albedo1 ) * _Tint );
			float4 color92 = IsGammaSpace() ? float4(1,1,1,1) : float4(1,1,1,1);
			float4 lerpResult91 = lerp( ( _AO * temp_output_3_0 ) , color92 , i.vertexColor.r);
			float4 temp_cast_0 = (_AOExp).xxxx;
			float4 clampResult96 = clamp( pow( ( lerpResult91 + ( 1.0 - _AOPower ) ) , temp_cast_0 ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			o.Albedo = ( temp_output_3_0 * clampResult96 ).rgb;
			float2 uv_Emission58 = i.uv_texcoord;
			o.Emission = ( SAMPLE_TEXTURE2D( _Emission, sampler_Emission, uv_Emission58 ) * _EmissionColor ).rgb;
			o.Metallic = _Metallic;
			float3 PixelNormalWorld24 = normalize( (WorldNormalVector( i , NormalMap63 )) );
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = Unity_SafeNormalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 LightDirection33 = ase_worldlightDir;
			float3 normalizeResult32 = normalize( ( _WorldSpaceCameraPos - ase_worldPos ) );
			float3 ViewDirection34 = normalizeResult32;
			float3 normalizeResult36 = normalize( ( LightDirection33 + ViewDirection34 ) );
			float3 HalfVector37 = normalizeResult36;
			float dotResult39 = dot( PixelNormalWorld24 , HalfVector37 );
			float nDotH40 = dotResult39;
			float2 uv2_FurMap212 = i.uv2_texcoord2;
			float4 tex2DNode12 = SAMPLE_TEXTURE2D( _FurMap2, sampler_FurMap2, uv2_FurMap212 );
			float dotResult25 = dot( PixelNormalWorld24 , LightDirection33 );
			float nDotL38 = dotResult25;
			float temp_output_50_0 = ( ( pow( max( sin( radians( ( ( _AnisotropyOffset + nDotH40 ) * 180.0 ) ) ) , 0.0 ) , _AnisotropyFalloff ) * tex2DNode12.r ) * nDotL38 );
			o.Smoothness = ( _SpecularPower + temp_output_50_0 );
			o.Occlusion = tex2DNode12.g;
			o.Alpha = 1;
			float2 uv2_FurMap14 = i.uv2_texcoord2;
			clip( SAMPLE_TEXTURE2D( _FurMap1, sampler_FurMap1, uv2_FurMap14 ).r - _AlphaClip );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows dithercrossfade 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float4 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				half4 color : COLOR0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv2_texcoord2;
				o.customPack1.xy = v.texcoord1;
				o.customPack1.zw = customInputData.uv_texcoord;
				o.customPack1.zw = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv2_texcoord2 = IN.customPack1.xy;
				surfIN.uv_texcoord = IN.customPack1.zw;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				surfIN.vertexColor = IN.color;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18702
0;18;1906;1011;797.0054;113.3923;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;19;-4176,1216;Inherit;False;911.5641;491.3683;View Direction Vector;5;34;32;30;29;28;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;29;-4160,1440;Float;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceCameraPos;28;-4160,1264;Inherit;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;20;-3792,896;Inherit;False;533.0206;260.4803;Light Direction Vector;2;33;31;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-1037.949,635.9708;Float;False;Property;_NormalPowerBody;Normal Power Body;10;0;Create;True;0;0;False;0;False;0;1.5;-3;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;30;-3888,1312;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;32;-3664,1408;Inherit;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;5;-723,577.5;Inherit;True;Property;_BodyNormal;Body Normal;8;3;[NoScaleOffset];[Normal];[SingleLineTexture];Create;True;0;0;False;0;False;-1;None;13579146ea933b144b166890fb81a011;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;31;-3776,928;Inherit;True;True;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;33;-3504,928;Float;True;LightDirection;3;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;21;-3136,1040;Inherit;False;661.2201;238.5203;Halfway Vector;3;37;36;35;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;63;-328.9478,592.8581;Inherit;True;NormalMap;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;34;-3488,1296;Float;True;ViewDirection;4;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;54;-4073.008,544.154;Inherit;False;63;NormalMap;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;35;-3088,1104;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;22;-3728,528;Inherit;False;537.9105;289.5802;Pixel Normal Vector;2;24;23;;1,1,1,1;0;0
Node;AmplifyShaderEditor.NormalizeNode;36;-2960,1072;Inherit;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldNormalVector;23;-3696,586.9948;Inherit;True;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;24;-3456,592;Float;True;PixelNormalWorld;2;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;37;-2720,1088;Float;False;HalfVector;6;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-1852.1,678.4998;Float;False;Property;_AnisotropyOffset;Anisotropy Offset;17;0;Create;True;0;0;False;0;False;-1;-0.3;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;39;-2464,848;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;40;-2193,850;Float;False;nDotH;7;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;68;-1861.545,858.1927;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;51;-1793.624,884.6771;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-1478.287,1045.965;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;180;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-342.2667,-663.3403;Inherit;True;Property;_Albedo;Albedo;1;2;[NoScaleOffset];[SingleLineTexture];Create;True;0;0;False;0;False;-1;None;2ea2ca207dd1b4c4ca116d22515f1849;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;2;-298.2416,-452.8141;Inherit;False;Property;_Tint;Tint;2;0;Create;True;0;0;False;0;False;1,1,1,1;0.7735849,0.7735849,0.7735849,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RadiansOpNode;43;-1295.286,1046.266;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-6.497739,-523.6666;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;88;-112.9675,-909.3112;Inherit;False;Property;_AO;AO;3;0;Create;True;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;95;600.2224,-526.7049;Inherit;False;Property;_AOPower;AO Power;4;0;Create;True;0;0;False;0;False;0;0.495;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;92;-109.4558,-1094.271;Inherit;False;Constant;_White;White;2;0;Create;True;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;77;438.5325,-701.5943;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinOpNode;45;-1121.484,1047.165;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;274.9686,-755.9631;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;46;-961.5074,1046.131;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-1851.465,758.1081;Float;False;Property;_AnisotropyFalloff;Anisotropy Falloff;16;0;Create;True;0;0;False;1;Header(Anisotropy);False;64;66;1;256;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;97;831.2349,-607.1219;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;91;637.8719,-865.1642;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DotProductOpNode;25;-3051.963,581.9556;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;99;1055.35,-560.1365;Inherit;False;Property;_AOExp;AO Exp;5;0;Create;True;0;0;False;0;False;0;1.47;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;94;997.5182,-733.1803;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;48;-669.5869,1029.066;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;38;-2771.045,578.3008;Float;False;nDotL;5;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;12;-767.949,776.9708;Inherit;True;Property;_FurMap2;Fur Map1 (Spec AO Trans);15;2;[NoScaleOffset];[SingleLineTexture];Create;False;0;0;False;0;False;-1;None;a955501da94190a43bfa2af7ebbf2bea;True;1;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;49;-325.1451,1187.753;Inherit;False;38;nDotL;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;98;1161.885,-718.534;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-352.6443,953.4191;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;76;-1055.746,166.3885;Float;False;Property;_NormalPowerFur;Normal Power Fur;11;0;Create;True;0;0;False;0;False;0;0.5;-3;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;58;-317.669,-65.25127;Inherit;True;Property;_Emission;Emission;6;2;[NoScaleOffset];[SingleLineTexture];Create;True;0;0;False;0;False;-1;None;654af7c78d16add428013c20bac17641;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;56;-270.6222,272.7609;Inherit;False;Property;_EmissionColor;Emission Color;7;1;[HDR];Create;True;0;0;False;0;False;1,1,1,1;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;96;1385.331,-693.4567;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-187.3074,826.2288;Float;False;Property;_SpecularPower;Specular Power;13;0;Create;True;0;0;False;0;False;0;0.1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-96.0748,966.0663;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;72;-771.0619,108.194;Inherit;True;Property;_FurNormal;Fur Normal;9;3;[NoScaleOffset];[Normal];[SingleLineTexture];Create;True;0;0;False;0;False;-1;None;b2b11ef38a0e7764a879b324c668a443;True;1;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendNormalsNode;101;239.2084,313.1922;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;52;170.4147,939.1705;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;74;296.8497,418.0943;Inherit;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0.5,0.5,0.5;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;89;1512.1,-361.2868;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;100;308.1244,186.9615;Inherit;False;Property;_AlphaClip;Alpha Clip;0;0;Create;True;0;0;False;0;False;0.5;0.557;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-723,368.5;Inherit;True;Property;_FurMap1;Fur Map1 (A Height ID Root);14;2;[NoScaleOffset];[SingleLineTexture];Create;False;0;0;False;0;False;-1;None;4465e4dc19128b841bf2a8338c3e6792;True;1;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;460.1523,887.5528;Inherit;True;2;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;55;235.8938,-52.96725;Inherit;False;Property;_Metallic;Metallic;12;0;Create;True;0;0;False;0;False;0;0.3;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;75;-79.85882,518.4955;Inherit;True;Overlay;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;71;409.3981,610.021;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;51.35571,71.275;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;725.5341,23.34881;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Malbers/Fur;False;False;False;False;False;False;False;False;False;False;False;False;True;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;2;5;False;-1;10;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;True;100;0;0;0;False;0.1;False;-1;0;False;-1;True;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;30;0;28;0
WireConnection;30;1;29;0
WireConnection;32;0;30;0
WireConnection;5;5;8;0
WireConnection;33;0;31;0
WireConnection;63;0;5;0
WireConnection;34;0;32;0
WireConnection;35;0;33;0
WireConnection;35;1;34;0
WireConnection;36;0;35;0
WireConnection;23;0;54;0
WireConnection;24;0;23;0
WireConnection;37;0;36;0
WireConnection;39;0;24;0
WireConnection;39;1;37;0
WireConnection;40;0;39;0
WireConnection;68;0;41;0
WireConnection;51;0;68;0
WireConnection;51;1;40;0
WireConnection;42;0;51;0
WireConnection;43;0;42;0
WireConnection;3;0;1;0
WireConnection;3;1;2;0
WireConnection;45;0;43;0
WireConnection;93;0;88;0
WireConnection;93;1;3;0
WireConnection;46;0;45;0
WireConnection;97;0;95;0
WireConnection;91;0;93;0
WireConnection;91;1;92;0
WireConnection;91;2;77;1
WireConnection;25;0;24;0
WireConnection;25;1;33;0
WireConnection;94;0;91;0
WireConnection;94;1;97;0
WireConnection;48;0;46;0
WireConnection;48;1;44;0
WireConnection;38;0;25;0
WireConnection;98;0;94;0
WireConnection;98;1;99;0
WireConnection;53;0;48;0
WireConnection;53;1;12;1
WireConnection;96;0;98;0
WireConnection;50;0;53;0
WireConnection;50;1;49;0
WireConnection;72;5;76;0
WireConnection;101;0;72;0
WireConnection;101;1;75;0
WireConnection;52;0;50;0
WireConnection;52;1;18;0
WireConnection;74;0;72;0
WireConnection;74;1;63;0
WireConnection;89;0;3;0
WireConnection;89;1;96;0
WireConnection;10;1;52;0
WireConnection;75;0;72;0
WireConnection;75;1;63;0
WireConnection;71;0;18;0
WireConnection;71;1;50;0
WireConnection;57;0;58;0
WireConnection;57;1;56;0
WireConnection;0;0;89;0
WireConnection;0;1;74;0
WireConnection;0;2;57;0
WireConnection;0;3;55;0
WireConnection;0;4;71;0
WireConnection;0;5;12;2
WireConnection;0;10;4;1
ASEEND*/
//CHKSM=614D513CCD22D4BFEEF211016C77CD14359713AC