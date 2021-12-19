Shader "ChuckLee/ARShadow"
{
	Properties
	{
		_ShadowColor("Shadow Color", Color) = (0.1, 0.1, 0.1, 0.53)
	}
		SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Geometry+1" }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
	{
		Tags{ "LightMode" = "ForwardBase" }

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_fwdbase

#include "UnityCG.cginc"
#include "AutoLight.cginc" 

		struct appdata
	{
		float4 vertex : POSITION;
	};

	struct v2f
	{
		float4 pos : SV_POSITION;
		SHADOW_COORDS(2)
	};

	fixed4 _ShadowColor;

	v2f vert(appdata v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		TRANSFER_SHADOW(o);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		fixed atten = SHADOW_ATTENUATION(i);
	return fixed4(_ShadowColor.rgb,saturate(1 - atten)*_ShadowColor.a);
	}
		ENDCG
	}
	}
		FallBack "Diffuse"
}