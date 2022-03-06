// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Malbers/Masks8Toon"
{
	Properties
	{
		[NoScaleOffset]_Albedo("Albedo", 2D) = "white" {}
		[NoScaleOffset]_Mask1("Mask1", 2D) = "white" {}
		[NoScaleOffset]_Mask2("Mask2", 2D) = "white" {}
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
		[Header(Mask 2 Blue)]_M2BHue("M2 B Hue", Range( 0 , 2)) = 0
		_M2BSaturation("M2 B Saturation", Range( 0 , 2)) = 1
		_M2BValue("M2 B Value", Range( 0 , 2)) = 1
		[Header(Mask 2 Alpha)]_M2AHue("M2 A Hue", Range( 0 , 2)) = 1
		_M2ASaturation("M2 A Saturation", Range( 0 , 2)) = 1
		_M2AValue("M2 A Value", Range( 0 , 2)) = 1
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
			float2 uv_Mask16 = i.uv_texcoord;
			float4 tex2DNode6 = tex2D( _Mask1, uv_Mask16 );
			float2 uv_Albedo2 = i.uv_texcoord;
			float3 hsvTorgb119 = RGBToHSV( tex2D( _Albedo, uv_Albedo2 ).rgb );
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
			float2 uv_Mask2106 = i.uv_texcoord;
			float4 tex2DNode106 = tex2D( _Mask2, uv_Mask2106 );
			float3 hsvTorgb86 = HSVToRGB( float3(( Hue121 * _M2RHue ),( Saturation122 * _M2RSaturation ),( Value123 * _M2RValue )) );
			float3 M2R155 = ( tex2DNode106.r * hsvTorgb86 );
			float3 hsvTorgb88 = HSVToRGB( float3(( Hue121 * _M2GHue ),( Saturation122 * _M2GSaturation ),( Value123 * _M2GValue )) );
			float3 M2G156 = ( tex2DNode106.g * hsvTorgb88 );
			float3 hsvTorgb96 = HSVToRGB( float3(( Hue121 * _M2BHue ),( Saturation122 * _M2BSaturation ),( Value123 * _M2BValue )) );
			float3 M2B157 = ( tex2DNode106.b * hsvTorgb96 );
			float3 hsvTorgb87 = HSVToRGB( float3(( Hue121 * _M2AHue ),( Saturation122 * _M2ASaturation ),( Value123 * _M2AValue )) );
			float3 M2A158 = ( tex2DNode106.a * hsvTorgb87 );
			o.Emission = ( M1R151 + M1G152 + M1B153 + M1A154 + M2R155 + M2G156 + M2B157 + M2A158 );
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16200
81;249;1309;794;3131.61;-3846.605;1.376026;True;True
Node;AmplifyShaderEditor.SamplerNode;2;-315.9219,1976.152;Float;True;Property;_Albedo;Albedo;0;1;[NoScaleOffset];Create;True;0;0;False;0;None;7a875ee0f9fb55b4bb9e6e42c095cf5e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RGBToHSVNode;119;24.90613,1972.208;Float;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;60;-2746.199,2441.396;Float;False;2112.242;2667.315;Mask 2;13;99;102;103;100;106;64;62;63;61;155;156;157;158;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;59;-2744.899,-223.7064;Float;False;2096.119;2628.729;Mask 1;13;58;6;48;37;7;28;49;23;39;151;152;153;154;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;62;-2659.576,3783.514;Float;False;890.8004;612.024;Mask 2 Blue Channel;10;96;95;94;93;79;76;74;141;142;143;;0,0,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;61;-2682.089,2519.823;Float;False;890.8004;612.024;Mask 2 Red Channel;10;92;91;86;83;78;73;72;135;136;137;;1,0,0,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;121;326.4949,1977.09;Float;False;Hue;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;122;325.188,2051.3;Float;False;Saturation;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;39;-2668.544,496.7281;Float;False;890.8004;612.024;Mask 1 Green Channel;10;47;45;43;42;41;44;46;126;127;128;;0,1,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;28;-2675.228,1126.771;Float;False;890.8004;612.024;Mask 1 Blue Channel;10;36;32;31;30;35;33;34;129;130;131;;0,0,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;23;-2669.603,1763.231;Float;False;890.8004;612.024;Mask 1 Alpha Channel;10;18;19;21;20;15;17;22;132;133;134;;1,1,1,0.534;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;123;321.285,2132.019;Float;False;Value;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;64;-2673.049,3148.762;Float;False;890.8004;612.024;Mask 2 Green Channel;10;90;88;85;81;75;66;65;138;139;140;;0,1,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;49;-2676.232,-130.2395;Float;False;890.8004;612.024;Mask 1 Red Channel;10;57;56;54;52;51;50;55;120;124;125;;1,0,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;63;-2669.891,4419.647;Float;False;890.8004;612.024;Mask 2 Alpha Channel;10;89;87;84;82;80;77;67;144;145;146;;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;143;-2520.077,4196.341;Float;False;123;Value;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;89;-2618.53,4949.463;Float;False;Property;_M2AValue;M2 A Value;26;0;Create;True;0;0;False;0;1;1.39;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;144;-2557.115,4505.64;Float;False;121;Hue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-2623.725,1422.448;Float;False;Property;_M1BSaturation;M1 B Saturation;10;0;Create;True;0;0;False;0;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-2629.982,2262.474;Float;False;Property;_M1AValue;M1 A Value;14;0;Create;True;0;0;False;0;1;0.5;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;134;-2543.271,1841.598;Float;False;121;Hue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-2634.913,3508.891;Float;False;Property;_M2GSaturation;M2 G Saturation;19;0;Create;True;0;0;False;0;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-2667.464,2869.568;Float;False;Property;_M2RSaturation;M2 R Saturation;16;0;Create;True;0;0;False;0;1;1.18;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-2627.263,3661.105;Float;False;Property;_M2GValue;M2 G Value;20;0;Create;True;0;0;False;0;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-2616.394,4121.054;Float;False;Property;_M2BSaturation;M2 B Saturation;22;0;Create;True;0;0;False;0;1;0.85;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-2636.241,1920.213;Float;False;Property;_M1AHue;M1 A Hue;12;0;Create;True;0;0;False;1;Header(Mask 1 Alpha);1;0.7;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;76;-2603.811,4266.851;Float;False;Property;_M2BValue;M2 B Value;23;0;Create;True;0;0;False;0;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;142;-2534.236,4035.137;Float;False;122;Saturation;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;74;-2619.388,3952.04;Float;False;Property;_M2BHue;M2 B Hue;21;0;Create;True;0;0;False;1;Header(Mask 2 Blue);0;1.4;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-2586.596,147.5818;Float;False;Property;_M1RHue;M1 R Hue;3;0;Create;True;0;0;False;1;Header(Mask 1 Red);1;0.34;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-2635.182,668.7261;Float;False;Property;_M1GHue;M1 G Hue;6;0;Create;True;0;0;False;1;Header(Mask 1 Green);1;1.66;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-2620.845,1267.236;Float;False;Property;_M1BHue;M1 B Hue;9;0;Create;True;0;0;False;1;Header(Mask 1 Blue);1;1.79;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;146;-2521.724,4872.493;Float;False;123;Value;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-2655.846,2694.623;Float;False;Property;_M2RHue;M2 R Hue;15;0;Create;True;0;0;False;1;Header(Mask 2 Red);1;0.7;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;138;-2543.953,3240.929;Float;False;121;Hue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-2639.493,3329.407;Float;False;Property;_M2GHue;M2 G Hue;18;0;Create;True;0;0;False;1;Header(Mask 2 Green);1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;141;-2534.236,3875.137;Float;False;121;Hue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-2626.122,1590.761;Float;False;Property;_M1BValue;M1 B Value;11;0;Create;True;0;0;False;0;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;77;-2636.772,4587.55;Float;False;Property;_M2AHue;M2 A Hue;24;0;Create;True;0;0;False;1;Header(Mask 2 Alpha);1;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-2633.119,2089.934;Float;False;Property;_M1ASaturation;M1 A Saturation;13;0;Create;True;0;0;False;0;0.8705882;2;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;137;-2569.933,2966.408;Float;False;123;Value;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;120;-2522.85,-79.79761;Float;False;121;Hue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;145;-2557.115,4690.185;Float;False;122;Saturation;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-2630.372,827.941;Float;False;Property;_M1GSaturation;M1 G Saturation;7;0;Create;True;0;0;False;0;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;124;-2535.76,-5.495001;Float;False;122;Saturation;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;125;-2516.648,65.41222;Float;False;123;Value;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;133;-2553.18,2015.005;Float;False;122;Saturation;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;126;-2531.57,905.225;Float;False;123;Value;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;127;-2543.777,590.3352;Float;False;121;Hue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;129;-2530.532,1516.756;Float;False;123;Value;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-2623.943,978.7378;Float;False;Property;_M1GValue;M1 G Value;8;0;Create;True;0;0;False;0;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;132;-2534.066,2182.014;Float;False;123;Value;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-2581.231,225.0372;Float;False;Property;_M1RSaturation;M1 R Saturation;4;0;Create;True;0;0;False;0;1;1.46;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;135;-2561.34,2618.576;Float;False;121;Hue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;73;-2664.483,3045.028;Float;False;Property;_M2RValue;M2 R Value;17;0;Create;True;0;0;False;0;1;1.38;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;131;-2545.14,1349.747;Float;False;122;Saturation;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;136;-2578.663,2787.534;Float;False;122;Saturation;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;140;-2542.905,3588.703;Float;False;123;Value;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;139;-2559.953,3432.931;Float;False;122;Saturation;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;67;-2631.629,4763.152;Float;False;Property;_M2ASaturation;M2 A Saturation;25;0;Create;True;0;0;False;0;1;1.08;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;128;-2556.689,754.7337;Float;False;122;Saturation;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;130;-2526.222,1189.854;Float;False;121;Hue;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-2579.615,329.7841;Float;False;Property;_M1RValue;M1 R Value;5;0;Create;True;0;0;False;0;1;1.15;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-2287.196,1380.095;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-2253.012,847.9394;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;-2261.845,4034.227;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-2246.305,131.7758;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-2279.361,1512.335;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-2241.03,1887.857;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;-2243.121,3410.779;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;-2239.965,4681.662;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;85;-2244.474,3273.389;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;-2267.525,4157.458;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;-2263.198,3896.841;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;-2252.161,2781.837;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-2239.677,2025.246;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;90;-2237.341,3582.38;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-2253.987,741.6636;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-2302.065,1229.193;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-2255.94,2655.274;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-2251.563,640.3604;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;-2241.318,4544.273;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;-2251.185,2959.844;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-2245.357,2148.476;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;-2240.525,303.3756;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-2247.658,-5.612677;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;-2245.645,4804.892;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.HSVToRGBNode;86;-2049.349,2685.642;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;106;-1751.459,3640.118;Float;True;Property;_Mask2;Mask2;2;1;[NoScaleOffset];Create;True;0;0;False;0;None;60304423c74d58241a81eeb313b40cc6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.HSVToRGBNode;96;-2060.38,3955.155;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.HSVToRGBNode;36;-2069.434,1329.471;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.HSVToRGBNode;17;-2050.212,1930.535;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.HSVToRGBNode;47;-2049.153,664.0313;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.HSVToRGBNode;88;-2053.658,3316.068;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;6;-1710.516,1000.104;Float;True;Property;_Mask1;Mask1;1;1;[NoScaleOffset];Create;True;0;0;False;0;None;da5d3dd4daeb1c740986a95999b46256;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.HSVToRGBNode;87;-2019.244,4507.394;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.HSVToRGBNode;57;-2056.84,37.06535;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;102;-1263.775,3246.237;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;100;-1264.263,3786.241;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-1249.841,1282.853;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-1273.15,2.308438;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-1230.712,1891.554;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;103;-1266.56,2514.971;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-1246.602,633.2107;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;99;-1318.784,4462.76;Float;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;154;-898.1682,1891.038;Float;False;M1A;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;158;-1071.685,4443.853;Float;False;M2A;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;152;-958.8002,631.2532;Float;False;M1G;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;155;-962.4984,2519.94;Float;False;M2R;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;151;-948.0219,-4.70237;Float;False;M1R;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;156;-977.705,3246.509;Float;False;M2G;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;157;-1027.384,3781.959;Float;False;M2B;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;153;-911.6426,1294.156;Float;False;M1B;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;161;-327.4548,2411.204;Float;False;153;M1B;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;165;-335.9438,2817.065;Float;False;157;M2B;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;164;-337.2448,2746.865;Float;False;156;M2G;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;160;-327.0557,2339.004;Float;False;152;M1G;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;162;-323.5567,2489.204;Float;False;154;M1A;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;159;-325.9998,2267.163;Float;False;151;M1R;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;166;-332.0457,2895.064;Float;False;158;M2A;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;163;-334.4898,2673.024;Float;False;155;M2R;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;167;30.04527,2463.207;Float;True;8;8;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;117;280.6553,2412.303;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Malbers/Masks8Toon;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;119;0;2;0
WireConnection;121;0;119;1
WireConnection;122;0;119;2
WireConnection;123;0;119;3
WireConnection;33;0;131;0
WireConnection;33;1;31;0
WireConnection;44;0;126;0
WireConnection;44;1;42;0
WireConnection;94;0;142;0
WireConnection;94;1;79;0
WireConnection;54;0;124;0
WireConnection;54;1;51;0
WireConnection;35;0;129;0
WireConnection;35;1;32;0
WireConnection;18;0;134;0
WireConnection;18;1;15;0
WireConnection;81;0;139;0
WireConnection;81;1;66;0
WireConnection;80;0;145;0
WireConnection;80;1;67;0
WireConnection;85;0;138;0
WireConnection;85;1;65;0
WireConnection;93;0;143;0
WireConnection;93;1;76;0
WireConnection;95;0;141;0
WireConnection;95;1;74;0
WireConnection;83;0;136;0
WireConnection;83;1;72;0
WireConnection;19;0;133;0
WireConnection;19;1;20;0
WireConnection;90;0;140;0
WireConnection;90;1;75;0
WireConnection;45;0;128;0
WireConnection;45;1;43;0
WireConnection;34;0;130;0
WireConnection;34;1;30;0
WireConnection;92;0;135;0
WireConnection;92;1;78;0
WireConnection;46;0;127;0
WireConnection;46;1;41;0
WireConnection;82;0;144;0
WireConnection;82;1;77;0
WireConnection;91;0;137;0
WireConnection;91;1;73;0
WireConnection;21;0;132;0
WireConnection;21;1;22;0
WireConnection;55;0;125;0
WireConnection;55;1;50;0
WireConnection;56;0;120;0
WireConnection;56;1;52;0
WireConnection;84;0;146;0
WireConnection;84;1;89;0
WireConnection;86;0;92;0
WireConnection;86;1;83;0
WireConnection;86;2;91;0
WireConnection;96;0;95;0
WireConnection;96;1;94;0
WireConnection;96;2;93;0
WireConnection;36;0;34;0
WireConnection;36;1;33;0
WireConnection;36;2;35;0
WireConnection;17;0;18;0
WireConnection;17;1;19;0
WireConnection;17;2;21;0
WireConnection;47;0;46;0
WireConnection;47;1;45;0
WireConnection;47;2;44;0
WireConnection;88;0;85;0
WireConnection;88;1;81;0
WireConnection;88;2;90;0
WireConnection;87;0;82;0
WireConnection;87;1;80;0
WireConnection;87;2;84;0
WireConnection;57;0;56;0
WireConnection;57;1;54;0
WireConnection;57;2;55;0
WireConnection;102;0;106;2
WireConnection;102;1;88;0
WireConnection;100;0;106;3
WireConnection;100;1;96;0
WireConnection;37;0;6;3
WireConnection;37;1;36;0
WireConnection;58;0;6;1
WireConnection;58;1;57;0
WireConnection;7;0;6;4
WireConnection;7;1;17;0
WireConnection;103;0;106;1
WireConnection;103;1;86;0
WireConnection;48;0;6;2
WireConnection;48;1;47;0
WireConnection;99;0;106;4
WireConnection;99;1;87;0
WireConnection;154;0;7;0
WireConnection;158;0;99;0
WireConnection;152;0;48;0
WireConnection;155;0;103;0
WireConnection;151;0;58;0
WireConnection;156;0;102;0
WireConnection;157;0;100;0
WireConnection;153;0;37;0
WireConnection;167;0;159;0
WireConnection;167;1;160;0
WireConnection;167;2;161;0
WireConnection;167;3;162;0
WireConnection;167;4;163;0
WireConnection;167;5;164;0
WireConnection;167;6;165;0
WireConnection;167;7;166;0
WireConnection;117;2;167;0
ASEEND*/
//CHKSM=B7729015F8FB6F003B39445B7B3A8A601A265AF8