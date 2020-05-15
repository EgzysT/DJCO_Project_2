Shader "Custom/SwitchShader"
{
    Properties
    {
        _Color ("Primary Color", Color) = (1,1,1,1)
        _MainTex ("Primary (RGB)", 2D) = "white" {}
        _Color2("Secondary Color", Color) = (1,1,1,1)
        _SecondTex("Secondary (RGB)", 2D) = "white" {}
        _NoiseTex("Dissolve Noise", 2D) = "white"{}
        _NScale("Noise Scale", Range(0, 10)) = 1
        _DisAmount("Noise Texture Opacity", Range(0.01, 1)) = 0.01
        _DisLineWidth("Line Width", Range(0, 2)) = 0
        _DisLineColor("Line Tint", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        float3 _Position; // from script
        float _Radius; // from script

        sampler2D _MainTex, _SecondTex;
        float4 _Color, _Color2;
        sampler2D _NoiseTex;
        float _DisAmount, _NScale;
        float _DisLineWidth;
        float4 _DisLineColor;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;    // built in value to use the world space position
            float3 worldNormal; // built in value for world normal
        };

        half _Glossiness;
        half _Metallic;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            // Albedo comes from a texture tinted by color
            half4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            half4 c2 = tex2D(_SecondTex, IN.uv_MainTex) * _Color2;

            // triplanar noise
            float3 blendNormal = saturate(pow(IN.worldNormal * 1.4, 4));
            half4 nSide1 = tex2D(_NoiseTex, (IN.worldPos.xy + _Time.x) * _NScale);
            half4 nSide2 = tex2D(_NoiseTex, (IN.worldPos.xz + _Time.x) * _NScale);
            half4 nTop = tex2D(_NoiseTex, (IN.worldPos.yz + _Time.x) * _NScale);

            float3 noisetexture = nSide1;
            noisetexture = lerp(noisetexture, nTop, blendNormal.x);
            noisetexture = lerp(noisetexture, nSide2, blendNormal.y);

            // distance influencer position to world position
            float3 dis = distance(_Position, IN.worldPos);
            float3 sphere = 1 - saturate(dis / _Radius);

            float3 sphereNoise = noisetexture.r * sphere;

            float3 DissolveLine = step(sphereNoise - _DisLineWidth, _DisAmount) * step(_DisAmount, sphereNoise); // line between two textures
            DissolveLine *= _DisLineColor; // color the line

            float3 primaryTex = (step(sphereNoise - _DisLineWidth, _DisAmount) * c.rgb);
            float3 secondaryTex = (step(_DisAmount, sphereNoise) * c2.rgb);
            float3 resultTex = primaryTex + secondaryTex + DissolveLine;
            o.Albedo = resultTex;

            o.Emission = DissolveLine;

            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
