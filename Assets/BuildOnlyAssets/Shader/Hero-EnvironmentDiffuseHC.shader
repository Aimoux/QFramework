// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "HeroShader/Environment/DiffuseHC" {
	Properties{
			// a 美术用的参数
			// a.1 贴图和tint
			_Color("Color", Color) = (1,1,1,1)
			_MainTex("MainTex", 2D) = "white" {}
			// a.2 模拟环境光
			_BackColor("BackColor", Color) = (0.4980392,0.4705883,0.427451,1)
			_FrontColor("FrontColor", Color) = (0.8718101,0.8916488,0.9191176,1)
			_AmbientLight("AmbientLight", Color) = (0.5,0.5,0.5,0.5)
			_AmbientPower("AmbientPower", Range(0, 1)) = 0.1
				// a.3 模拟顶光
			_TopLight("TopLight", Range(0, 1.5)) = 0.5
			// a.4 模拟阴影
			_Shadowlevel("Shadowlevel", Range(0, 3)) = 0
			_ShadowColor("ShadowColor", Color) = (0,0,0,1)

			// b. 程序用的参数
			_FlagColor("Flag Color", Color) = (0.5,0.5,0.5,1)  // 阵营颜色
			[HideInInspector] _EmitColor("EmitColor", Color) = (0.5,0.5,0.5,0) // 发光颜色
			[HideInInspector] _RimColor("RimColor", Color) = (0.5,0.5,0.5,0)   // Rim颜色
	}

    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            //Name "FORWARD"	
   /*         Tags {
                "LightMode"="ForwardBase"
            }*/
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0

			// a.1 贴图和tint
			uniform fixed4 _Color;
			uniform sampler2D _MainTex;
			// a.2 模拟环境光
			uniform fixed4 _BackColor;
			uniform fixed4 _FrontColor;
			uniform fixed4 _AmbientLight; // rgb用来控制环境光的方位
			uniform fixed _AmbientPower;
			// a.3 模拟顶光
			uniform fixed _TopLight; // 其实是漫反射的强度控制
			// a.4 模拟阴影
			uniform fixed _Shadowlevel;
			uniform fixed4 _ShadowColor;

			// 程序控制参数
			uniform fixed4 _FlagColor;
			uniform fixed4 _EmitColor;
			uniform fixed4 _RimColor;

            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
				float3 diffShadow : TEXCOORD1;
				float3 rimEmit : TEXCOORD2;
				//		UNITY_FOG_COORDS(3)  // fog 是不是也要去掉？
            };

            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord0;

				float3 normalDir = UnityObjectToWorldNormal(v.normal);
				float4 worldPos = mul(unity_ObjectToWorld, v.vertex); 
				float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - worldPos.xyz);


				float sl = saturate(worldPos.g + 1 - _Shadowlevel * 5);
				float NdotV = max(0, dot(normalDir, viewDir));
				float NdotA = max(0, dot(normalDir, _AmbientLight.rgb));

				// <程序> emit + rim 
				fixed3 emitRim = _EmitColor.rgb * _EmitColor.a + saturate(0.8 - NdotV) * _RimColor.rgb * _RimColor.a;  
				fixed3 shadow = lerp(_ShadowColor.rgb, 1, sl); // 模拟阴影

				// <美术> 漫反射
				o.diffShadow = _Color.rgb*(1 + NdotV * _TopLight) * shadow;  // 模拟光照, 顶光+漫反射, 其中NdotV默认为灯光在摄像机那边(默认的漫反射是NdotL); TopLight调节光的强度
				
				// <美术> 模拟环境光
				fixed3 envLight = lerp(_FrontColor.rgb, _BackColor.rgb, NdotA) * _AmbientPower; // 环境光模拟， 需要乘以阴影
				// <美术> 模拟阴影
				o.rimEmit = envLight * shadow + emitRim;
				
				return o;
            }
            float4 frag(VertexOutput i) : COLOR {
				fixed4 c = tex2D(_MainTex, i.uv);
				c.rgb = lerp(c.rgb * i.diffShadow, _FlagColor.rgb, c.a) + i.rimEmit;
				return c;
            }
            ENDCG
        }
        Pass {  //TODO 需要进一步优化
            Name "ShadowCaster"
            Tags {
				"QUEUE" = "AlphaTest+1" "RenderType" = "Opaque"
            }
			ZWrite Off
            Offset -5, 0
            
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
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma exclude_renderers d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
			
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };

			struct v2f
			{
				half4 Pos : SV_POSITION;
				fixed4 Color : COLOR0;
				float atten : TEXCOORD0;
			};

			v2f vert (appdata_base v) { 
				fixed _Power = 1.5;
				fixed _Instensity = 0.08;
				fixed _Height = 0.0;
				fixed4 _SunDir = fixed4(0.1, 0, 0.1, 1); 

				v2f o;
				float4 objPos = mul(unity_ObjectToWorld, float4(0.0, 0.0, 0.0, 1.0)) + _SunDir;
				float4 positionInWorldSpace = mul(unity_ObjectToWorld, v.vertex);
				float4 attPos = positionInWorldSpace - objPos;

				float posY = positionInWorldSpace.y;
				positionInWorldSpace.x -= positionInWorldSpace.y * _SunDir.x;
				positionInWorldSpace.z -= positionInWorldSpace.y * _SunDir.z;
				positionInWorldSpace.y = _Height;//force the vertex's world space Y = 0 (on the ground)
				o.Pos = mul(UNITY_MATRIX_VP, positionInWorldSpace); //complete to MVP matrix transform (we already change from local->world before, so this line only do VP)
				o.Color = 0;
				o.Color.a = 1.0;

				attPos.x -= attPos.y * _SunDir.x;
				attPos.z -= attPos.y * _SunDir.z;
				attPos.y = 0;
				o.atten = (length(attPos.xz) - 0.2) * _Instensity;
				o.atten = clamp(pow(o.atten, _Power), 0, 1);
				if (posY < -0.02)
					o.atten = 1;
				return o;
            }
			 
            float4 frag(v2f i) : COLOR {
				i.Color.a = (1 - i.atten) * 0.5;
				clip(1 - i.atten - 0.01);
				return 	i.Color;
            }
            ENDCG
        }
    }
    FallBack "Unlit/Texture"
    CustomEditor "ShaderForgeMaterialInspector"
}
