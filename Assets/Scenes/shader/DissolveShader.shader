Shader "Custom/AdvancedDissolveShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _DissolveTex ("Dissolve Texture", 2D) = "black" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _RoughnessMap ("Roughness Map", 2D) = "white" {}
        _HeightMap ("Height Map", 2D) = "white" {}

        _EdgeColor ("Edge Color", Color) = (1, 0, 0, 1)
        _EdgeWidth ("Edge Width", Range(0, 0.1)) = 0.02
        _DissolveRadius ("Dissolve Radius", Float) = 1.0
        _DissolveSoftness ("Dissolve Softness", Range(0, 1)) = 0.2
        _DissolveOffset ("Dissolve Offset", Float) = 0.0
        _NoiseScale ("Noise Scale", Float) = 1.0
        _NoiseSpeed ("Noise Speed", Vector) = (1.0, 0.0, 0.0, 0.0)
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc" 

            sampler2D _MainTex;
            sampler2D _DissolveTex;
            sampler2D _NoiseTex;
            sampler2D _NormalMap;
            sampler2D _RoughnessMap;
            sampler2D _HeightMap;

            float4 _EdgeColor;
            float _EdgeWidth;
            float _DissolveRadius;
            float _DissolveSoftness;
            float _DissolveOffset;
            float _NoiseScale;
            float4 _NoiseSpeed;
            float4 _MainTex_ST; 

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 viewPos : TEXCOORD1;
            };

            //anima o UV do Noise
            float2 animateNoiseUV(float2 uv, float4 speed)
            {
                float time = _Time.y; 
                return uv + (speed.xy * time);
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                //macro TRANSFORM_TEX serve pra aplicar escala e offset
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.viewPos = mul(UNITY_MATRIX_V, worldPos).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {                
                float2 uv = i.uv;

                // calculo do dissolve
                float distanceToCenter = length(i.viewPos.xy);
                float2 dissolveUV = uv + _DissolveOffset;
                float dissolveValue = tex2D(_DissolveTex, dissolveUV).r;
                float dissolveAlpha = smoothstep(_DissolveRadius - _DissolveSoftness, _DissolveRadius, distanceToCenter + dissolveValue);

                // aplica o noise
                float2 noiseUV = animateNoiseUV(uv * _NoiseScale, _NoiseSpeed);
                float noiseValue = tex2D(_NoiseTex, noiseUV).r;

                // calculo do edge
                float edgeFactor = smoothstep(_DissolveRadius - _EdgeWidth, _DissolveRadius, distanceToCenter + dissolveValue) - dissolveAlpha;
                edgeFactor *= noiseValue;
                
                fixed4 col = tex2D(_MainTex, uv);
                
                float3 normal = tex2D(_NormalMap, uv).rgb * 2.0 - 1.0;
             
                col.rgb = lerp(col.rgb, _EdgeColor.rgb, edgeFactor);
                
                col.a *= dissolveAlpha + edgeFactor;

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}