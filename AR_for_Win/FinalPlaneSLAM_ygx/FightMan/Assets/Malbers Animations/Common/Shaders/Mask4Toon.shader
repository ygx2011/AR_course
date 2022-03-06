// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Malbers/Masks4Toon"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {}
		_M1RHue("M1 R Hue", Range( 0 , 2)) = 1
		_M1RSaturation("M1 R Saturation", Range( 0 , 2)) = 1
		_M1RValue("M1 R Value", Range( 0 , 2)) = 1
		_M1GHue("M1 G Hue", Range( 0 , 2)) = 1
		_M1GSaturation("M1 G Saturation", Range( 0 , 2)) = 1
		_M1GValue("M1 G Value", Range( 0 , 2)) = 1
		_M1BHue("M1 B Hue", Range( 0 , 2)) = 1
		_M1BSaturation("M1 B Saturation", Range( 0 , 2)) = 1
		_M1BValue("M1 B Value", Range( 0 , 2)) = 1
		_M1AHue("M1 A Hue", Range( 0 , 2)) = 1
		_M1ASaturation("M1 A Saturation", Range( 0 , 2)) = 0.8705882
		_M1AValue("M1 A Value", Range( 0 , 2)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Mask;
		uniform float4 _Mask_ST;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
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

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_Mask = i.uv_texcoord * _Mask_ST.xy + _Mask_ST.zw;
			float4 tex2DNode6 = tex2D( _Mask, uv_Mask );
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float3 hsvTorgb119 = RGBToHSV( tex2D( _Albedo, uv_Albedo ).rgb );
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
			o.Emission = ( M1R151 + M1G152 + M1B153 + M1A154 );
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16200
1927;29;1352;732;1040.151;416.7155;1.082212;True;True
Node;AmplifyShaderEditor.SamplerNode;2;-601.0118,-196.9873;Float;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;7acc7af06cc1fe34780bd5ec1cf21e30;7acc7af06cc1fe34780bd5ec1cf21e30;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RGBToHSVNode;119;-260.1839,-200.9314;Float;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;59;-2744.899,-223.7064;Float;False;2096.119;2628.729;Mask 1;13;58;6;48;37;7;28;49;23;39;151;152;153;154;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;49;-2676.232,-130.2395;Float;False;890.8004;612.024;Mask 1 Red Channel;10;57;56;54;52;51;50;55;120;124;125;;1,0,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;23;-2669.603,1763.231;Float;False;890.8004;612.024;Mask 1 Alpha Channel;10;18;19;21;20;15;17;22;132;133;134;;1,1,1,0.534;0;0
Node;AmplifyShaderEditor.CommentaryNode;39;-2668.544,496.7281;Float;False;890.8004;612.024;Mask 1 Green Channel;10;47;45;43;42;41;44;46;126;127;128;;0,1,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;28;-2675.228,1126.771;Float;False;890.8004;612.024;Mask 1 Blue Channel;10;36;32;31;30;35;33;34;129;130;131;;0,0,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;123;36.19495,-41.12027;Float;False;Value;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;121;41.40485,-196.0493;Float;False;Hue;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;122;40.09793,-121.8394;Float;False;Saturation;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;126;-2531.57,905.225;Float;False;123;Value;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;133;-2553.18,2015.005;Float;False;122;Saturation;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-2635.182,668.7261;Float;False;Property;_M1GHue;M1 G Hue;5;0;Create;True;0;0;False;0;1;1.07;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;120;-2522.85,-79.79761;Float;False;121;Hue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;125;-2516.648,65.41222;Float;False;123;Value;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;132;-2534.066,2182.014;Float;False;123;Value;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;127;-2543.777,590.3352;Float;False;121;Hue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;129;-2530.532,1516.756;Float;False;123;Value;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-2623.943,978.7378;Float;False;Property;_M1GValue;M1 G Value;7;0;Create;True;0;0;False;0;1;0.85;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-2581.231,225.0372;Float;False;Property;_M1RSaturation;M1 R Saturation;3;0;Create;True;0;0;False;0;1;2;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;130;-2526.222,1189.854;Float;False;121;Hue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;131;-2545.14,1349.747;Float;False;122;Saturation;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-2626.122,1590.761;Float;False;Property;_M1BValue;M1 B Value;10;0;Create;True;0;0;False;0;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;134;-2543.271,1841.598;Float;False;121;Hue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-2586.596,147.5818;Float;False;Property;_M1RHue;M1 R Hue;2;0;Create;True;0;0;False;0;1;0.26;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-2633.119,2089.934;Float;False;Property;_M1ASaturation;M1 A Saturation;12;0;Create;True;0;0;False;0;0.8705882;1.34;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;128;-2556.689,754.7337;Float;False;122;Saturation;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-2620.845,1267.236;Float;False;Property;_M1BHue;M1 B Hue;8;0;Create;True;0;0;False;0;1;0.67;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;124;-2535.76,-5.495001;Float;False;122;Saturation;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-2630.372,827.941;Float;False;Property;_M1GSaturation;M1 G Saturation;6;0;Create;True;0;0;False;0;1;1.21;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-2579.615,329.7841;Float;False;Property;_M1RValue;M1 R Value;4;0;Create;True;0;0;False;0;1;1.01;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-2623.725,1422.448;Float;False;Property;_M1BSaturation;M1 B Saturation;9;0;Create;True;0;0;False;0;1;1.76;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-2629.982,2262.474;Float;False;Property;_M1AValue;M1 A Value;13;0;Create;True;0;0;False;0;1;1.14;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-2636.241,1920.213;Float;False;Property;_M1AHue;M1 A Hue;11;0;Create;True;0;0;False;0;1;0.51;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-2239.677,2025.246;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-2247.658,-5.612677;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-2251.563,640.3604;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-2253.987,741.6636;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-2302.065,1229.193;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-2287.196,1380.095;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-2245.357,2148.476;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-2246.305,131.7758;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-2279.361,1512.335;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-2253.012,847.9394;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;-2240.525,303.3756;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-2241.03,1887.857;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.HSVToRGBNode;36;-2069.434,1329.471;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.HSVToRGBNode;17;-2050.212,1930.535;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.HSVToRGBNode;57;-2056.84,37.06535;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.HSVToRGBNode;47;-2049.153,664.0313;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;6;-1710.516,1000.104;Float;True;Property;_Mask;Mask;1;0;Create;True;0;0;False;0;697e853118ed8f44085069e39464b1d8;697e853118ed8f44085069e39464b1d8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-1249.841,1282.853;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-1273.15,2.308438;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-1230.712,1891.554;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-1246.602,633.2107;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;151;-948.0219,-4.70237;Float;False;M1R;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;152;-958.8002,631.2532;Float;False;M1G;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;154;-898.1682,1891.038;Float;False;M1A;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;153;-911.6426,1294.156;Float;False;M1B;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;162;-561.5598,457.3265;Float;False;154;M1A;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;161;-565.4579,379.3266;Float;False;153;M1B;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;159;-564.0029,235.2853;Float;False;151;M1R;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;160;-565.0588,307.1263;Float;False;152;M1G;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;167;-255.0448,290.0679;Float;True;4;4;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;117;-4.434741,239.1637;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Malbers/Masks4Toon;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;119;0;2;0
WireConnection;123;0;119;3
WireConnection;121;0;119;1
WireConnection;122;0;119;2
WireConnection;19;0;133;0
WireConnection;19;1;20;0
WireConnection;56;0;120;0
WireConnection;56;1;52;0
WireConnection;46;0;127;0
WireConnection;46;1;41;0
WireConnection;45;0;128;0
WireConnection;45;1;43;0
WireConnection;34;0;130;0
WireConnection;34;1;30;0
WireConnection;33;0;131;0
WireConnection;33;1;31;0
WireConnection;21;0;132;0
WireConnection;21;1;22;0
WireConnection;54;0;124;0
WireConnection;54;1;51;0
WireConnection;35;0;129;0
WireConnection;35;1;32;0
WireConnection;44;0;126;0
WireConnection;44;1;42;0
WireConnection;55;0;125;0
WireConnection;55;1;50;0
WireConnection;18;0;134;0
WireConnection;18;1;15;0
WireConnection;36;0;34;0
WireConnection;36;1;33;0
WireConnection;36;2;35;0
WireConnection;17;0;18;0
WireConnection;17;1;19;0
WireConnection;17;2;21;0
WireConnection;57;0;56;0
WireConnection;57;1;54;0
WireConnection;57;2;55;0
WireConnection;47;0;46;0
WireConnection;47;1;45;0
WireConnection;47;2;44;0
WireConnection;37;0;6;3
WireConnection;37;1;36;0
WireConnection;58;0;6;1
WireConnection;58;1;57;0
WireConnection;7;0;6;4
WireConnection;7;1;17;0
WireConnection;48;0;6;2
WireConnection;48;1;47;0
WireConnection;151;0;58;0
WireConnection;152;0;48;0
WireConnection;154;0;7;0
WireConnection;153;0;37;0
WireConnection;167;0;159;0
WireConnection;167;1;160;0
WireConnection;167;2;161;0
WireConnection;167;3;162;0
WireConnection;117;2;167;0
ASEEND*/
//CHKSM=1D3A959257AF35A87461B98BAEB1786E64F2FECE