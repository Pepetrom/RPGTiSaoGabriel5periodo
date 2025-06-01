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
        _EdgeWidth ("Edge Width", Range(0, 0.1)) = 0.01 
        _DissolveScreenRadius ("Dissolve Screen Radius", Range(0, 1)) = 0.1 
        _DissolveSoftness ("Dissolve Softness", Range(0, 1)) = 0.1 
        _NoiseScale ("Noise Scale", Float) = 1.0
        _NoiseSpeed ("Noise Speed", Vector) = (1.0, 0.0, 0.0, 0.0)

       
        [HideInInspector] _PlayerScreenPos ("Player Screen Pos", Vector) = (0.5, 0.5, 0, 0) // Default center
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector"="True" }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off // Often needed for dissolve effects to render backfaces if object becomes thin

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag           
            // Se estiver em URP, considere converter para HLSL e usar Shader Graph ou includes como "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _DissolveTex;
            sampler2D _NoiseTex;
            sampler2D _NormalMap;
            sampler2D _RoughnessMap;
            sampler2D _HeightMap;

            float4 _EdgeColor;
            float _EdgeWidth;
            float _DissolveScreenRadius;
            float _DissolveSoftness;
            float _NoiseScale;
            float4 _NoiseSpeed;
            float4 _MainTex_ST;
            float4 _PlayerScreenPos; // Recebe (x/width, y/height, ?, ?) do script

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL; // Adicionado para Normal Map
                float4 tangent : TANGENT; // Adicionado para Normal Map
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float4 screenPos : TEXCOORD1; // Para coordenadas de tela               
                float3 worldNormal : TEXCOORD2;
                float3 worldTangent : TEXCOORD3;
                float3 worldBinormal : TEXCOORD4;
                float3 worldPos : TEXCOORD5; 
            };

            float2 animateNoiseUV(float2 uv, float4 speed)
            {
                float time = _Time.y;
                return uv + (speed.xy * time);
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.screenPos = ComputeScreenPos(o.pos); // Calcula coordenadas de tela

                // Cálculos para Normal Mapping (se necessário)
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
                o.worldBinormal = cross(o.worldNormal, o.worldTangent) * v.tangent.w * unity_WorldTransformParams.w;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Coordenadas de tela normalizadas (0 a 1)
                float2 screenUV = i.screenPos.xy / i.screenPos.w;

                // Posição do jogador em coordenadas de tela normalizadas (já vem do script)
                float2 playerScreenUV = _PlayerScreenPos.xy;

                // Cálculo da distância no espaço da tela
                float screenDistance = distance(screenUV, playerScreenUV);

                
                // Cálculo da dissolução baseado na distância da tela
                // O raio e a suavidade agora são relativos ao espaço da tela
                float dissolveAlpha = smoothstep(_DissolveScreenRadius - _DissolveSoftness, _DissolveScreenRadius, screenDistance);

                // Noise animado aplicado à borda
                float2 noiseUV = animateNoiseUV(i.uv * _NoiseScale, _NoiseSpeed);
                float noiseValue = tex2D(_NoiseTex, noiseUV).r;

                // Cálculo da borda baseado na distância da tela
                float edgeFactor = smoothstep(_DissolveScreenRadius - _EdgeWidth, _DissolveScreenRadius, screenDistance) - dissolveAlpha;
                edgeFactor = saturate(edgeFactor * (1 + noiseValue)); // Multiplica pelo noise (ajustado para não ficar negativo)

                // Textura base
                fixed4 col = tex2D(_MainTex, i.uv);

               

                // Aplica cor da borda
                col.rgb = lerp(col.rgb, _EdgeColor.rgb, edgeFactor * _EdgeColor.a); // Usa alpha da cor da borda para intensidade

                // Alpha final: 1 - dissolveAlpha significa que fica visível onde dissolveAlpha é 0
                col.a *= dissolveAlpha; 
                // Garante que a borda não adicione alpha onde já é transparente
                col.a = saturate(col.a + edgeFactor); 
                                
                // Recorta pixels totalmente transparentes para otimização
                clip(col.a - 0.01);

                return col;
            }
            ENDCG
        }
    }
    FallBack "Legacy Shaders/Transparent/Diffuse" // Fallback apropriado
    CustomEditor "ShaderGraph.PBRMasterGUI" // Se for URP/HDRP, pode precisar de um custom editor ou ser Shader Graph
}
