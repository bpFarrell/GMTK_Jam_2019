﻿Shader "Custom/Slime"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Jiggle("_Jiggle",Float) = 0
		_Death("Death",Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        CGPROGRAM
		#include "Assets/ShaderBox.cginc"
        // Physically based Standard lighting model, and enable shadows on all light types
#pragma surface surf Lambert vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
			float3 localPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
		float _Death;
		float _Jiggle;
        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.localPos = v.vertex.xyz;
			v.vertex.y += sin(v.vertex.z + _Time*100)*_Jiggle;
		}
        void surf (Input IN, inout SurfaceOutput  o)
        {
            // Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			float n = noise(IN.localPos * 10);
			float decay = n - _Death;
			if (decay<0)
				discard;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            //o.Metallic = _Metallic;
            //o.Smoothness = _Glossiness;
            o.Alpha = c.a;
			o.Emission = (1-decay)*2*_Death;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
