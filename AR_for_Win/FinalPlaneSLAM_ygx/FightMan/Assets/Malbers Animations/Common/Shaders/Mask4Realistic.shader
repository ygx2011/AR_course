// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Malbers/Mask4Realistic"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		[NoScaleOffset]_Albedo("Albedo", 2D) = "white" {}
		[NoScaleOffset]_Metalic_Smoothness("Metalic_Smoothness", 2D) = "white" {}
		[NoScaleOffset]_Normal("Normal", 2D) = "bump" {}
		_NormalIntensity("Normal Intensity", Range( -3 , 3)) = 1
		[NoScaleOffset]_Emision("Emision", 2D) = "white" {}
		_EmissionPower("Emission Power", Range( 0 , 3)) = 1
		[NoScaleOffset]_Mask1("Mask 1", 2D) = "white" {}
		[Header(Mask Red)]_M1RHue("M1 R Hue", Range( 0 , 2)) = 1
		_M1RSaturation("M1 R Saturation", Range( 0 , 2)) = 1
		_M1RValue("M1 R Value", Range( 0 , 2)) = 1
		[Header(Mask Green)]_M1GHue("M1 G Hue", Range( 0 , 2)) = 1
		_M1GSaturation("M1 G Saturation", Range( 0 , 2)) = 1
		_M1GValue("M1 G Value", Range( 0 , 2)) = 1
		[Header(Mask Blue)]_M1BHue("M1 B Hue", Range( 0 , 2)) = 1
		_M1BSaturation("M1 B Saturation", Range( 0 , 2)) = 1
		_M1BValue("M1 B Value", Range( 0 , 2)) = 1
		[Header(Mask Alpha)]_M1AHue("M1 A Hue", Range( 0 , 2)) = 1
		_M1ASaturation("M1 A Saturation", Range( 0 , 2)) = 0.8705882
		_M1AValue("M1 A Value", Range( 0 , 2)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Overlay+0" "IsEmissive" = "true"  }
		Cull Back
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
		uniform sampler2D _Emision;
		uniform float _EmissionPower;
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
			float3 temp_output_167_0 = ( M1R151 + M1G152 + M1B153 + M1A154 );
			o.Albedo = temp_output_167_0;
			float2 uv_Emision237 = i.uv_texcoord;
			float3 desaturateInitialColor263 = tex2D( _Emision, uv_Emision237 ).rgb;
			float desaturateDot263 = dot( desaturateInitialColor263, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar263 = lerp( desaturateInitialColor263, desaturateDot263.xxx, 1.0 );
			float3 Emission266 = ( ( desaturateVar263 * temp_output_167_0 ) * _EmissionPower );
			o.Emission = Emission266;
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
-89;173;1119;776;3008.835;-1431.285;1;True;True
Node;AmplifyShaderEditor.SamplerNode;2;753.6071,1857.412;Float;True;Property;_Albedo;Albedo;1;1;[NoScaleOffset];Create;True;0;0;False;0;None;ccf9a38af1dd0cd43a8b70254914480c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;260;1102.11,1575.108;Float;True;TextureBase;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;176;-278.2505,1255.946;Float;False;260;TextureBase;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;59;-2744.899,-223.7064;Float;False;2096.119;2628.729;Mask 1;13;58;6;48;37;7;28;49;23;39;151;152;153;154;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RGBToHSVNode;119;-12.57645,1268.732;Float;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;39;-2668.544,496.7281;Float;False;890.8004;612.024;Mask 1 Green Channel;10;47;45;43;42;41;44;46;126;127;128;;0,1,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;23;-2669.603,1763.231;Float;False;890.8004;612.024;Mask 1 Alpha Channel;10;17;22;18;19;21;132;20;133;15;134;;1,1,1,0.534;0;0
Node;AmplifyShaderEditor.CommentaryNode;49;-2676.232,-130.2395;Float;False;890.8004;612.024;Mask 1 Red Channel;10;57;56;54;52;51;50;55;120;124;125;;1,0,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;28;-2675.228,1126.771;Float;False;890.8004;612.024;Mask 1 Blue Channel;10;32;36;34;33;35;129;131;30;130;31;;0,0,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;122;287.7054,1348.824;Float;False;Saturation;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;121;285.0123,1228.614;Float;False;Hue;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;123;283.8024,1428.544;Float;False;Value;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;132;-2567.581,2129.348;Float;False;123;Value;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-2586.596,147.5818;Float;False;Property;_M1RHue;M1 R Hue;8;0;Create;True;0;0;False;1;Header(Mask Red);1;0.21;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-2635.182,668.7261;Float;False;Property;_M1GHue;M1 G Hue;11;0;Create;True;0;0;False;1;Header(Mask Green);1;1.77;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-2598.042,1252.826;Float;False;Property;_M1BHue;M1 B Hue;14;0;Create;True;0;0;False;1;Header(Mask Blue);1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-2621.878,1805.306;Float;False;Property;_M1AHue;M1 A Hue;17;0;Create;True;0;0;False;1;Header(Mask Alpha);1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;124;-2535.76,-5.495001;Float;False;122;Saturation;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;133;-2591.483,1890.522;Float;False;122;Saturation;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;130;-2551.142,1164.58;Float;False;121;Hue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-2629.982,2262.474;Float;False;Property;_M1AValue;M1 A Value;19;0;Create;True;0;0;False;0;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-2579.615,329.7841;Float;False;Property;_M1RValue;M1 R Value;10;0;Create;True;0;0;False;0;1;0.53;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-2623.943,978.7378;Float;False;Property;_M1GValue;M1 G Value;13;0;Create;True;0;0;False;0;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-2630.372,827.941;Float;False;Property;_M1GSaturation;M1 G Saturation;12;0;Create;True;0;0;False;0;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-2633.119,2003.755;Float;False;Property;_M1ASaturation;M1 A Saturation;18;0;Create;True;0;0;False;0;0.8705882;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;126;-2531.57,905.225;Float;False;123;Value;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;129;-2545.142,1499.843;Float;False;123;Value;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;127;-2543.777,590.3352;Float;False;121;Hue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-2626.122,1590.761;Float;False;Property;_M1BValue;M1 B Value;16;0;Create;True;0;0;False;0;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;128;-2556.689,754.7337;Float;False;122;Saturation;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;120;-2522.85,-79.79761;Float;False;121;Hue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-2608.09,1428.587;Float;False;Property;_M1BSaturation;M1 B Saturation;15;0;Create;True;0;0;False;0;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;131;-2562.021,1345.931;Float;False;122;Saturation;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;134;-2582.704,1944.515;Float;False;121;Hue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-2581.231,225.0372;Float;False;Property;_M1RSaturation;M1 R Saturation;9;0;Create;True;0;0;False;0;1;1.15;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;125;-2516.648,65.41222;Float;False;123;Value;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-2191.868,1557.564;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-2253.012,847.9394;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-2247.658,-5.612677;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-2221.256,1243.886;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-2220.387,1398.731;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;-2240.525,303.3756;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-2253.987,741.6636;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-2255.394,1883.069;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-2246.305,131.7758;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-2251.563,640.3604;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-2211.842,2181.992;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-2215.738,2020.459;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.HSVToRGBNode;47;-2049.153,664.0313;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.HSVToRGBNode;57;-2056.84,37.06535;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;6;-1710.516,1000.104;Float;True;Property;_Mask1;Mask 1;7;1;[NoScaleOffset];Create;True;0;0;False;0;None;e492f88ea6daa6340978a6d0b79bed0e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.HSVToRGBNode;36;-2031.132,1243.291;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.HSVToRGBNode;17;-2050.212,1930.535;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-1230.712,1891.554;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-1249.841,1282.853;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-1246.602,633.2107;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-1273.15,2.308438;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;270;-80.67878,406.9695;Float;False;1588.313;587.9938;Emission;6;237;263;264;266;265;268;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;153;-911.6426,1294.156;Float;False;M1B;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;152;-958.8002,631.2532;Float;False;M1G;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;154;-898.1682,1891.038;Float;False;M1A;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;151;-948.0219,-4.70237;Float;False;M1R;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;161;2.903245,1848.056;Float;False;153;M1B;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;160;3.302354,1775.856;Float;False;152;M1G;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;237;-30.67885,456.9698;Float;True;Property;_Emision;Emision;5;1;[NoScaleOffset];Create;True;0;0;False;0;None;33351c5cb99a1cf45ae2e58fa7b9f0eb;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;162;6.801255,1926.056;Float;False;154;M1A;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;159;4.358154,1704.015;Float;False;151;M1R;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DesaturateOpNode;263;349.9471,463.4965;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;167;352.1048,1767.663;Float;True;4;4;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;268;692.4894,699.2685;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;265;658.269,565.9216;Float;False;Property;_EmissionPower;Emission Power;6;0;Create;True;0;0;False;0;1;1;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;264;1035.52,604.8804;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;266;1246.633,606.1754;Float;False;Emission;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;271;419.4945,2164.511;Float;False;Property;_NormalIntensity;Normal Intensity;4;0;Create;True;0;0;False;0;1;1;-3;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;259;859.7109,2265.407;Float;False;266;Emission;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;236;744.0275,2059.101;Float;True;Property;_Normal;Normal;3;1;[NoScaleOffset];Create;True;0;0;False;0;None;0ed95205e4ee71843b4e4240d6bfe413;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;235;775.0109,2376.892;Float;True;Property;_Metalic_Smoothness;Metalic_Smoothness;2;1;[NoScaleOffset];Create;True;0;0;False;0;None;5003bcba5772f3b4d9ef1df8672dc9c1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;117;1499.797,1761.758;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Malbers/Mask4Realistic;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Overlay;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;260;0;2;0
WireConnection;119;0;176;0
WireConnection;122;0;119;2
WireConnection;121;0;119;1
WireConnection;123;0;119;3
WireConnection;35;0;129;0
WireConnection;35;1;32;0
WireConnection;44;0;126;0
WireConnection;44;1;42;0
WireConnection;56;0;120;0
WireConnection;56;1;52;0
WireConnection;34;0;130;0
WireConnection;34;1;30;0
WireConnection;33;0;131;0
WireConnection;33;1;31;0
WireConnection;55;0;125;0
WireConnection;55;1;50;0
WireConnection;45;0;128;0
WireConnection;45;1;43;0
WireConnection;18;0;134;0
WireConnection;18;1;15;0
WireConnection;54;0;124;0
WireConnection;54;1;51;0
WireConnection;46;0;127;0
WireConnection;46;1;41;0
WireConnection;21;0;132;0
WireConnection;21;1;22;0
WireConnection;19;0;133;0
WireConnection;19;1;20;0
WireConnection;47;0;46;0
WireConnection;47;1;45;0
WireConnection;47;2;44;0
WireConnection;57;0;56;0
WireConnection;57;1;54;0
WireConnection;57;2;55;0
WireConnection;36;0;34;0
WireConnection;36;1;33;0
WireConnection;36;2;35;0
WireConnection;17;0;18;0
WireConnection;17;1;19;0
WireConnection;17;2;21;0
WireConnection;7;0;6;4
WireConnection;7;1;17;0
WireConnection;37;0;6;3
WireConnection;37;1;36;0
WireConnection;48;0;6;2
WireConnection;48;1;47;0
WireConnection;58;0;6;1
WireConnection;58;1;57;0
WireConnection;153;0;37;0
WireConnection;152;0;48;0
WireConnection;154;0;7;0
WireConnection;151;0;58;0
WireConnection;263;0;237;0
WireConnection;167;0;159;0
WireConnection;167;1;160;0
WireConnection;167;2;161;0
WireConnection;167;3;162;0
WireConnection;268;0;263;0
WireConnection;268;1;167;0
WireConnection;264;0;268;0
WireConnection;264;1;265;0
WireConnection;266;0;264;0
WireConnection;236;5;271;0
WireConnection;117;0;167;0
WireConnection;117;1;236;0
WireConnection;117;2;259;0
WireConnection;117;3;235;1
WireConnection;117;4;235;4
WireConnection;117;10;2;4
ASEEND*/
//CHKSM=6F340C824CE9B0318E89E4CD17445841E363A9F4