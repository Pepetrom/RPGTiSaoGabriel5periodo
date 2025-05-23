Shader "Unlit/Outro"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Mask ("Noise Mask", 2D) = "white" {}
        _MaskT ("Noise Mask", 2D) = "white" {}
        [HDR]_Color ("Color", Color) = (1,1,1,1)
        _TexSpeed ("Texture Speed", Range(-0.5,0.5)) = 0.5
        _ErosionA ("Erosion", Range(0, 5)) = 0.5
        _ErosionB ("Erosion", Range(0, 100)) = 0.5
        _Power ("Power", Float) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        //ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            sampler2D _Mask, _MaskT;
            float4 _MainTex_ST;
            float4 _Mask_ST;
            float4 _MaskT_ST;
            float4 _Color;
            float _TexSpeed;
            float _ErosionA, _ErosionB;
            float _Power;

            inline float unity_noise_randomValue (float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453);
            }

            inline float unity_noise_interpolate (float a, float b, float t)
            {
                return (1.0 - t) * a + (t * b);
            }

            inline float unity_valueNoise (float2 uv)
            {
                float2 i = floor(uv);
                float2 f = frac(uv);
                f = f * f * (3.0 - 2.0 * f);

                float2 c0 = i + float2(0.0, 0.0);
                float2 c1 = i + float2(1.0, 0.0);
                float2 c2 = i + float2(0.0, 1.0);
                float2 c3 = i + float2(1.0, 1.0);
                float r0 = unity_noise_randomValue(c0);
                float r1 = unity_noise_randomValue(c1);
                float r2 = unity_noise_randomValue(c2);
                float r3 = unity_noise_randomValue(c3);

                float bottomOfGrid = unity_noise_interpolate(r0, r1, f.x);
                float topOfGrid = unity_noise_interpolate(r2, r3, f.x);
                float t = unity_noise_interpolate(bottomOfGrid, topOfGrid, f.y);
                return t;
            }

            float SimpleNoise(float2 UV, float Scale)
            {
                float t = 0.0;

                float freq = pow(2.0, float(0));
                float amp = pow(0.5, float(3 - 0));
                t += unity_valueNoise(UV * Scale / freq) * amp;

                freq = pow(2.0, float(1));
                amp = pow(0.5, float(3 - 1));
                t += unity_valueNoise(UV * Scale / freq) * amp;

                freq = pow(2.0, float(2));
                amp = pow(0.5, float(3 - 2));
                t += unity_valueNoise(UV * Scale / freq) * amp;

                return t;
            }

            v2f vert(appdata IN)
            {
                v2f OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            half4 frag(v2f IN) : SV_Target
            {
                float2 uvO = IN.uv + float2(0, _Time.y * _TexSpeed);
                float4 mc = tex2D(_MainTex, uvO) * _Color;
                float4 mask1 = tex2D(_Mask, IN.uv + float2(_Time.x, 0));
                float4 mask2 = tex2D(_Mask, IN.uv - float2(_Time.x, 0));
                float4 maskT = tex2D(_MaskT, IN.uv - float2(_Time.x,0));
                mc.a = mask1 * mask2 + smoothstep(_ErosionA, 0.2, SimpleNoise(uvO, _ErosionB)) * _Color;
                float4 mcf = mc *= maskT.r;
                return mcf;
            }
            ENDHLSL
        }
    }
}
