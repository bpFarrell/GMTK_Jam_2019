Shader "Custom/GhostAnim"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_Death("Death",Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
		#include "Assets/ShaderBox.cginc"
        // Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard vertex:vert 

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

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)
		void vert(inout appdata_full v, out Input o) {
			//float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			//float m = sin(_Time.z * 2 + worldPos.x + worldPos.y + worldPos.z);
			float t = v.texcoord.x * saturate(1-v.texcoord.y);
			t = sin(t * 3.141529*8 + _Time.x*100)*0.003* saturate(1 - v.texcoord.y);
			v.vertex.z += t;
			o.uv_MainTex = v.texcoord;
			o.localPos = v.vertex.xyz;
			//v.vertex.xz += m * worldPos.y * 0.001;
		}
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			float n = abs(2*noise(IN.localPos * 300)-1);
			float decay = n - _Death;
			if (decay < 0)
				discard;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
