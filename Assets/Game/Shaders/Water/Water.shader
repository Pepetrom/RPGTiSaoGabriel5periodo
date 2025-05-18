Shader "Custom/WaterUnlitURP_TwoFoams_Transparent"
{
    Properties
    {
        // texturas
        _WaterTex("Water Map",2D) = "white" {}
        _FoamTexA("Foam Map A",2D) = "white" {}
        _FoamTexB("Foam Map B",2D) = "white" {}

        // direção da água
        _FlowDir ("Flow Direction", Vector) = (1,0,0,0)
        _FlowSpeed ("Flow Speed", Float)  = 0.1

        // bink da agua
        _FoamBlinkSpeed ("Foam Blink Speed", Float)  = 1.0

        // cores
        _WaterColor ("Water Color",Color) = (0.2,0.6,0.8,0.5)
        _FoamColor ("Foam Color",Color) = (1,1,1,0.7)
        _FresnelColor ("Fresnel Color",Color) = (0.0,0.2,0.3,0.0)

        // fresnel
        _FresnelPower ("Fresnel Power",Range(0,10)) = 2
        _FresnelIntensity("Fresnel Intensity",Range(0,1)) = 0.5

        // alpha
        _Transparency ("Transparency",Range(0,1)) = 1.0
    }
    SubShader
    {
        Tags {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }
        LOD 100

        Pass
        {
            Name "UniversalForward"
            Tags { "LightMode"="UniversalForward" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
         
            TEXTURE2D(_WaterTex);   SAMPLER(sampler_WaterTex);
            TEXTURE2D(_FoamTexA);   SAMPLER(sampler_FoamTexA);
            TEXTURE2D(_FoamTexB);   SAMPLER(sampler_FoamTexB);

            float4 _FoamTexA_ST;
            float4 _FoamTexB_ST;
            float4 _WaterTex_ST;

            float4 _FlowDir;
            float  _FlowSpeed;
            float  _FoamBlinkSpeed;
            float4 _WaterColor;
            float4 _FoamColor;
            float4 _FresnelColor;
            float  _FresnelPower;
            float  _FresnelIntensity;
            float  _Transparency;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv         : TEXCOORD0;
            };

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS);
                o.uv         = v.uv;
                return o;
            }

            float4 frag(Varyings i) : SV_Target
            {
                // direcao da agua
                float2 dirNorm  = normalize(_FlowDir.xy);
                float2 uvOffset = dirNorm * (_Time.y * _FlowSpeed);

                // água
                float2 uvW = TRANSFORM_TEX(i.uv, _WaterTex) + uvOffset;
                float4 sampleW = SAMPLE_TEXTURE2D(_WaterTex, sampler_WaterTex, uvW);
                float4 colW = sampleW * _WaterColor;

                // movimento espuma
                float phase = sin(_Time.y * _FoamBlinkSpeed * 6.2831853) * 0.5 + 0.5;

                float2 uvFA = TRANSFORM_TEX(i.uv, _FoamTexA) + uvOffset;
                float2 uvFB = TRANSFORM_TEX(i.uv, _FoamTexB) + uvOffset;

                float foamA = SAMPLE_TEXTURE2D(_FoamTexA, sampler_FoamTexA, uvFA).r * phase;
                float foamB = SAMPLE_TEXTURE2D(_FoamTexB, sampler_FoamTexB, uvFB).r * (1 - phase);
                float foamMask = saturate(foamA + foamB);
                float4 colF = _FoamColor * foamMask;

                // fresnel
                float2 center  = float2(0.5, 0.5);
                float dist = distance(i.uv, center);
                float fresnel = pow(dist, _FresnelPower) * _FresnelIntensity;
                fresnel = saturate(fresnel);
                colW.rgb = lerp(colW.rgb, _FresnelColor.rgb, fresnel);

                // blend água + espuma
                float3 rgbOut = lerp(colW.rgb, colF.rgb, foamMask);
                float alphaW = colW.a;
                float alphaF = _FoamColor.a * foamMask;
                float alphaOut = saturate((alphaW + alphaF) * _Transparency);

                return float4(rgbOut, alphaOut);
            }
            ENDHLSL
        }
    }
    FallBack "Unlit/Transparent"
}
