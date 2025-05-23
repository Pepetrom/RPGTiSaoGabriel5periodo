// Guarde este arquivo como "WallCutoutEffect.shader" em uma pasta do seu projeto (ex: Shaders)
Shader "Hidden/WallCutoutEffect"
{
    Properties
    {
        _MainTex ("Source (RGB)", 2D) = "white" {} // Cena renderizada
        _PlayerOcclusionTex ("Player Occlusion (RGBA)", 2D) = "black" {} // Jogador renderizado separadamente
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" }
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            Name "WallCutoutPass"
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float2 uv           : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_PlayerOcclusionTex);
            SAMPLER(sampler_PlayerOcclusionTex);

            // Variáveis globais setadas pelo C#
            float4 _MaskWorldPosition; // .xyz = posição, não usamos .w aqui
            float _MaskRadius;
            float _MaskEdgeSoftness;   // Largura da borda suave em unidades do mundo
            int _EnableWallCutout;

            Varyings Vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;
                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                half4 sceneColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);

                if (_EnableWallCutout == 0)
                {
                    return sceneColor;
                }

                // Reconstrói a posição mundial do pixel atual
                float rawDepth = SampleSceneDepth(input.uv);
                float3 worldPos = ComputeWorldSpacePosition(input.uv, rawDepth, UNITY_MATRIX_I_VP);

                float distToMaskCenter = distance(worldPos, _MaskWorldPosition.xyz);

                // alphaBlend: 0 = totalmente jogador/corte, 1 = totalmente cena/parede
                // Se dist < (_MaskRadius - _MaskEdgeSoftness) => estamos dentro da área interna do corte (alphaBlend = 0)
                // Se dist > _MaskRadius => estamos fora do corte (alphaBlend = 1)
                // Entre esses dois, temos a transição suave.
                float innerRadius = _MaskRadius - _MaskEdgeSoftness;
                float alphaBlend = saturate((distToMaskCenter - innerRadius) / max(_MaskEdgeSoftness, 0.001h)); // Evita divisão por zero

                if (distToMaskCenter < _MaskRadius) // Se o pixel está dentro do raio externo do corte
                {
                    half4 playerTexColor = SAMPLE_TEXTURE2D(_PlayerOcclusionTex, sampler_PlayerOcclusionTex, input.uv);

                    // Se a _PlayerOcclusionTex tem o jogador com alpha 1 e fundo com alpha 0:
                    // A cor final será uma mistura da cor do jogador e da cor da cena,
                    // controlada pelo alpha da textura do jogador E pela suavidade da borda do círculo.
                    // Queremos que o jogador apareça totalmente onde playerTexColor.a é alto e alphaBlend é baixo.
                    
                    // Mistura a cor do jogador (com seu alfa) com a cor da cena, usando o alphaBlend da borda do círculo.
                    // Onde o jogador é visível (playerTexColor.a alta), ele se sobrepõe.
                    // A transição para a sceneColor é controlada por alphaBlend.
                    return lerp(playerTexColor, sceneColor, alphaBlend * (1.0 - playerTexColor.a) + playerTexColor.a * alphaBlend);
                    // Uma forma mais simples, se playerTexColor.a é 1 para jogador e 0 para transparente:
                    // return lerp(playerTexColor, sceneColor, alphaBlend);
                    // Se playerTexColor já for (R,G,B,A_jogador), onde A_jogador é a silhueta:
                    // half3 blendedPlayer = playerTexColor.rgb * playerTexColor.a; // Pré-multiplica se não estiver
                    // half3 blendedScene = sceneColor.rgb * (1.0 - playerTexColor.a);
                    // half3 combinedColor = blendedPlayer + blendedScene; // Combina jogador sobre o fundo (se houver)
                    // return lerp(half4(combinedColor, max(playerTexColor.a, sceneColor.a)), sceneColor, alphaBlend);
                    // A mais direta:
                    // Cor final = CorDoJogador se alphaBlend próximo de 0, CorDaCena se alphaBlend próximo de 1
                    // E a CorDoJogador só deve aparecer se playerTexColor.a > 0
                    // return lerp( (playerTexColor.a > 0.01 ? playerTexColor : sceneColor), sceneColor, alphaBlend);
                    // Melhor:
                    return lerp(playerTexColor, sceneColor, alphaBlend); // Assumindo que _PlayerOcclusionTex é (R,G,B,A) onde A=0 é transparente.
                                                                       // A cor base da _PlayerOcclusionTex (onde A=0) deve ser (0,0,0,0).
                }
                
                return sceneColor;
            }
            ENDHLSL
        }
    }
}