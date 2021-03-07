// Upgrade NOTE: upgraded instancing buffer 'Foam_Shader' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Foam_Shader"
{
	Properties
	{
		_FoamTint("Foam Tint", Color) = (0.4745891,0.5197171,0.9779412,0)
		_Cutoff("Cutoff", Range( 0 , 1)) = 0
		_BorderWidth("Border Width", Range( 0 , 5)) = 0
		_NoiseA("Noise A", 2D) = "white" {}
		_TileNoiseA("Tile Noise A", Vector) = (2,1.5,0,0)
		_NoiseBSpeed("Noise B Speed", Float) = 0.023
		_NoiseAMaskClip("Noise A Mask Clip", Float) = 0
		_NoiseB("Noise B", 2D) = "white" {}
		_TileNoiseB("Tile Noise B", Vector) = (1,1,0,0)
		_NoiseASpeed("Noise A Speed", Float) = 0.023
		_NoiseBMaskClip("Noise B Mask Clip", Float) = 1
		[HideInInspector]_TextureSample0("Texture Sample 0", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Back
		Blend One One
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		struct Input
		{
			float2 uv_texcoord;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float4 _FoamTint;
		uniform float _BorderWidth;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform sampler2D _NoiseB;
		uniform float _NoiseBSpeed;
		uniform sampler2D _NoiseA;
		uniform float _NoiseASpeed;
		uniform float _Cutoff;

		UNITY_INSTANCING_BUFFER_START(Foam_Shader)
			UNITY_DEFINE_INSTANCED_PROP(float2, _TileNoiseA)
#define _TileNoiseA_arr Foam_Shader
			UNITY_DEFINE_INSTANCED_PROP(float2, _TileNoiseB)
#define _TileNoiseB_arr Foam_Shader
			UNITY_DEFINE_INSTANCED_PROP(float, _NoiseAMaskClip)
#define _NoiseAMaskClip_arr Foam_Shader
			UNITY_DEFINE_INSTANCED_PROP(float, _NoiseBMaskClip)
#define _NoiseBMaskClip_arr Foam_Shader
		UNITY_INSTANCING_BUFFER_END(Foam_Shader)

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			float clampResult67 = clamp( i.uv_texcoord.y , 0.0 , 1.0 );
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float2 appendResult55 = (float2(0.0 , _NoiseBSpeed));
			float2 _TileNoiseB_Instance = UNITY_ACCESS_INSTANCED_PROP(_TileNoiseB_arr, _TileNoiseB);
			float2 uv_TexCoord19 = i.uv_texcoord * _TileNoiseB_Instance;
			float2 panner20 = ( _Time.y * appendResult55 + uv_TexCoord19);
			float _NoiseBMaskClip_Instance = UNITY_ACCESS_INSTANCED_PROP(_NoiseBMaskClip_arr, _NoiseBMaskClip);
			float2 appendResult52 = (float2(0.0 , _NoiseASpeed));
			float2 _TileNoiseA_Instance = UNITY_ACCESS_INSTANCED_PROP(_TileNoiseA_arr, _TileNoiseA);
			float2 uv_TexCoord6 = i.uv_texcoord * _TileNoiseA_Instance;
			float2 panner8 = ( _Time.y * appendResult52 + uv_TexCoord6);
			float _NoiseAMaskClip_Instance = UNITY_ACCESS_INSTANCED_PROP(_NoiseAMaskClip_arr, _NoiseAMaskClip);
			c.rgb = 0;
			c.a = 1;
			clip( ( pow( clampResult67 , _BorderWidth ) + ( tex2D( _TextureSample0, uv_TextureSample0 ) * ( ( tex2D( _NoiseB, panner20 ) * _NoiseBMaskClip_Instance ) * ( tex2D( _NoiseA, panner8 ) * _NoiseAMaskClip_Instance ) ) ) ).r - _Cutoff );
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Emission = _FoamTint.rgb;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				UnityGI gi;
				UNITY_INITIALIZE_OUTPUT( UnityGI, gi );
				o.Alpha = LightingStandardCustomLighting( o, worldViewDir, gi ).a;
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
1927;29;1906;1004;-417.4417;391.407;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;54;-1148.321,125.434;Float;False;Property;_NoiseBSpeed;Noise B Speed;5;0;Create;True;0;0;False;0;0.023;0.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-1059.426,-223.5174;Float;False;Property;_NoiseASpeed;Noise A Speed;9;0;Create;True;0;0;False;0;0.023;0.08;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;10;-1165.255,-465.9363;Float;False;InstancedProperty;_TileNoiseA;Tile Noise A;4;0;Create;True;0;0;False;0;2,1.5;2,0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;16;-1216.584,-42.88404;Float;False;InstancedProperty;_TileNoiseB;Tile Noise B;8;0;Create;True;0;0;False;0;1,1;6,0.62;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DynamicAppendNode;52;-828.9275,-243.9688;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;55;-962.0226,119.2826;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;12;-864.5848,-120.8129;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-916.5593,-393.715;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;19;-1019.889,-33.06255;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;18;-967.9141,239.8396;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;20;-741.0042,22.71298;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;8;-637.6748,-337.9394;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-306.543,216.5502;Float;False;InstancedProperty;_NoiseBMaskClip;Noise B Mask Clip;10;0;Create;True;0;0;False;0;1;4.45;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-359.7519,-101.4734;Float;False;InstancedProperty;_NoiseAMaskClip;Noise A Mask Clip;6;0;Create;True;0;0;False;0;0;0.46;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;24;-489.4012,3.099426;Float;True;Property;_NoiseB;Noise B;7;0;Create;True;0;0;False;0;None;8704aeacd8373c34ca93f6ee56ef3fab;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-436.7062,-310.956;Float;True;Property;_NoiseA;Noise A;3;0;Create;True;0;0;False;0;None;8704aeacd8373c34ca93f6ee56ef3fab;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;61;663.5258,-7.420288;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-66.96534,132.3245;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-101.4398,-84.79488;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;58;141.526,-236.1319;Float;True;Property;_TextureSample0;Texture Sample 0;11;1;[HideInInspector];Create;True;0;0;False;0;None;4130b9429150bec4ebf7396b605c3bea;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;173.912,89.12491;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;64;916.5258,-92.42029;Float;False;Property;_BorderWidth;Border Width;2;0;Create;True;0;0;False;0;0;2.09;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;67;982.4417,55.59296;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;65;1226.395,52.45642;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;516.2351,142.6005;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;44;699.8385,-346.294;Float;False;Property;_FoamTint;Foam Tint;0;0;Create;True;0;0;False;0;0.4745891,0.5197171,0.9779412,0;0.5896226,0.629905,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;60;563.5472,-69.15054;Float;False;Property;_Cutoff;Cutoff;1;0;Create;True;0;0;False;0;0;0.523;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;62;1409.526,189.5797;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;51;1828.836,-137.8775;Float;False;True;2;Float;ASEMaterialInspector;0;0;CustomLighting;Foam_Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.523;True;True;0;True;Custom;;AlphaTest;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;4;1;False;-1;1;False;-1;0;1;False;-1;1;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;True;60;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;52;1;53;0
WireConnection;55;1;54;0
WireConnection;6;0;10;0
WireConnection;19;0;16;0
WireConnection;20;0;19;0
WireConnection;20;2;55;0
WireConnection;20;1;18;0
WireConnection;8;0;6;0
WireConnection;8;2;52;0
WireConnection;8;1;12;0
WireConnection;24;1;20;0
WireConnection;2;1;8;0
WireConnection;22;0;24;0
WireConnection;22;1;23;0
WireConnection;4;0;2;0
WireConnection;4;1;5;0
WireConnection;21;0;22;0
WireConnection;21;1;4;0
WireConnection;67;0;61;2
WireConnection;65;0;67;0
WireConnection;65;1;64;0
WireConnection;57;0;58;0
WireConnection;57;1;21;0
WireConnection;62;0;65;0
WireConnection;62;1;57;0
WireConnection;51;2;44;0
WireConnection;51;10;62;0
ASEEND*/
//CHKSM=0494B049BAD356FDFDB35159F0E85AE8D1B2333E