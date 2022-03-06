// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Malbers/Color4x4v2"
{
	Properties
	{
		[Header(Albedo (A Gradient))]_Color1("Color 1", Color) = (1,0.1544118,0.1544118,0.291)
		_Color2("Color 2", Color) = (1,0.1544118,0.8017241,0.253)
		_Color3("Color 3", Color) = (0.2535501,0.1544118,1,0.541)
		_Color4("Color 4", Color) = (0.1544118,0.5451319,1,0.253)
		[Space(10)]_Color5("Color 5", Color) = (0.9533468,1,0.1544118,0.553)
		_Color6("Color 6", Color) = (0.2720588,0.1294625,0,0.097)
		_Color7("Color 7", Color) = (0.1544118,0.6151115,1,0.178)
		_Color8("Color 8", Color) = (0.4849697,0.5008695,0.5073529,0.078)
		[Space(10)]_Color9("Color 9", Color) = (0.3164301,0,0.7058823,0.134)
		_Color10("Color 10", Color) = (0.362069,0.4411765,0,0.759)
		_Color11("Color 11", Color) = (0.6691177,0.6691177,0.6691177,0.647)
		_Color12("Color 12", Color) = (0.5073529,0.1574544,0,0.128)
		[Space(10)]_Color13("Color 13", Color) = (1,0.5586207,0,0.272)
		_Color14("Color 14", Color) = (0,0.8025862,0.875,0.047)
		_Color15("Color 15", Color) = (1,0,0,0.391)
		_Color16("Color 16", Color) = (0.4080882,0.75,0.4811866,0.134)
		[Header(Metallic(R) Rough(G) Emmission(B))]_MRE1("MRE 1", Color) = (0,1,0,0)
		_MRE2("MRE 2", Color) = (0,1,0,0)
		_MRE3("MRE 3", Color) = (0,1,0,0)
		_MRE4("MRE 4", Color) = (0,1,0,0)
		[Space(10)]_MRE5("MRE 5", Color) = (0,1,0,0)
		_MRE6("MRE 6", Color) = (0,1,0,0)
		_MRE7("MRE 7", Color) = (0,1,0,0)
		_MRE8("MRE 8", Color) = (0,1,0,0)
		[Space(10)]_MRE9("MRE 9", Color) = (0,1,0,0)
		_MRE10("MRE 10", Color) = (0,1,0,0)
		_MRE11("MRE 11", Color) = (0,1,0,0)
		_MRE12("MRE 12", Color) = (0,1,0,0)
		[Space(10)]_MRE13("MRE 13", Color) = (0,1,0,0)
		_MRE14("MRE 14", Color) = (0,1,0,0)
		_MRE15("MRE 15", Color) = (0,1,0,0)
		_MRE16("MRE 16", Color) = (0,1,0,0)
		[Header(Emmision)]_EmissionPower1("Emission Power", Float) = 1
		[SingleLineTexture][Header(Gradient)]_Gradient("Gradient", 2D) = "white" {}
		_GradientIntensity("Gradient Intensity", Range( 0 , 1)) = 0.75
		_GradientColor("Gradient Color", Color) = (0,0,0,0)
		_GradientScale("Gradient Scale", Float) = 1
		_GradientOffset("Gradient Offset", Float) = 0
		_GradientPower("Gradient Power", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Gradient;
		uniform float4 _GradientColor;
		uniform float _GradientIntensity;
		uniform float _GradientScale;
		uniform float _GradientOffset;
		uniform float _GradientPower;
		uniform float4 _Color1;
		uniform float4 _Color2;
		uniform float4 _Color3;
		uniform float4 _Color4;
		uniform float4 _Color5;
		uniform float4 _Color6;
		uniform float4 _Color7;
		uniform float4 _Color8;
		uniform float4 _Color9;
		uniform float4 _Color10;
		uniform float4 _Color11;
		uniform float4 _Color12;
		uniform float4 _Color13;
		uniform float4 _Color14;
		uniform float4 _Color15;
		uniform float4 _Color16;
		uniform float _EmissionPower1;
		uniform float4 _MRE1;
		uniform float4 _MRE2;
		uniform float4 _MRE3;
		uniform float4 _MRE4;
		uniform float4 _MRE5;
		uniform float4 _MRE6;
		uniform float4 _MRE7;
		uniform float4 _MRE8;
		uniform float4 _MRE9;
		uniform float4 _MRE10;
		uniform float4 _MRE11;
		uniform float4 _MRE12;
		uniform float4 _MRE13;
		uniform float4 _MRE14;
		uniform float4 _MRE15;
		uniform float4 _MRE16;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord255 = i.uv_texcoord * float2( 1,4 );
			float4 clampResult234 = clamp( ( ( tex2D( _Gradient, uv_TexCoord255 ) + _GradientColor ) + ( 1.0 - _GradientIntensity ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 temp_cast_0 = (_GradientPower).xxxx;
			float temp_output_3_0_g654 = 1.0;
			float temp_output_7_0_g654 = 4.0;
			float temp_output_9_0_g654 = 4.0;
			float temp_output_8_0_g654 = 4.0;
			float temp_output_3_0_g655 = 2.0;
			float temp_output_7_0_g655 = 4.0;
			float temp_output_9_0_g655 = 4.0;
			float temp_output_8_0_g655 = 4.0;
			float temp_output_3_0_g658 = 3.0;
			float temp_output_7_0_g658 = 4.0;
			float temp_output_9_0_g658 = 4.0;
			float temp_output_8_0_g658 = 4.0;
			float temp_output_3_0_g656 = 4.0;
			float temp_output_7_0_g656 = 4.0;
			float temp_output_9_0_g656 = 4.0;
			float temp_output_8_0_g656 = 4.0;
			float temp_output_3_0_g667 = 1.0;
			float temp_output_7_0_g667 = 4.0;
			float temp_output_9_0_g667 = 3.0;
			float temp_output_8_0_g667 = 4.0;
			float temp_output_3_0_g665 = 2.0;
			float temp_output_7_0_g665 = 4.0;
			float temp_output_9_0_g665 = 3.0;
			float temp_output_8_0_g665 = 4.0;
			float temp_output_3_0_g653 = 3.0;
			float temp_output_7_0_g653 = 4.0;
			float temp_output_9_0_g653 = 3.0;
			float temp_output_8_0_g653 = 4.0;
			float temp_output_3_0_g660 = 4.0;
			float temp_output_7_0_g660 = 4.0;
			float temp_output_9_0_g660 = 3.0;
			float temp_output_8_0_g660 = 4.0;
			float temp_output_3_0_g666 = 1.0;
			float temp_output_7_0_g666 = 4.0;
			float temp_output_9_0_g666 = 2.0;
			float temp_output_8_0_g666 = 4.0;
			float temp_output_3_0_g664 = 2.0;
			float temp_output_7_0_g664 = 4.0;
			float temp_output_9_0_g664 = 2.0;
			float temp_output_8_0_g664 = 4.0;
			float temp_output_3_0_g661 = 3.0;
			float temp_output_7_0_g661 = 4.0;
			float temp_output_9_0_g661 = 2.0;
			float temp_output_8_0_g661 = 4.0;
			float temp_output_3_0_g668 = 4.0;
			float temp_output_7_0_g668 = 4.0;
			float temp_output_9_0_g668 = 2.0;
			float temp_output_8_0_g668 = 4.0;
			float temp_output_3_0_g659 = 1.0;
			float temp_output_7_0_g659 = 4.0;
			float temp_output_9_0_g659 = 1.0;
			float temp_output_8_0_g659 = 4.0;
			float temp_output_3_0_g663 = 2.0;
			float temp_output_7_0_g663 = 4.0;
			float temp_output_9_0_g663 = 1.0;
			float temp_output_8_0_g663 = 4.0;
			float temp_output_3_0_g657 = 3.0;
			float temp_output_7_0_g657 = 4.0;
			float temp_output_9_0_g657 = 1.0;
			float temp_output_8_0_g657 = 4.0;
			float temp_output_3_0_g662 = 4.0;
			float temp_output_7_0_g662 = 4.0;
			float temp_output_9_0_g662 = 1.0;
			float temp_output_8_0_g662 = 4.0;
			float4 temp_output_155_0 = ( ( ( _Color1 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g654 - 1.0 ) / temp_output_7_0_g654 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g654 / temp_output_7_0_g654 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g654 - 1.0 ) / temp_output_8_0_g654 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g654 / temp_output_8_0_g654 ) ) * 1.0 ) ) ) ) + ( _Color2 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g655 - 1.0 ) / temp_output_7_0_g655 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g655 / temp_output_7_0_g655 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g655 - 1.0 ) / temp_output_8_0_g655 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g655 / temp_output_8_0_g655 ) ) * 1.0 ) ) ) ) + ( _Color3 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g658 - 1.0 ) / temp_output_7_0_g658 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g658 / temp_output_7_0_g658 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g658 - 1.0 ) / temp_output_8_0_g658 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g658 / temp_output_8_0_g658 ) ) * 1.0 ) ) ) ) + ( _Color4 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g656 - 1.0 ) / temp_output_7_0_g656 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g656 / temp_output_7_0_g656 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g656 - 1.0 ) / temp_output_8_0_g656 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g656 / temp_output_8_0_g656 ) ) * 1.0 ) ) ) ) ) + ( ( _Color5 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g667 - 1.0 ) / temp_output_7_0_g667 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g667 / temp_output_7_0_g667 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g667 - 1.0 ) / temp_output_8_0_g667 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g667 / temp_output_8_0_g667 ) ) * 1.0 ) ) ) ) + ( _Color6 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g665 - 1.0 ) / temp_output_7_0_g665 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g665 / temp_output_7_0_g665 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g665 - 1.0 ) / temp_output_8_0_g665 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g665 / temp_output_8_0_g665 ) ) * 1.0 ) ) ) ) + ( _Color7 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g653 - 1.0 ) / temp_output_7_0_g653 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g653 / temp_output_7_0_g653 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g653 - 1.0 ) / temp_output_8_0_g653 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g653 / temp_output_8_0_g653 ) ) * 1.0 ) ) ) ) + ( _Color8 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g660 - 1.0 ) / temp_output_7_0_g660 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g660 / temp_output_7_0_g660 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g660 - 1.0 ) / temp_output_8_0_g660 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g660 / temp_output_8_0_g660 ) ) * 1.0 ) ) ) ) ) + ( ( _Color9 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g666 - 1.0 ) / temp_output_7_0_g666 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g666 / temp_output_7_0_g666 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g666 - 1.0 ) / temp_output_8_0_g666 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g666 / temp_output_8_0_g666 ) ) * 1.0 ) ) ) ) + ( _Color10 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g664 - 1.0 ) / temp_output_7_0_g664 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g664 / temp_output_7_0_g664 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g664 - 1.0 ) / temp_output_8_0_g664 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g664 / temp_output_8_0_g664 ) ) * 1.0 ) ) ) ) + ( _Color11 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g661 - 1.0 ) / temp_output_7_0_g661 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g661 / temp_output_7_0_g661 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g661 - 1.0 ) / temp_output_8_0_g661 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g661 / temp_output_8_0_g661 ) ) * 1.0 ) ) ) ) + ( _Color12 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g668 - 1.0 ) / temp_output_7_0_g668 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g668 / temp_output_7_0_g668 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g668 - 1.0 ) / temp_output_8_0_g668 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g668 / temp_output_8_0_g668 ) ) * 1.0 ) ) ) ) ) + ( ( _Color13 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g659 - 1.0 ) / temp_output_7_0_g659 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g659 / temp_output_7_0_g659 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g659 - 1.0 ) / temp_output_8_0_g659 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g659 / temp_output_8_0_g659 ) ) * 1.0 ) ) ) ) + ( _Color14 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g663 - 1.0 ) / temp_output_7_0_g663 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g663 / temp_output_7_0_g663 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g663 - 1.0 ) / temp_output_8_0_g663 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g663 / temp_output_8_0_g663 ) ) * 1.0 ) ) ) ) + ( _Color15 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g657 - 1.0 ) / temp_output_7_0_g657 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g657 / temp_output_7_0_g657 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g657 - 1.0 ) / temp_output_8_0_g657 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g657 / temp_output_8_0_g657 ) ) * 1.0 ) ) ) ) + ( _Color16 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g662 - 1.0 ) / temp_output_7_0_g662 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g662 / temp_output_7_0_g662 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g662 - 1.0 ) / temp_output_8_0_g662 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g662 / temp_output_8_0_g662 ) ) * 1.0 ) ) ) ) ) );
			float4 clampResult261 = clamp( ( pow( (clampResult234*_GradientScale + _GradientOffset) , temp_cast_0 ) + ( 1.0 - (temp_output_155_0).a ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
			o.Albedo = ( clampResult261 * temp_output_155_0 ).rgb;
			float temp_output_3_0_g690 = 1.0;
			float temp_output_7_0_g690 = 4.0;
			float temp_output_9_0_g690 = 4.0;
			float temp_output_8_0_g690 = 4.0;
			float temp_output_3_0_g669 = 2.0;
			float temp_output_7_0_g669 = 4.0;
			float temp_output_9_0_g669 = 4.0;
			float temp_output_8_0_g669 = 4.0;
			float temp_output_3_0_g684 = 3.0;
			float temp_output_7_0_g684 = 4.0;
			float temp_output_9_0_g684 = 4.0;
			float temp_output_8_0_g684 = 4.0;
			float temp_output_3_0_g688 = 4.0;
			float temp_output_7_0_g688 = 4.0;
			float temp_output_9_0_g688 = 4.0;
			float temp_output_8_0_g688 = 4.0;
			float temp_output_3_0_g687 = 1.0;
			float temp_output_7_0_g687 = 4.0;
			float temp_output_9_0_g687 = 3.0;
			float temp_output_8_0_g687 = 4.0;
			float temp_output_3_0_g694 = 2.0;
			float temp_output_7_0_g694 = 4.0;
			float temp_output_9_0_g694 = 3.0;
			float temp_output_8_0_g694 = 4.0;
			float temp_output_3_0_g679 = 3.0;
			float temp_output_7_0_g679 = 4.0;
			float temp_output_9_0_g679 = 3.0;
			float temp_output_8_0_g679 = 4.0;
			float temp_output_3_0_g691 = 4.0;
			float temp_output_7_0_g691 = 4.0;
			float temp_output_9_0_g691 = 3.0;
			float temp_output_8_0_g691 = 4.0;
			float temp_output_3_0_g674 = 1.0;
			float temp_output_7_0_g674 = 4.0;
			float temp_output_9_0_g674 = 2.0;
			float temp_output_8_0_g674 = 4.0;
			float temp_output_3_0_g689 = 2.0;
			float temp_output_7_0_g689 = 4.0;
			float temp_output_9_0_g689 = 2.0;
			float temp_output_8_0_g689 = 4.0;
			float temp_output_3_0_g678 = 3.0;
			float temp_output_7_0_g678 = 4.0;
			float temp_output_9_0_g678 = 2.0;
			float temp_output_8_0_g678 = 4.0;
			float temp_output_3_0_g672 = 4.0;
			float temp_output_7_0_g672 = 4.0;
			float temp_output_9_0_g672 = 2.0;
			float temp_output_8_0_g672 = 4.0;
			float temp_output_3_0_g685 = 1.0;
			float temp_output_7_0_g685 = 4.0;
			float temp_output_9_0_g685 = 1.0;
			float temp_output_8_0_g685 = 4.0;
			float temp_output_3_0_g692 = 2.0;
			float temp_output_7_0_g692 = 4.0;
			float temp_output_9_0_g692 = 1.0;
			float temp_output_8_0_g692 = 4.0;
			float temp_output_3_0_g693 = 3.0;
			float temp_output_7_0_g693 = 4.0;
			float temp_output_9_0_g693 = 1.0;
			float temp_output_8_0_g693 = 4.0;
			float temp_output_3_0_g677 = 4.0;
			float temp_output_7_0_g677 = 4.0;
			float temp_output_9_0_g677 = 1.0;
			float temp_output_8_0_g677 = 4.0;
			float4 temp_output_283_0 = ( ( ( _MRE1 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g690 - 1.0 ) / temp_output_7_0_g690 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g690 / temp_output_7_0_g690 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g690 - 1.0 ) / temp_output_8_0_g690 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g690 / temp_output_8_0_g690 ) ) * 1.0 ) ) ) ) + ( _MRE2 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g669 - 1.0 ) / temp_output_7_0_g669 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g669 / temp_output_7_0_g669 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g669 - 1.0 ) / temp_output_8_0_g669 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g669 / temp_output_8_0_g669 ) ) * 1.0 ) ) ) ) + ( _MRE3 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g684 - 1.0 ) / temp_output_7_0_g684 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g684 / temp_output_7_0_g684 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g684 - 1.0 ) / temp_output_8_0_g684 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g684 / temp_output_8_0_g684 ) ) * 1.0 ) ) ) ) + ( _MRE4 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g688 - 1.0 ) / temp_output_7_0_g688 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g688 / temp_output_7_0_g688 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g688 - 1.0 ) / temp_output_8_0_g688 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g688 / temp_output_8_0_g688 ) ) * 1.0 ) ) ) ) ) + ( ( _MRE5 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g687 - 1.0 ) / temp_output_7_0_g687 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g687 / temp_output_7_0_g687 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g687 - 1.0 ) / temp_output_8_0_g687 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g687 / temp_output_8_0_g687 ) ) * 1.0 ) ) ) ) + ( _MRE6 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g694 - 1.0 ) / temp_output_7_0_g694 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g694 / temp_output_7_0_g694 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g694 - 1.0 ) / temp_output_8_0_g694 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g694 / temp_output_8_0_g694 ) ) * 1.0 ) ) ) ) + ( _MRE7 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g679 - 1.0 ) / temp_output_7_0_g679 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g679 / temp_output_7_0_g679 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g679 - 1.0 ) / temp_output_8_0_g679 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g679 / temp_output_8_0_g679 ) ) * 1.0 ) ) ) ) + ( _MRE8 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g691 - 1.0 ) / temp_output_7_0_g691 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g691 / temp_output_7_0_g691 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g691 - 1.0 ) / temp_output_8_0_g691 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g691 / temp_output_8_0_g691 ) ) * 1.0 ) ) ) ) ) + ( ( _MRE9 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g674 - 1.0 ) / temp_output_7_0_g674 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g674 / temp_output_7_0_g674 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g674 - 1.0 ) / temp_output_8_0_g674 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g674 / temp_output_8_0_g674 ) ) * 1.0 ) ) ) ) + ( _MRE10 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g689 - 1.0 ) / temp_output_7_0_g689 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g689 / temp_output_7_0_g689 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g689 - 1.0 ) / temp_output_8_0_g689 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g689 / temp_output_8_0_g689 ) ) * 1.0 ) ) ) ) + ( _MRE11 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g678 - 1.0 ) / temp_output_7_0_g678 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g678 / temp_output_7_0_g678 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g678 - 1.0 ) / temp_output_8_0_g678 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g678 / temp_output_8_0_g678 ) ) * 1.0 ) ) ) ) + ( _MRE12 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g672 - 1.0 ) / temp_output_7_0_g672 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g672 / temp_output_7_0_g672 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g672 - 1.0 ) / temp_output_8_0_g672 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g672 / temp_output_8_0_g672 ) ) * 1.0 ) ) ) ) ) + ( ( _MRE13 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g685 - 1.0 ) / temp_output_7_0_g685 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g685 / temp_output_7_0_g685 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g685 - 1.0 ) / temp_output_8_0_g685 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g685 / temp_output_8_0_g685 ) ) * 1.0 ) ) ) ) + ( _MRE14 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g692 - 1.0 ) / temp_output_7_0_g692 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g692 / temp_output_7_0_g692 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g692 - 1.0 ) / temp_output_8_0_g692 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g692 / temp_output_8_0_g692 ) ) * 1.0 ) ) ) ) + ( _MRE15 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g693 - 1.0 ) / temp_output_7_0_g693 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g693 / temp_output_7_0_g693 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g693 - 1.0 ) / temp_output_8_0_g693 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g693 / temp_output_8_0_g693 ) ) * 1.0 ) ) ) ) + ( _MRE16 * ( ( ( 1.0 - step( i.uv_texcoord.x , ( ( temp_output_3_0_g677 - 1.0 ) / temp_output_7_0_g677 ) ) ) * ( step( i.uv_texcoord.x , ( temp_output_3_0_g677 / temp_output_7_0_g677 ) ) * 1.0 ) ) * ( ( 1.0 - step( i.uv_texcoord.y , ( ( temp_output_9_0_g677 - 1.0 ) / temp_output_8_0_g677 ) ) ) * ( step( i.uv_texcoord.y , ( temp_output_9_0_g677 / temp_output_8_0_g677 ) ) * 1.0 ) ) ) ) ) );
			o.Emission = ( temp_output_155_0 * ( _EmissionPower1 * (temp_output_283_0).b ) ).rgb;
			o.Metallic = (temp_output_283_0).r;
			o.Smoothness = ( 1.0 - (temp_output_283_0).g );
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
}
/*ASEBEGIN
Version=18702
149;143;1535;830;1913.853;1169.105;3.681664;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;255;615.0826,-371.7701;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,4;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;159;-187.9672,688.0273;Float;False;Property;_Color5;Color 5;4;0;Create;True;0;0;False;1;Space(10);False;0.9533468,1,0.1544118,0.553;0.6838235,0.6476211,0.5933174,0.291;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;213;-234.6901,2683.007;Float;False;Property;_Color13;Color 13;12;0;Create;True;0;0;False;1;Space(10);False;1,0.5586207,0,0.272;0.6132076,0.6132076,0.6132076,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;230;859.2263,-398.1579;Inherit;True;Property;_Gradient;Gradient;33;1;[SingleLineTexture];Create;True;0;0;False;1;Header(Gradient);False;-1;0f424a347039ef447a763d3d4b4782b0;0f424a347039ef447a763d3d4b4782b0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;214;-242.6307,2942.365;Float;False;Property;_Color14;Color 14;13;0;Create;True;0;0;False;0;False;0,0.8025862,0.875,0.047;0.5441177,0.5441177,0.5441177,0.047;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;152;-194.2135,166.9271;Float;False;Property;_Color3;Color 3;2;0;Create;True;0;0;False;0;False;0.2535501,0.1544118,1,0.541;0.6838235,0.6476211,0.5933174,0.291;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;158;-183.7895,1424.406;Float;False;Property;_Color8;Color 8;7;0;Create;True;0;0;False;0;False;0.4849697,0.5008695,0.5073529,0.078;0.6838235,0.6476211,0.5933174,0.291;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;181;-218.8154,2174.284;Float;False;Property;_Color11;Color 11;10;0;Create;True;0;0;False;0;False;0.6691177,0.6691177,0.6691177,0.647;0.6838235,0.6476211,0.5933174,0.291;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;229;874.0561,-170.8387;Float;False;Property;_GradientColor;Gradient Color;35;0;Create;True;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;23;-199.8005,-326.2955;Float;False;Property;_Color1;Color 1;0;0;Create;True;0;0;False;1;Header(Albedo (A Gradient));False;1,0.1544118,0.1544118,0.291;0.6838235,0.6476211,0.5933174,0.7803922;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;218;-229.103,3176.23;Float;False;Property;_Color15;Color 15;14;0;Create;True;0;0;False;0;False;1,0,0,0.391;0.5188679,0.4068953,0.07097723,0.847;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;157;-182.3802,1181.25;Float;False;Property;_Color7;Color 7;6;0;Create;True;0;0;False;0;False;0.1544118,0.6151115,1,0.178;0.6838235,0.6476211,0.5933174,0.291;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;180;-232.3431,1940.419;Float;False;Property;_Color10;Color 10;9;0;Create;True;0;0;False;0;False;0.362069,0.4411765,0,0.759;0.5514706,0.5514706,0.5514706,0.591;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;182;-220.2247,2417.44;Float;False;Property;_Color12;Color 12;11;0;Create;True;0;0;False;0;False;0.5073529,0.1574544,0,0.128;0.6838235,0.6476211,0.5933174,0.291;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;228;845.7399,12.46821;Float;False;Property;_GradientIntensity;Gradient Intensity;34;0;Create;True;0;0;False;0;False;0.75;0.75;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;154;-195.6228,411.2479;Float;False;Property;_Color4;Color 4;3;0;Create;True;0;0;False;0;False;0.1544118,0.5451319,1,0.253;0.6838235,0.6476211,0.5933174,0.291;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;156;-195.9079,947.3851;Float;False;Property;_Color6;Color 6;5;0;Create;True;0;0;False;0;False;0.2720588,0.1294625,0,0.097;0.6838235,0.6476211,0.5933174,0.291;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;217;-264.3738,3419.386;Float;False;Property;_Color16;Color 16;15;0;Create;True;0;0;False;0;False;0.4080882,0.75,0.4811866,0.134;0.5849056,0.5849056,0.5849056,0.547;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;183;-224.4024,1681.061;Float;False;Property;_Color9;Color 9;8;0;Create;True;0;0;False;1;Space(10);False;0.3164301,0,0.7058823,0.134;0.6838235,0.6476211,0.5933174,0.291;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;150;-207.7412,-66.93771;Float;False;Property;_Color2;Color 2;1;0;Create;True;0;0;False;0;False;1,0.1544118,0.8017241,0.253;0.6132076,0.6132076,0.6132076,0.291;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;186;96.90227,2179.125;Inherit;True;ColorShartSlot;-1;;661;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;3;False;9;FLOAT;2;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;185;97.41646,2422.281;Inherit;True;ColorShartSlot;-1;;668;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;4;False;9;FLOAT;2;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;163;127.7504,692.868;Inherit;True;ColorShartSlot;-1;;667;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;1;False;9;FLOAT;3;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;188;91.31517,1685.902;Inherit;True;ColorShartSlot;-1;;666;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;1;False;9;FLOAT;2;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;160;119.8096,952.2258;Inherit;True;ColorShartSlot;-1;;665;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;2;False;9;FLOAT;3;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;187;83.37437,1945.26;Inherit;True;ColorShartSlot;-1;;664;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;2;False;9;FLOAT;2;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;223;73.08682,2945.046;Inherit;True;ColorShartSlot;-1;;663;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;2;False;9;FLOAT;1;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;222;87.12894,3424.227;Inherit;True;ColorShartSlot;-1;;662;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;4;False;9;FLOAT;1;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;162;133.8517,1429.247;Inherit;True;ColorShartSlot;-1;;660;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;4;False;9;FLOAT;3;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;149;107.9764,-62.09709;Inherit;True;ColorShartSlot;-1;;655;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;2;False;9;FLOAT;4;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;151;121.5042,171.7677;Inherit;True;ColorShartSlot;-1;;658;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;3;False;9;FLOAT;4;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;224;86.61465,3181.071;Inherit;True;ColorShartSlot;-1;;657;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;3;False;9;FLOAT;1;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;153;122.0185,414.924;Inherit;True;ColorShartSlot;-1;;656;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;4;False;9;FLOAT;4;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;231;1182.122,-372.6908;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;145;115.9171,-321.4549;Inherit;True;ColorShartSlot;-1;;654;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;1;False;9;FLOAT;4;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;161;133.3375,1186.091;Inherit;True;ColorShartSlot;-1;;653;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;3;False;9;FLOAT;3;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;232;1156.605,-68.40891;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;216;81.02762,2687.848;Inherit;True;ColorShartSlot;-1;;659;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;0.7843138,0.3137255,0,0;False;3;FLOAT;1;False;9;FLOAT;1;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;293;888.8267,5554.006;Float;False;Property;_MRE15;MRE 15;30;0;Create;True;0;0;False;0;False;0,1,0,0;0,1,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;233;1367.421,-383.9108;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;225;683.3512,1524.765;Inherit;True;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;292;886.7413,5342.351;Float;False;Property;_MRE14;MRE 14;29;0;Create;True;0;0;False;0;False;0,1,0,0;0,1,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;287;879.1714,4698.762;Float;False;Property;_MRE12;MRE 12;27;0;Create;True;0;0;False;0;False;0,1,0,0;0,1,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;184;686.7443,1260.558;Inherit;True;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;294;884.4691,5770.134;Float;False;Property;_MRE16;MRE 16;31;0;Create;True;0;0;False;0;False;0,1,0,0;0,1,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;288;881.2148,4482.635;Float;False;Property;_MRE11;MRE 11;26;0;Create;True;0;0;False;0;False;0,1,0,0;0,1,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;146;688.2412,727.387;Inherit;True;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;164;688.9302,993.4156;Inherit;True;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;262;888.3417,3297.014;Float;False;Property;_MRE6;MRE 6;21;0;Create;True;0;0;False;0;False;0,1,0,0;0.9098039,0.8666667,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;265;928.9007,2379.895;Float;False;Property;_MRE2;MRE 2;17;0;Create;True;0;0;False;0;False;0,1,0,0;0.9098039,0.8666667,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;269;876.3038,4037.208;Float;False;Property;_MRE9;MRE 9;24;0;Create;True;0;0;False;1;Space(10);False;0,1,0,0;0.9098039,0.8666667,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;291;881.6017,5108.58;Float;False;Property;_MRE13;MRE 13;28;0;Create;True;0;0;False;1;Space(10);False;0,1,0,0;0,1,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;263;924.805,2591.167;Float;False;Property;_MRE3;MRE 3;18;0;Create;True;0;0;False;0;False;0,1,0,0;0.9098039,0.8666667,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;264;887.6243,3070.586;Float;False;Property;_MRE5;MRE 5;20;0;Create;True;0;0;False;1;Space(10);False;0,1,0,0;0.9098039,0.8666667,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;290;881.4436,4270.979;Float;False;Property;_MRE10;MRE 10;25;0;Create;True;0;0;False;0;False;0,1,0,0;0,1,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;267;883.4818,3508.669;Float;False;Property;_MRE7;MRE 7;22;0;Create;True;0;0;False;0;False;0,1,0,0;0.9098039,0.8666667,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;270;881.4384,3724.796;Float;False;Property;_MRE8;MRE 8;23;0;Create;True;0;0;False;0;False;0,1,0,0;0.9098039,0.8666667,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;268;920.3644,2166.243;Float;False;Property;_MRE1;MRE 1;16;0;Create;True;0;0;False;1;Header(Metallic(R) Rough(G) Emmission(B));False;0,1,0,0;0.9098039,0.8666667,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;266;927.7203,2799.667;Float;False;Property;_MRE4;MRE 4;19;0;Create;True;0;0;False;0;False;0,1,0,0;0.9098039,0.8666667,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;295;1178.695,5106.292;Inherit;True;ColorShartSlot;-1;;685;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;1;False;9;FLOAT;1;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;297;1194.631,5541.872;Inherit;True;ColorShartSlot;-1;;693;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;3;False;9;FLOAT;1;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;296;1192.958,5326.958;Inherit;True;ColorShartSlot;-1;;692;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;2;False;9;FLOAT;1;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;276;1182.602,3719.032;Inherit;True;ColorShartSlot;-1;;691;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;4;False;9;FLOAT;3;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;273;1187.369,3281.62;Inherit;True;ColorShartSlot;-1;;694;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;2;False;9;FLOAT;3;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;289;1187.66,4255.586;Inherit;True;ColorShartSlot;-1;;689;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;2;False;9;FLOAT;2;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;277;1200.345,2802.812;Inherit;True;ColorShartSlot;-1;;688;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;4;False;9;FLOAT;4;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;278;1189.738,3073.549;Inherit;True;ColorShartSlot;-1;;687;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;1;False;9;FLOAT;3;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;274;1200.378,2592.315;Inherit;True;ColorShartSlot;-1;;684;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;3;False;9;FLOAT;4;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;275;1204.517,2165.975;Inherit;True;ColorShartSlot;-1;;690;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;1;False;9;FLOAT;4;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;238;1648.681,-135.2692;Float;False;Property;_GradientScale;Gradient Scale;36;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;239;1655.499,-42.52599;Float;False;Property;_GradientOffset;Gradient Offset;37;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;285;1189.334,4470.5;Inherit;True;ColorShartSlot;-1;;678;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;3;False;9;FLOAT;2;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;234;1652.17,-392.4709;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;155;1016.686,1030.498;Inherit;True;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;298;1185.632,5764.37;Inherit;True;ColorShartSlot;-1;;677;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;4;False;9;FLOAT;1;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;272;1173.398,4034.921;Inherit;True;ColorShartSlot;-1;;674;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;1;False;9;FLOAT;2;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;286;1180.335,4692.998;Inherit;True;ColorShartSlot;-1;;672;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;4;False;9;FLOAT;2;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;271;1202.304,2381.9;Inherit;True;ColorShartSlot;-1;;669;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;2;False;9;FLOAT;4;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;279;1189.043,3496.534;Inherit;True;ColorShartSlot;-1;;679;231fe18505db4a84b9c478d379c9247d;0;5;38;COLOR;1,1,1,1;False;3;FLOAT;3;False;9;FLOAT;3;False;7;FLOAT;4;False;8;FLOAT;4;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;299;1571.928,5274.19;Inherit;True;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;280;1566.631,4202.819;Inherit;True;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;281;1569.768,2500.448;Inherit;True;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;245;1659.99,53.90989;Float;False;Property;_GradientPower;Gradient Power;38;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;282;1553.285,3234.033;Inherit;True;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;256;2060.061,-74.53971;Inherit;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;237;1929.951,-353.3528;Inherit;True;3;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;258;2237.497,-354.0456;Inherit;True;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;259;2282.192,-66.26985;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;283;1960.203,2821.543;Inherit;True;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;302;2524.654,1230.847;Inherit;False;Property;_EmissionPower1;Emission Power;32;0;Create;True;0;0;False;1;Header(Emmision);False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;301;2514.004,1365.87;Inherit;True;False;False;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;260;2530.138,-355.3468;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;261;2796.672,-358.3472;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;306;2576.238,929.5774;Inherit;True;False;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;303;2828.549,1217.506;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;304;3114.618,1152.562;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;284;2918.832,2649.558;Inherit;True;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;236;3433.932,229.384;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;307;2570.26,731.6305;Inherit;True;True;False;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;305;2944.843,925.5689;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;4040.206,769.1205;Float;False;True;-1;2;;0;0;Standard;Malbers/Color4x4v2;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.1;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;230;1;255;0
WireConnection;186;38;181;0
WireConnection;185;38;182;0
WireConnection;163;38;159;0
WireConnection;188;38;183;0
WireConnection;160;38;156;0
WireConnection;187;38;180;0
WireConnection;223;38;214;0
WireConnection;222;38;217;0
WireConnection;162;38;158;0
WireConnection;149;38;150;0
WireConnection;151;38;152;0
WireConnection;224;38;218;0
WireConnection;153;38;154;0
WireConnection;231;0;230;0
WireConnection;231;1;229;0
WireConnection;145;38;23;0
WireConnection;161;38;157;0
WireConnection;232;0;228;0
WireConnection;216;38;213;0
WireConnection;233;0;231;0
WireConnection;233;1;232;0
WireConnection;225;0;216;0
WireConnection;225;1;223;0
WireConnection;225;2;224;0
WireConnection;225;3;222;0
WireConnection;184;0;188;0
WireConnection;184;1;187;0
WireConnection;184;2;186;0
WireConnection;184;3;185;0
WireConnection;146;0;145;0
WireConnection;146;1;149;0
WireConnection;146;2;151;0
WireConnection;146;3;153;0
WireConnection;164;0;163;0
WireConnection;164;1;160;0
WireConnection;164;2;161;0
WireConnection;164;3;162;0
WireConnection;295;38;291;0
WireConnection;297;38;293;0
WireConnection;296;38;292;0
WireConnection;276;38;270;0
WireConnection;273;38;262;0
WireConnection;289;38;290;0
WireConnection;277;38;266;0
WireConnection;278;38;264;0
WireConnection;274;38;263;0
WireConnection;275;38;268;0
WireConnection;285;38;288;0
WireConnection;234;0;233;0
WireConnection;155;0;146;0
WireConnection;155;1;164;0
WireConnection;155;2;184;0
WireConnection;155;3;225;0
WireConnection;298;38;294;0
WireConnection;272;38;269;0
WireConnection;286;38;287;0
WireConnection;271;38;265;0
WireConnection;279;38;267;0
WireConnection;299;0;295;0
WireConnection;299;1;296;0
WireConnection;299;2;297;0
WireConnection;299;3;298;0
WireConnection;280;0;272;0
WireConnection;280;1;289;0
WireConnection;280;2;285;0
WireConnection;280;3;286;0
WireConnection;281;0;275;0
WireConnection;281;1;271;0
WireConnection;281;2;274;0
WireConnection;281;3;277;0
WireConnection;282;0;278;0
WireConnection;282;1;273;0
WireConnection;282;2;279;0
WireConnection;282;3;276;0
WireConnection;256;0;155;0
WireConnection;237;0;234;0
WireConnection;237;1;238;0
WireConnection;237;2;239;0
WireConnection;258;0;237;0
WireConnection;258;1;245;0
WireConnection;259;0;256;0
WireConnection;283;0;281;0
WireConnection;283;1;282;0
WireConnection;283;2;280;0
WireConnection;283;3;299;0
WireConnection;301;0;283;0
WireConnection;260;0;258;0
WireConnection;260;1;259;0
WireConnection;261;0;260;0
WireConnection;306;0;283;0
WireConnection;303;0;302;0
WireConnection;303;1;301;0
WireConnection;304;0;155;0
WireConnection;304;1;303;0
WireConnection;284;0;283;0
WireConnection;236;0;261;0
WireConnection;236;1;155;0
WireConnection;307;0;283;0
WireConnection;305;0;306;0
WireConnection;0;0;236;0
WireConnection;0;2;304;0
WireConnection;0;3;307;0
WireConnection;0;4;305;0
ASEEND*/
//CHKSM=EBE0A7C4514E89EE3F9ECC5A27C94EA4A7A1AD70