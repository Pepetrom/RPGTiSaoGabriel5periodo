Shader "Custom/SimpleDissolveShader"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1) // Cor principal do objeto
        _Cutoff ("Dissolve Amount", Range(0,1)) = 0.5 // Controla a dissolução
    }
    SubShader
    {
        Tags {"RenderType"="Opaque"}
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            float _Cutoff; // Valor de dissolve
            float4 _Color; // Cor do objeto

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex); // Transformação da posição do vértice
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // Randomiza um valor de dissolve para o objeto com base no valor de Cutoff
                if (frac(sin(dot(i.pos.xy, float2(12.9898, 78.233))) * 43758.5453) < _Cutoff)
                {
                    discard; // Remove o pixel se o valor for menor que o Cutoff
                }

                return _Color; // Retorna a cor do objeto se não for dissolvido
            }
            ENDCG
        }
    }
}
