// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Malbers/Mask8Realistic"
{
	Properties
	{
		[NoScaleOffset]_Albedo("Albedo", 2D) = "white" {}
		[NoScaleOffset]_Metalic_Smoothness("Metalic_Smoothness", 2D) = "white" {}
		[NoScaleOffset]_Normal("Normal", 2D) = "bump" {}
		_NormalIntensity("Normal Intensity", Range( -2 , 2)) = 3
		[NoScaleOffset][Header(Emission)]_Emission("Emission", 2D) = "white" {}
		_EmissionPower("Emission Power", Range( 0 , 5)) = 3
		[Toggle]_UseRawEmission("Use Raw Emission", Float) = 0
		_AddEmissiveColor("Add Emissive Color", Color) = (0,0,0,0)
		[NoScaleOffset][Header(Masks)]_Mask1("Mask 1", 2D) = "white" {}
		[NoScaleOffset]_Mask2("Mask 2", 2D) = "white" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		[Header(Mask 1 Red)]_M1RHue("M1 R Hue", Range( 0 , 2)) = 1
		_M1RSaturation("M1 R Saturation", Range( 0 , 2)) = 1
		_M1RValue("M1 R Value", Range( 0 , 2)) = 1
		[Header(Mask 1 Green)]_M1GHue("M1 G Hue", Range( 0 , 2)) = 1
		_M1GSaturation("M1 G Saturation", Range( 0 , 2)) = 1
		_M1GValue("M1 G Value", Range( 0 , 2)) = 1
		[Header(Mask 1 Blue)]_M1BHue("M1 B Hue", Range( 0 , 2)) = 1
		_M1BSaturation("M1 B Saturation", Range( 0 , 2)) = 1
		_M1BValue("M1 B Value", Range( 0 , 2)) = 1
		[Header(Mask 1 Alpha)]_M1AHue("M1 A Hue", Range( 0 , 2)) = 1
		_M1ASaturation("M1 A Saturation", Range( 0 , 2)) = 0.8705882
		_M1AValue("M1 A Value", Range( 0 , 2)) = 1
		[Header(Mask 2 Red)]_M2RHue("M2 R Hue", Range( 0 , 2)) = 1
		_M2RSaturation("M2 R Saturation", Range( 0 , 2)) = 1
		_M2RValue("M2 R Value", Range( 0 , 2)) = 1
		[Header(Mask 2 Green)]_M2GHue("M2 G Hue", Range( 0 , 2)) = 1
		_M2GSaturation("M2 G Saturation", Range( 0 , 2)) = 1
		_M2GValue("M2 G Value", Range( 0 , 2)) = 1
		[Header(Mask 2 Blue)]_M2BHue("M2 B Hue", Range( 0 , 2)) = 1
		_M2BSaturation("M2 B Saturation", Range( 0 , 2)) = 1
		_M2BValue("M2 B Value", Range( 0 , 2)) = 1
		[Header(Mask 2 Alpha)]_M2AHue("M2 A Hue", Range( 0 , 2)) = 1
		_M2ASaturation("M2 A Saturation", Range( 0 , 2)) = 0.8705882
		_M2AValue("M2 A Value", Range( 0 , 2)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Overlay+0" "IsEmissive" = "true"  }
		Cull Back
		Blend One Zero , Zero Zero
		BlendOp Add , Add
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _NormalIntensity;
		uniform sampler2D _Normal;
		uniform sampler2D _Mask1;
		uniform sampler2D _Albedo;
		uniform float _M1RHue;
		uniform float _M1RSaturation;
		uniform float _M1RValue;
		uniform float _M1GHue;
		uniform float _M1GSaturation;
		uniform float _M1GValue;
		uniform float _M1BHue;
		uniform float _M1BSaturation;
		uniform float _M1BValue;
		uniform float _M1AHue;
		uniform float _M1ASaturation;
		uniform float _M1AValue;
		uniform sampler2D _Mask2;
		uniform float _M2RHue;
		uniform float _M2RSaturation;
		uniform float _M2RValue;
		uniform float _M2GHue;
		uniform float _M2GSaturation;
		uniform float _M2GValue;
		uniform float _M2BHue;
		uniform float _M2BSaturation;
		uniform float _M2BValue;
		uniform float _M2AHue;
		uniform float _M2ASaturation;
		uniform float _M2AValue;
		uniform float _UseRawEmission;
		uniform sampler2D _Emission;
		uniform float _EmissionPower;
		uniform float4 _AddEmissiveColor;
		uniform sampler2D _Metalic_Smoothness;
		uniform float _Cutoff = 0.5;


		float3 HSVToRGB( float3 c )
		{
			float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
			float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
			return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
		}


		float3 RGBToHSV(float3 c)
		{
			float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
			float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
			float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
			float d = q.x - min( q.w, q.y );
			float e = 1.0e-10;
			return float3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal236 = i.uv_texcoord;
			o.Normal = UnpackScaleNormal( tex2D( _Normal, uv_Normal236 ), _NormalIntensity );
			float2 uv_Mask16 = i.uv_texcoord;
			float4 tex2DNode6 = tex2D( _Mask1, uv_Mask16 );
			float2 uv_Albedo2 = i.uv_texcoord;
			float4 tex2DNode2 = tex2D( _Albedo, uv_Albedo2 );
			float4 TextureBase260 = tex2DNode2;
			float3 hsvTorgb119 = RGBToHSV( TextureBase260.rgb );
			float Hue121 = hsvTorgb119.x;
			float Saturation122 = hsvTorgb119.y;
			float Value123 = hsvTorgb119.z;
			float3 hsvTorgb57 = HSVToRGB( float3(( Hue121 * _M1RHue ),( Saturation122 * _M1RSaturation ),( Value123 * _M1RValue )) );
			float3 M1R151 = ( tex2DNode6.r * hsvTorgb57 );
			float3 hsvTorgb47 = HSVToRGB( float3(( Hue121 * _M1GHue ),( Saturation122 * _M1GSaturation ),( Value123 * _M1GValue )) );
			float3 M1G152 = ( tex2DNode6.g * hsvTorgb47 );
			float3 hsvTorgb36 = HSVToRGB( float3(( Hue121 * _M1BHue ),( Saturation122 * _M1BSaturation ),( Value123 * _M1BValue )) );
			float3 M1B153 = ( tex2DNode6.b * hsvTorgb36 );
			float3 hsvTorgb17 = HSVToRGB( float3(( Hue121 * _M1AHue ),( Saturation122 * _M1ASaturation ),( Value123 * _M1AValue )) );
			float3 M1A154 = ( tex2DNode6.a * hsvTorgb17 );
			float2 uv_Mask2222 = i.uv_texcoord;
			float4 tex2DNode222 = tex2D( _Mask2, uv_Mask2222 );
			float3 hsvTorgb220 = HSVToRGB( float3(( Hue121 * _M2RHue ),( Saturation122 * _M2RSaturation ),( Value123 * _M2RValue )) );
			float3 M2R230 = ( tex2DNode222.r * hsvTorgb220 );
			float3 hsvTorgb221 = HSVToRGB( float3(( Hue121 * _M2GHue ),( Saturation122 * _M2GSaturation ),( Value123 * _M2GValue )) );
			float3 M2G229 = ( tex2DNode222.g * hsvTorgb221 );
			float3 hsvTorgb218 = HSVToRGB( float3(( Hue121 * _M2BHue ),( Saturation122 * _M2BSaturation ),( Value123 * _M2BValue )) );
			float3 M2B228 = ( tex2DNode222.b * hsvTorgb218 );
			float3 hsvTorgb219 = HSVToRGB( float3(( Hue121 * _M2AHue ),( Saturation122 * _M2ASaturation ),( Value123 * _M2AValue )) );
			float3 M2A227 = ( tex2DNode222.a * hsvTorgb219 );
			float3 temp_output_167_0 = ( M1R151 + M1G152 + M1B153 + M1A154 + M2R230 + M2G229 + M2B228 + M2A227 );
			o.Albedo = temp_output_167_0;
			float2 uv_Emission237 = i.uv_texcoord;
			float4 tex2DNode237 = tex2D( _Emission, uv_Emission237 );
			float3 desaturateInitialColor263 = tex2DNode237.rgb;
			float desaturateDot263 = dot( desaturateInitialColor263, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar263 = lerp( desaturateInitialColor263, desaturateDot263.xxx, 1.0 );
			float4 blendOpSrc272 = _AddEmissiveColor;
			float4 blendOpDest272 = float4( temp_output_167_0 , 0.0 );
			float4 Emission266 = ( float4( desaturateVar263 , 0.0 ) * _EmissionPower * ( saturate( (( blendOpDest272 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpDest272 - 0.5 ) ) * ( 1.0 - blendOpSrc272 ) ) : ( 2.0 * blendOpDest272 * blendOpSrc272 ) ) )) );
			o.Emission = lerp(Emission266,( _EmissionPower * tex2DNode237 ),_UseRawEmission).rgb;
			float2 uv_Metalic_Smoothness235 = i.uv_texcoord;
			float4 tex2DNode235 = tex2D( _Metalic_Smoothness, uv_Metalic_Smoothness235 );
			o.Metallic = tex2DNode235.r;
			o.Smoothness = tex2DNode235.a;
			o.Alpha = 1;
			clip( tex2DNode2.a - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16200
1927;29;1352;732;3860.981;193.4327;3.602147;True;True
Node;AmplifyShaderEditor.SamplerNode;2;1359.875,2525.387;Float;True;Property;_Albedo;Albedo;0;1;[NoScaleOffset];Create;True;0;0;False;0;None;867a4e4681a3c6a40b9179f669431ce8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;260;1737.283,2517.482;Float;False;TextureBase;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;176;-99.59785,1822.35;Float;False;260;TextureBase;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;269;-2751.428,2469.185;Float;False;2071.064;2557.713;Mask 2;13;178;180;179;181;222;225;226;223;224;229;227;228;230;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RGBToHSVNode;119;174.0762,1813.136;Float;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;59;-2744.899,-223.7064;Float;False;2096.119;2628.729;Mask 1;13;58;6;48;37;7;28;49;23;39;151;152;153;154;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;181;-2700.424,3776.196;Float;False;890.8004;612.024;Mask 2 Blue Channel;10;218;215;209;208;205;201;197;193;192;187;;0,0,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;122;474.358,1892.228;Float;False;Saturation;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;39;-2668.544,496.7281;Float;False;890.8004;612.024;Mask 1 Green Channel;10;47;45;43;42;41;44;46;126;127;128;;0,1,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;179;-2694.799,4412.655;Float;False;890.8004;612.024;Mask 2 Alpha Channel;10;219;217;213;207;202;195;194;188;183;182;;1,1,1,0.534;0;0
Node;AmplifyShaderEditor.CommentaryNode;23;-2669.603,1763.231;Float;False;890.8004;612.024;Mask 1 Alpha Channel;10;17;22;18;19;21;132;20;133;15;134;;1,1,1,0.534;0;0
Node;AmplifyShaderEditor.CommentaryNode;180;-2693.74,3146.153;Float;False;890.8004;612.024;Mask 2 Green Channel;10;221;216;212;206;204;203;190;189;185;184;;0,1,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;49;-2676.232,-130.2395;Float;False;890.8004;612.024;Mask 1 Red Channel;10;57;56;54;52;51;50;55;120;124;125;;1,0,0,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;123;470.455,1972.948;Float;False;Value;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;28;-2675.228,1126.771;Float;False;890.8004;612.024;Mask 1 Blue Channel;10;32;36;34;33;35;129;131;30;130;31;;0,0,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;178;-2701.428,2519.185;Float;False;890.8004;612.024;Mask 2 Red Channel;10;220;214;211;210;200;199;198;196;191;186;;1,0,0,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;121;473.6649,1812.018;Float;False;Hue;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;183;-2655.178,4911.898;Float;False;Property;_M2AValue;M2 A Value;34;0;Create;True;0;0;False;0;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;128;-2556.689,754.7337;Float;False;122;Saturation;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;131;-2562.021,1345.931;Float;False;122;Saturation;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;133;-2591.483,1890.522;Float;False;122;Saturation;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-2579.615,329.7841;Float;False;Property;_M1RValue;M1 R Value;13;0;Create;True;0;0;False;0;1;0.64;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;134;-2582.704,1944.515;Float;False;121;Hue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-2633.119,2003.755;Float;False;Property;_M1ASaturation;M1 A Saturation;21;0;Create;True;0;0;False;0;0.8705882;2;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;132;-2567.581,2129.348;Float;False;123;Value;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-2623.943,978.7378;Float;False;Property;_M1GValue;M1 G Value;16;0;Create;True;0;0;False;0;1;0.32;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;126;-2531.57,905.225;Float;False;123;Value;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;130;-2551.142,1164.58;Float;False;121;Hue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-2581.231,225.0372;Float;False;Property;_M1RSaturation;M1 R Saturation;12;0;Create;True;0;0;False;0;1;1.34;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;182;-2660.437,4569.637;Float;False;Property;_M2AHue;M2 A Hue;32;0;Create;True;0;0;False;1;Header(Mask 2 Alpha);1;0.94;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-2630.372,827.941;Float;False;Property;_M1GSaturation;M1 G Saturation;15;0;Create;True;0;0;False;0;1;0.7;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-2608.09,1428.587;Float;False;Property;_M1BSaturation;M1 B Saturation;18;0;Create;True;0;0;False;0;1;0.7;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;199;-2611.792,2797.006;Float;False;Property;_M2RHue;M2 R Hue;23;0;Create;True;0;0;False;1;Header(Mask 2 Red);1;0.6;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;184;-2660.378,3318.151;Float;False;Property;_M2GHue;M2 G Hue;26;0;Create;True;0;0;False;1;Header(Mask 2 Green);1;1.41;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;205;-2646.041,3916.661;Float;False;Property;_M2BHue;M2 B Hue;29;0;Create;True;0;0;False;1;Header(Mask 2 Blue);1;1.09;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-2629.982,2262.474;Float;False;Property;_M1AValue;M1 A Value;22;0;Create;True;0;0;False;0;1;0.65;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;198;-2560.956,2643.93;Float;False;122;Saturation;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;127;-2543.777,590.3352;Float;False;121;Hue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;129;-2545.142,1499.843;Float;False;123;Value;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;120;-2522.85,-79.79761;Float;False;121;Hue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;203;-2581.885,3404.158;Float;False;122;Saturation;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-2626.122,1590.761;Float;False;Property;_M1BValue;M1 B Value;19;0;Create;True;0;0;False;0;1;0.41;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;125;-2516.648,65.41222;Float;False;123;Value;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;192;-2555.728,4166.18;Float;False;123;Value;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-2621.878,1805.306;Float;False;Property;_M1AHue;M1 A Hue;20;0;Create;True;0;0;False;1;Header(Mask 1 Alpha);1;0.09;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;201;-2648.922,4071.872;Float;False;Property;_M2BSaturation;M2 B Saturation;30;0;Create;True;0;0;False;0;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;193;-2651.318,4240.185;Float;False;Property;_M2BValue;M2 B Value;31;0;Create;True;0;0;False;0;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;189;-2556.766,3554.649;Float;False;123;Value;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;187;-2570.336,3999.171;Float;False;122;Saturation;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;185;-2649.14,3628.163;Float;False;Property;_M2GValue;M2 G Value;28;0;Create;True;0;0;False;0;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;194;-2559.262,4831.438;Float;False;123;Value;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;197;-2551.418,3839.279;Float;False;121;Hue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;186;-2541.844,2714.837;Float;False;123;Value;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;191;-2548.047,2569.627;Float;False;121;Hue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;188;-2578.376,4664.429;Float;False;122;Saturation;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;190;-2568.974,3239.76;Float;False;121;Hue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;124;-2535.76,-5.495001;Float;False;122;Saturation;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;200;-2604.811,2979.208;Float;False;Property;_M2RValue;M2 R Value;25;0;Create;True;0;0;False;0;1;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;202;-2568.467,4491.021;Float;False;121;Hue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-2586.596,147.5818;Float;False;Property;_M1RHue;M1 R Hue;11;0;Create;True;0;0;False;1;Header(Mask 1 Red);1;1.25;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;204;-2655.568,3477.366;Float;False;Property;_M2GSaturation;M2 G Saturation;27;0;Create;True;0;0;False;0;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;196;-2606.427,2874.462;Float;False;Property;_M2RSaturation;M2 R Saturation;24;0;Create;True;0;0;False;0;1;2;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;195;-2658.315,4739.358;Float;False;Property;_M2ASaturation;M2 A Saturation;33;0;Create;True;0;0;False;0;0.8705882;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-2598.042,1252.826;Float;False;Property;_M1BHue;M1 B Hue;17;0;Create;True;0;0;False;1;Header(Mask 1 Blue);1;1.08;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-2635.182,668.7261;Float;False;Property;_M1GHue;M1 G Hue;14;0;Create;True;0;0;False;1;Header(Mask 1 Green);1;1.01;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;214;-2265.721,2952.8;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-2191.868,1557.564;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;206;-2279.183,3391.088;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-2251.563,640.3604;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;-2240.525,303.3756;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;211;-2272.854,2643.812;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-2215.738,2020.459;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;212;-2276.759,3289.785;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;208;-2327.261,3878.618;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-2255.394,1883.069;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;215;-2304.558,4161.759;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-2221.256,1243.886;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;210;-2271.501,2781.201;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-2247.658,-5.612677;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-2246.305,131.7758;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-2211.842,2181.992;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-2220.387,1398.731;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;216;-2278.208,3497.364;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;213;-2270.553,4797.9;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-2253.987,741.6636;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;217;-2266.226,4537.281;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-2253.012,847.9394;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;209;-2312.392,4029.519;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;207;-2264.873,4674.669;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.HSVToRGBNode;219;-2075.408,4579.958;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.HSVToRGBNode;17;-2050.212,1930.535;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;222;-1735.712,3649.529;Float;True;Property;_Mask2;Mask 2;9;1;[NoScaleOffset];Create;True;0;0;False;0;None;3f5ad38c3a0b1df4a91f5d65b5dd75e0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.HSVToRGBNode;36;-2031.132,1243.291;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.HSVToRGBNode;221;-2074.35,3313.456;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.HSVToRGBNode;57;-2056.84,37.06535;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.HSVToRGBNode;47;-2049.153,664.0313;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.HSVToRGBNode;220;-2082.037,2686.49;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.HSVToRGBNode;218;-2094.631,3978.896;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;6;-1710.516,1000.104;Float;True;Property;_Mask1;Mask 1;8;1;[NoScaleOffset];Create;True;0;0;False;1;Header(Masks);None;c178487faf41b8444a01600b426dea53;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-1273.15,2.308438;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;226;-1275.037,3932.278;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-1230.712,1891.554;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;224;-1271.798,3282.635;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-1246.602,633.2107;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;223;-1298.346,2651.733;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-1249.841,1282.853;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;225;-1255.908,4540.978;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;228;-935.4912,3943.581;Float;False;M2B;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;154;-898.1682,1891.038;Float;False;M1A;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;227;-923.364,4540.461;Float;False;M2A;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;152;-958.8002,631.2532;Float;False;M1G;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;153;-911.6426,1294.156;Float;False;M1B;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;151;-948.0219,-4.70237;Float;False;M1R;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;230;-973.2177,2644.722;Float;False;M2R;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;229;-982.6489,3287.413;Float;False;M2G;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;234;191.1064,2732.044;Float;False;227;M2A;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;162;182.4539,2352.46;Float;False;154;M1A;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;233;189.676,2629.709;Float;False;228;M2B;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;159;180.0108,2130.419;Float;False;151;M1R;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;232;185.1897,2543.238;Float;False;229;M2G;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;161;178.5559,2274.46;Float;False;153;M1B;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;160;178.955,2202.26;Float;False;152;M1G;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;231;191.1062,2449.822;Float;False;230;M2R;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;270;-69.15874,730.6395;Float;False;2116.08;972.3696;Emission;8;266;268;272;263;265;271;237;275;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;237;-19.06968,1013.364;Float;True;Property;_Emission;Emission;4;1;[NoScaleOffset];Create;True;0;0;False;1;Header(Emission);None;dc1ffb9a16fc7e24d9d98527cc45c292;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;271;599.5974,815.7463;Float;False;Property;_AddEmissiveColor;Add Emissive Color;7;0;Create;True;0;0;False;0;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;167;553.1392,2258.137;Float;True;8;8;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BlendOpsNode;272;944.4301,797.4062;Float;True;Overlay;True;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;265;943.8673,1109.792;Float;False;Property;_EmissionPower;Emission Power;5;0;Create;True;0;0;False;0;3;1;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.DesaturateOpNode;263;404.1066,1013.858;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;268;1366.916,1030.306;Float;True;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;275;1272.07,1300.282;Float;False;628.3253;329.7061;Toggle Emission Option;3;273;259;274;;1,0.7453228,0.07075471,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;266;1663.14,1025.584;Float;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;259;1345.471,1372.381;Float;False;266;Emission;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;274;1353.13,1496.984;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;276;1031.266,2766.481;Float;False;Property;_NormalIntensity;Normal Intensity;3;0;Create;True;0;0;False;0;3;1;-2;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;236;1375.269,2723.411;Float;True;Property;_Normal;Normal;2;1;[NoScaleOffset];Create;True;0;0;False;0;None;183d5ae93f9529f41bad7d1f015b544f;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;235;1385.617,2937.997;Float;True;Property;_Metalic_Smoothness;Metalic_Smoothness;1;1;[NoScaleOffset];Create;True;0;0;False;0;None;dc1ffb9a16fc7e24d9d98527cc45c292;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;273;1630.396,1431.755;Float;False;Property;_UseRawEmission;Use Raw Emission;6;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;117;2330.836,2331.53;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Malbers/Mask8Realistic;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Overlay;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;1;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;10;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;260;0;2;0
WireConnection;119;0;176;0
WireConnection;122;0;119;2
WireConnection;123;0;119;3
WireConnection;121;0;119;1
WireConnection;214;0;186;0
WireConnection;214;1;200;0
WireConnection;35;0;129;0
WireConnection;35;1;32;0
WireConnection;206;0;203;0
WireConnection;206;1;204;0
WireConnection;46;0;127;0
WireConnection;46;1;41;0
WireConnection;55;0;125;0
WireConnection;55;1;50;0
WireConnection;211;0;191;0
WireConnection;211;1;199;0
WireConnection;19;0;133;0
WireConnection;19;1;20;0
WireConnection;212;0;190;0
WireConnection;212;1;184;0
WireConnection;208;0;197;0
WireConnection;208;1;205;0
WireConnection;18;0;134;0
WireConnection;18;1;15;0
WireConnection;215;0;192;0
WireConnection;215;1;193;0
WireConnection;34;0;130;0
WireConnection;34;1;30;0
WireConnection;210;0;198;0
WireConnection;210;1;196;0
WireConnection;56;0;120;0
WireConnection;56;1;52;0
WireConnection;54;0;124;0
WireConnection;54;1;51;0
WireConnection;21;0;132;0
WireConnection;21;1;22;0
WireConnection;33;0;131;0
WireConnection;33;1;31;0
WireConnection;216;0;189;0
WireConnection;216;1;185;0
WireConnection;213;0;194;0
WireConnection;213;1;183;0
WireConnection;45;0;128;0
WireConnection;45;1;43;0
WireConnection;217;0;202;0
WireConnection;217;1;182;0
WireConnection;44;0;126;0
WireConnection;44;1;42;0
WireConnection;209;0;187;0
WireConnection;209;1;201;0
WireConnection;207;0;188;0
WireConnection;207;1;195;0
WireConnection;219;0;217;0
WireConnection;219;1;207;0
WireConnection;219;2;213;0
WireConnection;17;0;18;0
WireConnection;17;1;19;0
WireConnection;17;2;21;0
WireConnection;36;0;34;0
WireConnection;36;1;33;0
WireConnection;36;2;35;0
WireConnection;221;0;212;0
WireConnection;221;1;206;0
WireConnection;221;2;216;0
WireConnection;57;0;56;0
WireConnection;57;1;54;0
WireConnection;57;2;55;0
WireConnection;47;0;46;0
WireConnection;47;1;45;0
WireConnection;47;2;44;0
WireConnection;220;0;211;0
WireConnection;220;1;210;0
WireConnection;220;2;214;0
WireConnection;218;0;208;0
WireConnection;218;1;209;0
WireConnection;218;2;215;0
WireConnection;58;0;6;1
WireConnection;58;1;57;0
WireConnection;226;0;222;3
WireConnection;226;1;218;0
WireConnection;7;0;6;4
WireConnection;7;1;17;0
WireConnection;224;0;222;2
WireConnection;224;1;221;0
WireConnection;48;0;6;2
WireConnection;48;1;47;0
WireConnection;223;0;222;1
WireConnection;223;1;220;0
WireConnection;37;0;6;3
WireConnection;37;1;36;0
WireConnection;225;0;222;4
WireConnection;225;1;219;0
WireConnection;228;0;226;0
WireConnection;154;0;7;0
WireConnection;227;0;225;0
WireConnection;152;0;48;0
WireConnection;153;0;37;0
WireConnection;151;0;58;0
WireConnection;230;0;223;0
WireConnection;229;0;224;0
WireConnection;167;0;159;0
WireConnection;167;1;160;0
WireConnection;167;2;161;0
WireConnection;167;3;162;0
WireConnection;167;4;231;0
WireConnection;167;5;232;0
WireConnection;167;6;233;0
WireConnection;167;7;234;0
WireConnection;272;0;271;0
WireConnection;272;1;167;0
WireConnection;263;0;237;0
WireConnection;268;0;263;0
WireConnection;268;1;265;0
WireConnection;268;2;272;0
WireConnection;266;0;268;0
WireConnection;274;0;265;0
WireConnection;274;1;237;0
WireConnection;236;5;276;0
WireConnection;273;0;259;0
WireConnection;273;1;274;0
WireConnection;117;0;167;0
WireConnection;117;1;236;0
WireConnection;117;2;273;0
WireConnection;117;3;235;1
WireConnection;117;4;235;4
WireConnection;117;10;2;4
ASEEND*/
//CHKSM=C57271D4C0BB7637EC5333076661F269B4597449