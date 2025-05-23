using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WallCutoutRenderPass : ScriptableRenderPass {
    private Material m_Material;
    private RenderTargetIdentifier m_CameraColorTarget;
    private RenderTargetHandle m_TempColorTexture; // Para blit intermedi�rio se necess�rio
    private string m_ProfilerTag;
    private Texture m_PlayerOcclusionTexture;


    public WallCutoutRenderPass(Material material, string profilerTag, Texture playerOcclusionTexture) {
        m_Material = material;
        m_ProfilerTag = profilerTag;
        m_PlayerOcclusionTexture = playerOcclusionTexture;
        m_TempColorTexture.Init("_TempColorTexture"); // Nome da textura tempor�ria
    }

    // Este m�todo � chamado pelo renderer antes de executar o pass.
    // Pode ser usado para configurar render targets e limpar o estado.
    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData) {
        m_CameraColorTarget = renderingData.cameraData.renderer.cameraColorTarget;
        if (m_Material != null && m_PlayerOcclusionTexture != null) {
            m_Material.SetTexture("_PlayerOcclusionTex", m_PlayerOcclusionTexture);
        }
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {
        if (m_Material == null || m_PlayerOcclusionTexture == null) {
            // Debug.LogError("Material ou Player Occlusion Texture n�o configurado para WallCutoutRenderPass.");
            return;
        }
        if (renderingData.cameraData.isPreviewCamera) return; // N�o rodar em c�meras de preview


        CommandBuffer cmd = CommandBufferPool.Get(m_ProfilerTag);

        // Blit da cor da c�mera para uma textura tempor�ria
        //RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
        //opaqueDesc.depthBufferBits = 0; // N�o precisamos de depth para a textura tempor�ria
        //cmd.GetTemporaryRT(m_TempColorTexture.id, opaqueDesc);
        //cmd.Blit(m_CameraColorTarget, m_TempColorTexture.Identifier());

        // Aplica o efeito: l� da m_TempColorTexture (agora source) e escreve de volta no m_CameraColorTarget (destination)
        //cmd.Blit(m_TempColorTexture.Identifier(), m_CameraColorTarget, m_Material);

        // Ou Blit diretamente se o material puder ler e escrever no mesmo target (pode causar problemas em algumas plataformas)
        // Mais seguro usar uma textura intermedi�ria ou usar Blit com source e destination diferentes.
        // Para um efeito de tela cheia que l� o RT da c�mera e escreve nele, o pipeline do URP
        // geralmente lida com isso internamente se voc� usar renderer.cameraColorTarget.
        // A forma mais simples com URP � usar Blit sobre o target da c�mera, mas o material precisa
        // referenciar a textura correta (_MainTex). O URP faz isso automaticamente com Blit.

        Blit(cmd, m_CameraColorTarget, m_CameraColorTarget, m_Material, 0); // Pass 0 do shader

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
        //cmd.ReleaseTemporaryRT(m_TempColorTexture.id); // Libera a textura tempor�ria
    }

    // Limpa quaisquer recursos alocados pelo ScriptableRenderPass
    public override void OnCameraCleanup(CommandBuffer cmd) {
        // if (cmd != null)
        // {
        //     cmd.ReleaseTemporaryRT(m_TempColorTexture.id);
        // }
    }
}