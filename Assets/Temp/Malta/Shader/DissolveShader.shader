Shader "Custom/AdvancedDissolveShader"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1) // Cor principal do objeto
        _MainTex ("Diffuse Texture", 2D) = "white" {} // Textura difusa
        _MetallicGlossMap ("Metallic Map", 2D) = "white" {} // Mapa de metallic
        _OcclusionMap ("Ambient Occlusion Map", 2D) = "white" {} // Mapa de oclusão
        _BumpMap ("Normal Map", 2D) = "bump" {} // Mapa de normal
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
            #include "UnityStandardUtils.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex; // Textura difusa
            sampler2D _MetallicGlossMap; // Mapa de metallic
            sampler2D _OcclusionMap; // Mapa de oclusão
            sampler2D _BumpMap; // Mapa de normal
            float _Cutoff; // Valor de dissolve
            float4 _Color; // Cor do objeto

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex); // Transformação da posição do vértice
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // Calcula o valor de dissolução
                if (frac(sin(dot(i.pos.xy, float2(12.9898, 78.233))) * 43758.5453) < _Cutoff)
                {
                    discard; // Remove o pixel se o valor for menor que o Cutoff
                }

                // Carrega as texturas
                float4 albedo = tex2D(_MainTex, i.uv) * _Color;
                float metallic = tex2D(_MetallicGlossMap, i.uv).r;
                float occlusion = tex2D(_OcclusionMap, i.uv).r;
                float3 normal = UnpackNormal(tex2D(_BumpMap, i.uv));

                // Aplica os valores das texturas no resultado final
                half3 finalColor = albedo.rgb * occlusion;
                return half4(finalColor, albedo.a);
            }
            ENDCG
        }
    }
}
