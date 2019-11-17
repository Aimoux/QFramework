Shader "HeroShader/Characters/UIShowCharacterRReflect_NoShadow" 
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_MaskTex_RGB("MaskTex_RGB", 2D) = "white" {}
		_Ramp2DMap("Ramp2D", 2D) = "white" {}
		_MatcapTex("Matcap", 2D) = "white" {}
		_NormalTex("Normal Map", 2D) = "bump" {}
		_FlowTex("Flow Map", 2D) = "white" {}
		_ReflectTex("Reflection Map", CUBE) = "white" {}
		_SpecColor("Spec Color", Color) = (0,0,0,0)
		_SpecPower("Spec Power", Range(1,128)) = 15
		_SpecFactor("Spec Factor", Float) = 1
		_ShadowColor("Shadow Color", Color) = (0,0,0,0)
		_ScrollUVX("UVXSpeed", Float) = 1
		_Scroll2Y("UVYSpeed", Float) = 0
		_FlowColor("Flow Color", Color) = (1,1,1,1)
		_MaskFactor("Mask Factor", Float) = 2
		_ReflectColor("Reflect Color", Color) = (1,1,1,1)
		_ReflectPower("Reflect Power", Range(0.1,5)) = 1
		_ReflectionFactor("Reflection Factor", Float) = 2
		_AmbientYOffset("AmbientYOffset", Float) = 0.8
		_AmbientColor("AmbientColor", Color) = (0.5,0.5,0.5,1)
		_Brightness("Brightness", Float) = 1
		_InterpolationN("SmoothReflection",Range(0,1.0)) = 0
		_ShadowHeight("ShadowHeight", Range(0,1)) = 0.01
	}
	SubShader
	{
		Tags{ "QUEUE" = "Geometry"  "IgnoreProjector" = "True"  "ForceNoShadowCasting" = "True"  "RenderType" = "Opaque" }
		UsePass"HeroShader/Characters/UIShowCharacterRReflect/UISHOWCHARACTERPASS0"
		UsePass"HeroShader/Characters/UIShowCharacterRReflect/UISHOWCHARACTERPASS1"
	}
}