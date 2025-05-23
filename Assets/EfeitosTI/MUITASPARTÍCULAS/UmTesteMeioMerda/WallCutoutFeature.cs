using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WallCutoutFeature : ScriptableRendererFeature {
    [System.Serializable]
    public class WallCutoutSettings {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingOpaques; // Ou BeforeRenderingPostProcessing
        public Shader effectShader; // Atribua o WallCutoutEffect.shader aqui
        public RenderTexture playerOcclusionTexture; // Atribua a RT do jogador aqui
    }

    public WallCutoutSettings settings = new WallCutoutSettings();
    private WallCutoutRenderPass m_ScriptablePass;
    private Material m_EffectMaterial;

    public override void Create() {
        if (settings.effectShader == null) {
            Debug.LogWarning("Shader do efeito de corte não atribuído no WallCutoutFeature.");
            return;
        }
        m_EffectMaterial = CoreUtils.CreateEngineMaterial(settings.effectShader);

        m_ScriptablePass = new WallCutoutRenderPass(m_EffectMaterial, name, settings.playerOcclusionTexture);
        m_ScriptablePass.renderPassEvent = settings.renderPassEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
        if (m_ScriptablePass == null || m_EffectMaterial == null || settings.playerOcclusionTexture == null) return;

        // Aqui você pode querer verificar se o efeito deve ser ativo
        // bool shouldAddPass = ... ;
        // if (shouldAddPass)

        // Passa o target da cor da câmera para o pass
        // m_ScriptablePass.Setup(renderer.cameraColorTarget); // Se necessário pelo seu Setup
        renderer.EnqueuePass(m_ScriptablePass);
    }

    protected override void Dispose(bool disposing) {
        CoreUtils.Destroy(m_EffectMaterial);
    }
}