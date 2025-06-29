Shader "Custom/WaterUnlitURP_Improved"
{
    Properties
    {
        _WaterTex("Water Map", 2D) = "white" {}
        _FoamTexA("Foam Map A", 2D) = "white" {}
        _FoamTexB("Foam Map B", 2D) = "white" {}
        _NormalMap("Normal Map", 2D) = "bump" {}
        _FlowDir("Flow Direction", Vector) = (1,0,0,0)
        _FlowSpeed("Flow Speed", Float) = 0.1
        _NormalSpeed("Normal Map Speed", Float) = 0.1
        _FoamBlinkSpeed("Foam Blink Speed", Float) = 1.0
        _WaterColor("Water Color", Color) = (0.2,0.6,0.8,0.5)
        _FoamColor("Foam Color", Color) = (1,1,1,0.7)
        _FresnelColor("Fresnel Color", Color) = (0.0,0.2,0.3,0.0)
        _FresnelPower("Fresnel Power", Range(0,10)) = 2
        _FresnelIntensity("Fresnel Intensity", Range(0,1)) = 0.5
        _Transparency("Transparency", Range(0,1)) = 1.0
        _RefractionStrength("Refraction Strength", Range(0, 0.1)) = 0.02
        _WaveAmplitude("Wave Amplitude", Float) = 0.1
        _WaveFrequency("Wave Frequency", Float) = 1.0
        _DepthFadeDistance("Depth Fade Distance", Float) = 1.0
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "Queue"="Transparent" "RenderType"="Transparent" }
        Pass
        {
            Name "Forward"
            Tags { "LightMode"="UniversalForward" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_WaterTex);  SAMPLER(sampler_WaterTex);
            TEXTURE2D(_FoamTexA);  SAMPLER(sampler_FoamTexA);
            TEXTURE2D(_FoamTexB);  SAMPLER(sampler_FoamTexB);
            TEXTURE2D(_NormalMap); SAMPLER(sampler_NormalMap);
            TEXTURE2D(_CameraOpaqueTexture); SAMPLER(sampler_CameraOpaqueTexture);
            TEXTURE2D_X_FLOAT(_CameraDepthTexture); SAMPLER(sampler_CameraDepthTexture);

            float4 _WaterTex_ST, _FoamTexA_ST, _FoamTexB_ST, _NormalMap_ST;
            float4 _FlowDir;
            float _FlowSpeed, _NormalSpeed, _FoamBlinkSpeed;
            float4 _WaterColor, _FoamColor, _FresnelColor;
            float _FresnelPower, _FresnelIntensity, _Transparency, _RefractionStrength;
            float _WaveAmplitude, _WaveFrequency, _DepthFadeDistance;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            Varyings vert(Attributes v)
            {
                Varyings o;
                
                float wave = sin(v.positionOS.x * _WaveFrequency + _Time.y) * cos(v.positionOS.z * _WaveFrequency + _Time.y);
                v.positionOS.y += wave * _WaveAmplitude;

                o.positionCS = TransformObjectToHClip(v.positionOS);
                o.uv = v.uv;
                o.screenPos = ComputeScreenPos(o.positionCS);
                o.worldPos = TransformObjectToWorld(v.positionOS).xyz;
                return o;
            }

            float4 frag(Varyings i) : SV_Target
            {
                float2 dirNorm = normalize(_FlowDir.xy);
                float2 uvOffset = dirNorm * (_Time.y * _FlowSpeed);
                float2 uvNormalOffset = dirNorm * (_Time.y * _NormalSpeed);

                float2 uvN = TRANSFORM_TEX(i.uv, _NormalMap) + uvNormalOffset;
                float3 normalTS = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, uvN));
                float2 distortion = normalTS.xy * _RefractionStrength;

                float2 uvW = TRANSFORM_TEX(i.uv, _WaterTex) + uvOffset + distortion;
                float2 uvFA = TRANSFORM_TEX(i.uv, _FoamTexA) + uvOffset + distortion;
                float2 uvFB = TRANSFORM_TEX(i.uv, _FoamTexB) + uvOffset + distortion;

                float4 sampleW = SAMPLE_TEXTURE2D(_WaterTex, sampler_WaterTex, uvW);
                float4 colW = sampleW * _WaterColor;

                float phase = sin(_Time.y * _FoamBlinkSpeed * 6.2831853) * 0.5 + 0.5;
                float foamA = SAMPLE_TEXTURE2D(_FoamTexA, sampler_FoamTexA, uvFA).r * phase;
                float foamB = SAMPLE_TEXTURE2D(_FoamTexB, sampler_FoamTexB, uvFB).r * (1.0 - phase);
                float foamMask = saturate(foamA + foamB);

                float2 center = float2(0.5, 0.5);
                float dist = distance(i.uv, center);
                float fresnel = pow(dist, _FresnelPower) * _FresnelIntensity;
                fresnel = saturate(fresnel);
                colW.rgb = lerp(colW.rgb, _FresnelColor.rgb, fresnel);

                float2 screenUV = i.screenPos.xy / i.screenPos.w;
                screenUV += distortion;
                float4 background = SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, screenUV);

                // Depth fade 
                float sceneRawDepth = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, screenUV);
                float sceneLinearDepth = LinearEyeDepth(sceneRawDepth, _ZBufferParams);
                float waterLinearDepth = i.screenPos.w;
                float depthDiff = saturate((sceneLinearDepth - waterLinearDepth) / _DepthFadeDistance);

                // Espuma extra na borda
                float contactFoam = 1.0 - depthDiff;
                foamMask = saturate(foamMask + contactFoam);

                float3 finalColor = lerp(background.rgb, colW.rgb, colW.a);
                finalColor = lerp(finalColor, _FoamColor.rgb, foamMask);

                float alphaOut = saturate((_WaterColor.a + _FoamColor.a * foamMask) * _Transparency * depthDiff);

                return float4(finalColor, alphaOut);
            }
            ENDHLSL
        }
    }
    FallBack "Unlit/Transparent"
}
