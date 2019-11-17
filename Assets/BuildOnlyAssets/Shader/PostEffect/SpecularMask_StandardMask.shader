// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "HeroShader/Effect/PostEffect/SpecularMask_StandardMask" 
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_MaskTex_RGB("MaskTex_RGB", 2D) = "white" {}
		_FlowTex("_FlowTex", 2D) = "white" {}
		_MatcapTex("Matcap", 2D) = "white" {}
		_NormalTex("Normal Map", 2D) = "bump" {}
		_SpecColor("Spec Color", Color) = (0,0,0,0)
		_SpecPower("Spec Power", Range(1,128)) = 15
		_SpecFactor("Spec Factor", Float) = 1
		_Brightness("Brightness", Float) = 1
		_GlowColor("GlowColor", Color) = (1,1,1,1)
		_MaskEmissionIntesity("_MaskEmissionIntesity", Range(0, 10)) = 1
	}
	SubShader
	{
		Tags{ "Queue" = "Geometry"  "RenderType" = "Opaque" "IgnoreProjector" = "True" }
		Pass
		{
			Tags{ "LightMode" = "Always" }
			Fog{ Mode Off }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#pragma exclude_renderers xbox360 xboxone ps3 ps4 psp2
			#pragma multi_compile _BASIC _SELFSHADOW_ON
			//#pragma target 3.0

			float4 _MatcapLightXDir;
			float4x4 _ShadowWProjectionMatrix;
			float4 _MainTex_ST;
			float _SpecPower;
			float _SpecFactor;
			float4 _SpecColor;
			float4 _GlowColor;
			float _Brightness;
			float _MaskEmissionIntesity;
			uniform sampler2D _MainTex;
			uniform sampler2D _MatcapTex;
			uniform sampler2D _MaskTex_RGB;
			uniform sampler2D _FlowTex;
			uniform sampler2D _NormalTex;
			uniform sampler2D _ShadowMap;
			struct v2f
			{
				float4 pos:POSITION;
				float2 uv0:TEXCOORD0;
				float3 uv1:TEXCOORD1;
				float3 tangentViewDir:TEXCOORD2;
				float3 tangentLightDir:TEXCOORD3;
#ifdef _SELFSHADOW_ON
				float4 shadowUV:TEXCOORD9;
#endif
			};

			v2f vert(appdata_tan v)
			{
				v2f o;
				float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
				float3 Normal = normalize(v.normal);
				float4 node_8207;
				node_8207.xy = ((v.texcoord.xy * _MainTex_ST.xy) + _MainTex_ST.zw);

				TANGENT_SPACE_ROTATION;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv0 = node_8207;

				float4 node_606;
				node_606.w = 0.0;
				node_606.xyz = Normal;

				o.uv1 = mul(UNITY_MATRIX_MV, node_606).xyz;

				float4 viewDirection;
				viewDirection.w = 0.0;
				viewDirection.xyz = (_WorldSpaceCameraPos - worldPos.xyz);

				o.tangentViewDir = normalize(mul(rotation, mul(unity_WorldToObject, viewDirection).xyz));
				float4 node_4091;
				node_4091.w = 0.0;
				_MatcapLightXDir = float4(-0.078360740f, -0.330885300f, -0.940412100f, 0.5f);
				node_4091.xyz = -(_MatcapLightXDir.xyz);
				o.tangentLightDir = normalize(mul(rotation, mul(unity_WorldToObject, node_4091).xyz));;

				float3 viewDir = -ObjSpaceViewDir(v.vertex);
				float3 worldRefl = mul((float3x3)unity_ObjectToWorld, viewDir);
#ifdef _SELFSHADOW_ON
				o.shadowUV = mul(_ShadowWProjectionMatrix, worldPos);
#endif
				return o;
			}

			float4 frag(v2f i) :Color
			{
				float3 node_9218 = normalize(i.uv1);
				float2 tmp_Matcap_UV = ((node_9218.xy * 0.5) + 0.5);
				float3 normal;
#if defined(UNITY_NO_DXT5nm)
				normal = normalize(((tex2D(_NormalTex, i.uv0.xy).xyz * 2.0) - 1.0));
#else
				normal = UnpackNormalDXT5nm(tex2D(_NormalTex, i.uv0.xy));
#endif
				float4 _MainTex_Var = tex2D(_MainTex, i.uv0.xy);
				float3 node_6402 = ((_MainTex_Var.xyz + 0.15) * (tex2D(_MatcapTex, tmp_Matcap_UV) * 1.2).xyz);
				float3 node_1776 = node_6402 + _MainTex_Var.xyz;
				float4 _MaskTex_Var = tex2D(_MaskTex_RGB, i.uv0.xy);
				_MaskTex_Var.a = tex2D(_FlowTex, i.uv0.xy).g;
				float gloss = _MaskTex_Var.x;

				float node_5719 = 1.0;
#ifdef _SELFSHADOW_ON
				float3 node_1985 = (((i.shadowUV.xyz / i.shadowUV.w) * 0.5) + 0.5);
				float3 node_5276 = float3(node_1985.x, node_1985.y, (node_1985.z - 0.00025));
				float4 _ShadowMapTex_Var = tex2D(_ShadowMap, node_1985.xy);
				float node_8988 = clamp((2.0 - exp(((node_5276.z - dot(_ShadowMapTex_Var, float4(1.0, 0.00392157, 1.53787e-05, 6.03086e-08))) * 1024.0))), 0.0, 1.0);
				node_5719 = ((node_8988 * 0.5) + 0.5);
#endif

				float spec = max(0.0, dot(normal, normalize((i.tangentLightDir + i.tangentViewDir))));
				float3 finalColor = _SpecColor * ((((pow(spec, _SpecPower) * gloss) * _SpecFactor) * node_5719)* 2.0) + node_1776*gloss;

				float3 Albedo = (finalColor * _Brightness);
				Albedo = clamp(Albedo, 0.0, 1.0);

				float3 maskColor = _MainTex_Var.xyz*_MaskEmissionIntesity;
				maskColor *= _MaskTex_Var.w;
				maskColor *= _GlowColor.xyz;
				float4 Color;
				Color.xyz = Albedo.xyz + maskColor;
				Color.w = _MainTex_Var.w;
				return Color;
			}
			ENDCG
		}
	}
}