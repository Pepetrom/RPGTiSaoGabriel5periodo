using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WallCutoutRenderPass : ScriptableRenderPass {
    private Material m_Material;
    private RenderTargetIdentifier m_CameraColorTarget;
    private RenderTargetHandle m_TempColorTexture; // Para blit intermediário se necessário
    private string m_ProfilerTag;
    private Texture m_PlayerOcclusionTexture;


    public WallCutoutRenderPass(Material material, string profilerTag, Texture playerOcclusionTexture) {
        m_Material = material;
        m_ProfilerTag = profilerTag;
        m_PlayerOcclusionTexture = playerOcclusionTexture;
        m_TempColorTexture.Init("_TempColorTexture"); // Nome da textura temporária
    }

    // Este método é chamado pelo renderer antes de executar o pass.
    // Pode ser usado para configurar render targets e limpar o estado.
    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData) {
        m_CameraColorTarget = renderingData.cameraData.renderer.cameraColorTarget;
        if (m_Material != null && m_PlayerOcclusionTexture != null) {
            m_Material.SetTexture("_PlayerOcclusionTex", m_PlayerOcclusionTexture);
        }
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {
        if (m_Material == null || m_PlayerOcclusionTexture == null) {
            // Debug.LogError("Material ou Player Occlusion Texture não configurado para WallCutoutRenderPass.");
            return;
        }
        if (renderingData.cameraData.isPreviewCamera) return; // Não rodar em câmeras de preview


        CommandBuffer cmd = CommandBufferPool.Get(m_ProfilerTag);

        // Blit da cor da câmera para uma textura temporária
        //RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
        //opaqueDesc.depthBufferBits = 0; // Não precisamos de depth para a textura temporária
        //cmd.GetTemporaryRT(m_TempColorTexture.id, opaqueDesc);
        //cmd.Blit(m_CameraColorTarget, m_TempColorTexture.Identifier());

        // Aplica o efeito: lê da m_TempColorTexture (agora source) e escreve de volta no m_CameraColorTarget (destination)
        //cmd.Blit(m_TempColorTexture.Identifier(), m_CameraColorTarget, m_Material);

        // Ou Blit diretamente se o material puder ler e escrever no mesmo target (pode causar problemas em algumas plataformas)
        // Mais seguro usar uma textura intermediária ou usar Blit com source e destination diferentes.
        // Para um efeito de tela cheia que lê o RT da câmera e escreve nele, o pipeline do URP
        // geralmente lida com isso internamente se você usar renderer.cameraColorTarget.
        // A forma mais simples com URP é usar Blit sobre o target da câmera, mas o material precisa
        // referenciar a textura correta (_MainTex). O URP faz isso automaticamente com Blit.

        Blit(cmd, m_CameraColorTarget, m_CameraColorTarget, m_Material, 0); // Pass 0 do shader

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
        //cmd.ReleaseTemporaryRT(m_TempColorTexture.id); // Libera a textura temporária
    }

    // Limpa quaisquer recursos alocados pelo ScriptableRenderPass
    public override void OnCameraCleanup(CommandBuffer cmd) {
        // if (cmd != null)
        // {
        //     cmd.ReleaseTemporaryRT(m_TempColorTexture.id);
        // }
    }
}