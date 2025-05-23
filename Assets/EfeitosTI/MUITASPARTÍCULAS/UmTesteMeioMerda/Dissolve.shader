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

            // Vari�veis globais setadas pelo C#
            float4 _MaskWorldPosition; // .xyz = posi��o, n�o usamos .w aqui
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

                // Reconstr�i a posi��o mundial do pixel atual
                float rawDepth = SampleSceneDepth(input.uv);
                float3 worldPos = ComputeWorldSpacePosition(input.uv, rawDepth, UNITY_MATRIX_I_VP);

                float distToMaskCenter = distance(worldPos, _MaskWorldPosition.xyz);

                // alphaBlend: 0 = totalmente jogador/corte, 1 = totalmente cena/parede
                // Se dist < (_MaskRadius - _MaskEdgeSoftness) => estamos dentro da �rea interna do corte (alphaBlend = 0)
                // Se dist > _MaskRadius => estamos fora do corte (alphaBlend = 1)
                // Entre esses dois, temos a transi��o suave.
                float innerRadius = _MaskRadius - _MaskEdgeSoftness;
                float alphaBlend = saturate((distToMaskCenter - innerRadius) / max(_MaskEdgeSoftness, 0.001h)); // Evita divis�o por zero

                if (distToMaskCenter < _MaskRadius) // Se o pixel est� dentro do raio externo do corte
                {
                    half4 playerTexColor = SAMPLE_TEXTURE2D(_PlayerOcclusionTex, sampler_PlayerOcclusionTex, input.uv);

                    // Se a _PlayerOcclusionTex tem o jogador com alpha 1 e fundo com alpha 0:
                    // A cor final ser� uma mistura da cor do jogador e da cor da cena,
                    // controlada pelo alpha da textura do jogador E pela suavidade da borda do c�rculo.
                    // Queremos que o jogador apare�a totalmente onde playerTexColor.a � alto e alphaBlend � baixo.
                    
                    // Mistura a cor do jogador (com seu alfa) com a cor da cena, usando o alphaBlend da borda do c�rculo.
                    // Onde o jogador � vis�vel (playerTexColor.a alta), ele se sobrep�e.
                    // A transi��o para a sceneColor � controlada por alphaBlend.
                    return lerp(playerTexColor, sceneColor, alphaBlend * (1.0 - playerTexColor.a) + playerTexColor.a * alphaBlend);
                    // Uma forma mais simples, se playerTexColor.a � 1 para jogador e 0 para transparente:
                    // return lerp(playerTexColor, sceneColor, alphaBlend);
                    // Se playerTexColor j� for (R,G,B,A_jogador), onde A_jogador � a silhueta:
                    // half3 blendedPlayer = playerTexColor.rgb * playerTexColor.a; // Pr�-multiplica se n�o estiver
                    // half3 blendedScene = sceneColor.rgb * (1.0 - playerTexColor.a);
                    // half3 combinedColor = blendedPlayer + blendedScene; // Combina jogador sobre o fundo (se houver)
                    // return lerp(half4(combinedColor, max(playerTexColor.a, sceneColor.a)), sceneColor, alphaBlend);
                    // A mais direta:
                    // Cor final = CorDoJogador se alphaBlend pr�ximo de 0, CorDaCena se alphaBlend pr�ximo de 1
                    // E a CorDoJogador s� deve aparecer se playerTexColor.a > 0
                    // return lerp( (playerTexColor.a > 0.01 ? playerTexColor : sceneColor), sceneColor, alphaBlend);
                    // Melhor:
                    return lerp(playerTexColor, sceneColor, alphaBlend); // Assumindo que _PlayerOcclusionTex � (R,G,B,A) onde A=0 � transparente.
                                                                       // A cor base da _PlayerOcclusionTex (onde A=0) deve ser (0,0,0,0).
                }
                
                return sceneColor;
            }
            ENDHLSL
        }
    }
}