// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "HeroShader/Characters/UIShowCharacterRReflect" 
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
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True"  "ForceNoShadowCasting" = "True" "RenderType" = "Depth_ShadowMap" }
		Pass
		{
			Name "UISHOWCHARACTERPASS0"
			Tags{ "LightMode" = "Always"}
			Cull Off
			Fog{ Mode Off }
			Lighting Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			//#include "AutoLight.cginc"
			#pragma exclude_renderers xbox360 xboxone ps3 ps4 psp2 
			#pragma multi_compile _BASIC _YAMBIENT_ON
			#pragma multi_compile _BASIC _SELFSHADOW_ON
			//#pragma target 3.0

			float _ScrollUVX; float _Scroll2Y;
			float4 _FlowTex_ST;
			float4 _MatcapLightXDir;
			float4x4 _ShadowWProjectionMatrix;
			float4 _FlowColor;
			float _MaskFactor;
			float4 _ShadowColor;
			float4 _MainTex_ST;
			float _SpecPower;
			float _SpecFactor;
			float4 _SpecColor;
			float4 _ReflectColor;
			float _ReflectPower;
			float _ReflectionFactor;
			float _AmbientYOffset;
			float4 _AmbientColor;
			float _Brightness;
			float _InterpolationN;
			uniform sampler2D _MainTex;
			uniform sampler2D _MaskTex_RGB;
			uniform sampler2D _MatcapTex;
			uniform sampler2D _NormalTex;
			uniform samplerCUBE _ReflectTex;
			uniform sampler2D _Ramp2DMap;
			uniform sampler2D _FlowTex;
			uniform sampler2D _ShadowMap;

			struct v2f
			{
				float4 pos:POSITION;
				float4 uv0:TEXCOORD0;
				float3 uv1:TEXCOORD1;
				float3 tangentViewDir:TEXCOORD2;
				float3 tangentLightDir:TEXCOORD3;
				float4 posWorld:TEXCOORD4;
				float3 normalWorld:TEXCOORD5;
				float3 tangentWorld:TEXCOORD6;
				float3 binormalWorld:TEXCOORD7;
				float3 worldAngle:TEXCOORD8;
			};

			v2f vert(appdata_tan v)
			{
				v2f o;

				float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
				float3 Normal = normalize(v.normal);
				float4 node_8207;
				node_8207.xy = ((v.texcoord.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
				float2 node_2893 = node_8207.xy;
				float2 node_5892 = float2(_ScrollUVX, _Scroll2Y);
				node_2893 = (((v.texcoord.xy * _FlowTex_ST.xy) + _FlowTex_ST.zw) + frac(node_5892 * _Time.x));
				node_8207.zw = node_2893;

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
				o.posWorld = worldPos;

				float3 viewDir = -ObjSpaceViewDir(v.vertex);
				float3 worldRefl = mul((float3x3)unity_ObjectToWorld, viewDir);
				o.worldAngle = float3(worldRefl.x, worldRefl.y, worldRefl.z);

				o.normalWorld = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);
				o.tangentWorld = normalize(mul(unity_ObjectToWorld, v.tangent).xyz);
				o.binormalWorld = normalize(cross(o.normalWorld, o.tangentWorld) * v.tangent.w);

				//TRANSFER_VERTEX_TO_FRAGMENT(o)
				return o;
			}

			float4 frag(v2f i) :Color
			{
				float3 node_9218 = normalize(i.uv1);
				float2 tmp_Matcap_UV = ((node_9218.xy * 0.5) + 0.5);
				float4 _MainTex_Var = tex2D(_MainTex, i.uv0.xy);
				float threshold = _MainTex_Var.a-0.9;
				if (threshold < 0)
				{
					clip(threshold);
					return _MainTex_Var;
				}
				float3 normal;
				#if defined(UNITY_NO_DXT5nm)
					normal = normalize(((tex2D(_NormalTex, i.uv0.xy).xyz * 2.0) - 1.0));
				#else
					normal = UnpackNormalDXT5nm(tex2D(_NormalTex, i.uv0.xy));
				#endif
				float3 node_6402 = ((_MainTex_Var.xyz + 0.15) * (tex2D(_MatcapTex, tmp_Matcap_UV) * 1.2).xyz);
				float3 _FlowTex_Var;
				_FlowTex_Var = tex2D(_FlowTex, i.uv0.zw).xyz;
				float _FlowVal = _FlowTex_Var.x;
				float4 _MaskTex_Var = tex2D(_MaskTex_RGB, i.uv0.xy);
				_MaskTex_Var.a = _FlowTex_Var.y;

				_FlowTex_Var = ((_FlowVal * (_MainTex_Var.xyz * _FlowColor)) * (_MaskTex_Var.y * _MaskFactor));
				float3 node_1776 = ((node_6402 + _MainTex_Var.xyz) + _FlowTex_Var);

				float3x3 local2World = float3x3(i.tangentWorld, i.binormalWorld, i.normalWorld);
				float3 normalDirW = normalize(mul(normal, local2World));
				float3 normalDir = normalize(lerp(normalDirW,i.normalWorld, _InterpolationN));
				float4 _MacRelTex_Var = texCUBE(_ReflectTex,reflect(i.worldAngle, normalDir));

				float3 node_9943 = ((node_1776 * pow((_MacRelTex_Var.xyz * _ReflectColor), float3(_ReflectPower, _ReflectPower, _ReflectPower))) * _ReflectionFactor);
				node_1776 = lerp(node_1776, node_9943, _MaskTex_Var.zzz);
				float gloss = _MaskTex_Var.x;

				float node_5719 = 1.0;
#ifdef _SELFSHADOW_ON
				float4 uv2 = mul(_ShadowWProjectionMatrix, i.posWorld);
				float3 node_1985 = (((uv2.xyz / uv2.w) * 0.5) + 0.5);
				float3 node_5276 = float3(node_1985.x, node_1985.y, (node_1985.z - 0.00025));
				float4 _ShadowMapTex_Var = tex2D(_ShadowMap, node_1985.xy);
				float node_8988 = clamp((2.0 - exp(((node_5276.z - dot(_ShadowMapTex_Var, float4(1.0, 0.00392157, 1.53787e-05, 6.03086e-08))) * 1024.0))), 0.0, 1.0);
				node_5719 = ((node_8988 * 0.5) + 0.5);
#endif

				float spec = max(0.0, dot(normal, normalize((i.tangentLightDir + i.tangentViewDir))));
				float diff_Vavle = ((dot(normal, i.tangentLightDir) * 0.5) + 0.5);
				float2 ramp_UV = float2(diff_Vavle, 0.5);
				float3 _RampTex_Var = tex2D(_Ramp2DMap, ramp_UV).xyz;
				float3 finalColor = ((_SpecColor * ((((pow(spec, _SpecPower) * gloss) * _SpecFactor) * node_5719)* 2.0)) + ((_RampTex_Var + float3(0.5, 0.5, 0.5)) * node_1776));
				finalColor = lerp((node_1776 * _ShadowColor.xyz), finalColor, float3(node_5719, node_5719, node_5719));
				
				float3 node_8073 = float3(1.0, 1.0, 1.0);
#ifdef _YAMBIENT_ON
				float worldPosOffY = (i.posWorld.y - (mul(unity_ObjectToWorld, float4(0.0, 0.0, 0.0, 1.0)).y - _AmbientYOffset));
				float hWorldNor = (normalize(normalDirW).y * 0.5);
				float3 node_2000 = (float3(worldPosOffY, worldPosOffY, worldPosOffY) + float3(hWorldNor, hWorldNor, hWorldNor));
				node_2000 = clamp(node_2000, float3(0.0, 0.0, 0.0), float3(1.0, 1.0, 1.0));
				node_8073 = lerp(_AmbientColor.xyz, float3(1.0, 1.0, 1.0), node_2000);
				finalColor = (finalColor * node_8073);
#endif
				float3 Albedo = (finalColor * _Brightness);
				Albedo = clamp(Albedo, 0.0, 1.0);
				float4 Color;
				Color.xyz = Albedo.xyz;
				Color.w = _MainTex_Var.w;
				return Color;
			}
			ENDCG
		}

		Pass
		{
			Name "UISHOWCHARACTERPASS1"
			Tags{"LightMode" = "Always" }
			ZWrite Off
			Stencil
			{
				Ref 1
				Comp Equal
			}
			Fog{ Mode Off }
			Lighting Off
			Stencil
			{
				Comp Equal
				Pass IncrWrap
			}
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask RGB

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform fixed _Height;
			float _Instensity;
			float _Power;

			// Structure from vertex shader to fragment shader
			struct v2f
			{
				half4 Pos : SV_POSITION;
				fixed4 Color : COLOR0;
			};

			//in vertex shader project the model onto y=0 ground according to directional light direction 
			v2f vert(appdata_base v)
			{
				v2f o;
				half4 _SunDir = float4(0.3, 0, 0.5, 0.0);
				half4 objPos = mul(unity_ObjectToWorld, float4(0.0, 0.0, 0.0, 1.0)) + _SunDir;
				half4 positionInWorldSpace = mul(unity_ObjectToWorld, v.vertex);
				half4 attPos = positionInWorldSpace - objPos;

				half posY = positionInWorldSpace.y;
				positionInWorldSpace.x -= positionInWorldSpace.y * _SunDir.x;
				positionInWorldSpace.z -= positionInWorldSpace.y * _SunDir.z;
				positionInWorldSpace.y = _Height;//force the vertex's world space Y = 0 (on the ground)
				o.Pos = mul(UNITY_MATRIX_VP, positionInWorldSpace); //complete to MVP matrix transform (we already change from local->world before, so this line only do VP)
				o.Color = 0;

				//attPos.x -= attPos.y * _SunDir.x;
				attPos.z -= attPos.y * _SunDir.z;
				//attPos.y = 0;
				//attPos.x = min(0, attPos.x);
				attPos.z = min(0, attPos.z);

				fixed atten = 0;
				if (posY >= 0)
				{
					atten = length(attPos.z);
					atten = clamp(1 - clamp((atten - 0.15) / (3 - 0.15), 0, 1) * 2.5, 0.05, 0.7);
				}
				o.Color.a = atten;
				return o;
			}
			//simple outputing blending color from color calculated in vertex shader (above)
			fixed4 frag(v2f i) : COLOR
			{
				return 	i.Color;
			}
			ENDCG
		}
	}
}