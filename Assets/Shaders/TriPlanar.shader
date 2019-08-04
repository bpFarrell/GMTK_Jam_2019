Shader "Custom/TriPlanar"
{
	Properties
	{
		_Color("", Color) = (1, 1, 1, 1)
		_MainTex("", 2D) = "white" {}

		_Glossiness("", Range(0, 1)) = 0.5
		_Metallic("", Range(0, 1)) = 0

		_BumpScale("", Float) = 1
		_BumpMap("", 2D) = "bump" {}

		_OcclusionStrength("", Range(0, 1)) = 1
		_OcclusionMap("", 2D) = "white" {}

		_MapScale("", Float) = 1
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }

			CGPROGRAM

			#pragma surface surf Standard vertex:vert fullforwardshadows addshadow

			#pragma target 3.0

			half4 _Color;
			sampler2D _MainTex;

			half _Glossiness;
			half _Metallic;

			half _BumpScale;
			sampler2D _BumpMap;


			half _MapScale;

			struct Input
			{
				float3 worldPos;
				float3 worldNormal;

				INTERNAL_DATA
			};

			void vert(inout appdata_full v, out Input data)
			{
				UNITY_INITIALIZE_OUTPUT(Input, data);
			}

			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				IN.worldPos += 500;
				float3 worldNormal = WorldNormalVector(IN, o.Normal);
				float3 bf = normalize(abs(worldNormal));
				
				bf /= dot(bf, (float3)1);

				//Texture Space
				float2 uvx = IN.worldPos.zy * _MapScale;
				float2 uvy = IN.worldPos.zx * _MapScale;
				float2 uvz = IN.worldPos.xy * _MapScale;

				// Base color
				half4 sx = tex2D(_MainTex, uvx) * bf.x;
				half4 sy = tex2D(_MainTex, uvy) * bf.y;
				half4 sz = tex2D(_MainTex, uvz) * bf.z;
				half4 color = (sx + sy + sz)* _Color;

				o.Albedo = color.rgb;
				o.Alpha = color.a;
				// Normal map
				half4 nx = tex2D(_BumpMap, uvx) * bf.x;
				half4 ny = tex2D(_BumpMap, uvy) * bf.y;
				half4 nz = tex2D(_BumpMap, uvz) * bf.z;
				o.Normal = UnpackScaleNormal(nx + ny + nz, _BumpScale);

				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
			}
			ENDCG
		}
}