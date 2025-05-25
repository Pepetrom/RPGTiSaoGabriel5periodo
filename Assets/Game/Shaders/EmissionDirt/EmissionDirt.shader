Shader "Custom/URP_UnlitDirtGlow"
{
    Properties
    {
        _ColorDirt("Color Dirt", Color) = (1,1,1,1)
        _DirtTex("Dirt Texture", 2D) = "white" {}
        _EmissionSlider("Emission Intensity", Range(0, 5)) = 1.0
        _GlowVel("Glow Velocity", Range(0, 5)) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 100

        Pass
        {
            Name "UnlitPass"
            Tags { "LightMode"="UniversalForward" }

            ZWrite On
            Blend One Zero

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _ColorDirt;
                float _EmissionSlider;
                float _GlowVel;
                float4 _DirtTex_ST; 
            CBUFFER_END

            TEXTURE2D(_DirtTex);
            SAMPLER(sampler_DirtTex);

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {               
                float2 uv = IN.uv * _DirtTex_ST.xy + _DirtTex_ST.zw;
                half4 dirt = SAMPLE_TEXTURE2D(_DirtTex, sampler_DirtTex, uv) * _ColorDirt; 
                half glow = sin(_Time.y * _GlowVel) * 0.5 + 0.5;
                half3 emission = dirt.rgb * _EmissionSlider * glow;
                return half4(dirt.rgb + emission, 1.0);
            }
            ENDHLSL
        }
    }

    FallBack Off
}
