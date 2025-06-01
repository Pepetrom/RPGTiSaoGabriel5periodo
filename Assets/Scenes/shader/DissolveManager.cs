using UnityEngine;
using System.Collections.Generic;

public class DissolveManager : MonoBehaviour
{
public Transform player;
    public Camera mainCamera;
    public Shader dissolveShader;
    public Texture2D dissolveTexture;
    public Texture2D noiseTexture;

    public float dissolveRadius = 1.0f;
    public float dissolveSoftness = 0.2f;
    public float edgeWidth = 0.02f;
    public Color edgeColor = Color.red;

    public float noiseScale = 1.0f;
    public Vector2 noiseSpeed = new Vector2(1.0f, 0.0f);

    
    public LayerMask dissolveLayerMask = ~0; 

    private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();
    private HashSet<Renderer> affectedRenderers = new HashSet<Renderer>();
    private List<Material> createdDissolveMaterials = new List<Material>(); // Para gerenciar materiais criados

    void Update()
    {
        if (player == null || mainCamera == null || dissolveShader == null)
        {
            Debug.LogWarning("DissolveManager: Player, MainCamera ou DissolveShader não atribuídos.");
            return;
        }

        // Direção entre a câmera e o jogador
        Vector3 direction = (player.position - mainCamera.transform.position).normalized;
        float distance = Vector3.Distance(player.position, mainCamera.transform.position);

        // Raycast para detectar objetos entre o jogador e a câmera, usando a LayerMask
        Ray ray = new Ray(mainCamera.transform.position, direction);
        // Usamos RaycastAll para pegar todos os objetos no caminho
        RaycastHit[] hits = Physics.RaycastAll(ray, distance, dissolveLayerMask, QueryTriggerInteraction.Ignore);

        HashSet<Renderer> currentFrameRenderers = new HashSet<Renderer>();

        foreach (var hit in hits)
        {            
            if (hit.transform == player)
            {
                continue; // Pula para a próxima iteração se o objeto atingido for o player
            }

            Renderer renderer = hit.collider.GetComponent<Renderer>();

            if (renderer != null)
            {
                currentFrameRenderers.Add(renderer);

                if (!originalMaterials.ContainsKey(renderer))
                {
                    // Salva os materiais originais (pode haver mais de um)
                    originalMaterials[renderer] = renderer.sharedMaterials;

                    // Cria novos materiais de dissolve para cada submesh
                    Material[] dissolveMaterials = new Material[renderer.sharedMaterials.Length];
                    for (int i = 0; i < renderer.sharedMaterials.Length; i++)
                    {
                        Material originalMat = renderer.sharedMaterials[i];
                        Material dissolveMaterial = new Material(dissolveShader);
                        
                        // Copia propriedades básicas se existirem no original (Textura principal, Cor)
                        if (originalMat.HasProperty("_MainTex"))
                            dissolveMaterial.SetTexture("_MainTex", originalMat.GetTexture("_MainTex"));
                        else 
                            dissolveMaterial.SetTexture("_MainTex", Texture2D.whiteTexture); // Fallback
                            
                        if (originalMat.HasProperty("_Color"))
                            dissolveMaterial.SetColor("_Color", originalMat.color);
                        else
                            dissolveMaterial.SetColor("_Color", Color.white); // Fallback

                        // Define as texturas e propriedades do dissolve
                        dissolveMaterial.SetTexture("_DissolveTex", dissolveTexture);
                        dissolveMaterial.SetTexture("_NoiseTex", noiseTexture);
                        // Copia outras propriedades relevantes se necessário (NormalMap, etc.)
                        if (originalMat.HasProperty("_NormalMap"))
                           dissolveMaterial.SetTexture("_NormalMap", originalMat.GetTexture("_NormalMap"));
                        if (originalMat.HasProperty("_RoughnessMap"))
                           dissolveMaterial.SetTexture("_RoughnessMap", originalMat.GetTexture("_RoughnessMap"));
                         if (originalMat.HasProperty("_HeightMap"))
                           dissolveMaterial.SetTexture("_HeightMap", originalMat.GetTexture("_HeightMap"));

                        dissolveMaterials[i] = dissolveMaterial;
                        createdDissolveMaterials.Add(dissolveMaterial); // Adiciona à lista para limpeza posterior
                    }
                    renderer.materials = dissolveMaterials; // Aplica os novos materiais
                }

                // Atualiza os parâmetros dos materiais de dissolve (todos os submeshes)
                foreach (Material mat in renderer.materials)
                {
                    // Verifica se o material realmente usa o shader de dissolve antes de setar propriedades
                    if (mat.shader == dissolveShader)
                    {
                        // Calcula a posição do ponto de hit em relação ao centro do objeto (ou use a posição do player)
                        // Vector3 localHitPoint = renderer.transform.InverseTransformPoint(hit.point);
                        // Poderia usar a posição do player como centro do dissolve:
                        Vector3 dissolveCenterWorld = player.position;
                        mat.SetVector("_DissolveCenterWorld", dissolveCenterWorld);

                        mat.SetFloat("_DissolveRadius", dissolveRadius);
                        mat.SetFloat("_DissolveSoftness", dissolveSoftness);
                        mat.SetFloat("_EdgeWidth", edgeWidth);
                        mat.SetColor("_EdgeColor", edgeColor);
                        mat.SetFloat("_NoiseScale", noiseScale);
                        mat.SetVector("_NoiseSpeed", new Vector4(noiseSpeed.x, noiseSpeed.y, 0, 0));
                        // Você pode precisar passar a posição do jogador para o shader se quiser que o dissolve seja centrado nele
                        // mat.SetVector("_PlayerPosition", player.position);
                    }
                }
            }
        }

        // Restaura materiais de objetos que não estão mais entre a câmera e o jogador
        // Cria uma cópia da lista de chaves para poder modificar o dicionário durante a iteração
        List<Renderer> renderersToRemove = new List<Renderer>();
        foreach (var kvp in originalMaterials)
        {
            Renderer renderer = kvp.Key;
            if (renderer == null) // Objeto pode ter sido destruído
            {
                renderersToRemove.Add(renderer); 
                continue;
            }

            if (!currentFrameRenderers.Contains(renderer))
            {
                // Limpa os materiais de dissolve criados para este renderer antes de restaurar
                foreach(Material mat in renderer.materials)
                {
                    if(mat.shader == dissolveShader && createdDissolveMaterials.Contains(mat))
                    {
                        Destroy(mat); // Destroi o material criado
                        createdDissolveMaterials.Remove(mat);
                    }
                }
                
                renderer.materials = kvp.Value; // Restaura os materiais originais
                renderersToRemove.Add(renderer);
            }
        }

        // Remove as entradas do dicionário fora do loop principal
        foreach(var renderer in renderersToRemove)
        {
            originalMaterials.Remove(renderer);
        }

        // Atualiza a lista de renderizadores afetados para o próximo frame (opcional, currentFrameRenderers já faz isso)
        affectedRenderers = currentFrameRenderers;
    }

    // Limpa materiais criados quando o script é desabilitado ou destruído
    void OnDisable()
    {
        RestoreAllMaterials();
    }

    void OnDestroy()
    {
        RestoreAllMaterials();
        // Limpa a lista de materiais criados restantes
        foreach (var mat in createdDissolveMaterials)
        {
            if (mat != null) Destroy(mat);
        }
        createdDissolveMaterials.Clear();
    }

    void RestoreAllMaterials()
    {
        // Restaura todos os materiais originais
        foreach (var kvp in originalMaterials)
        {
            Renderer renderer = kvp.Key;
            if (renderer != null)
            {
                 // Limpa os materiais de dissolve criados para este renderer
                foreach(Material mat in renderer.materials)
                {
                    if(mat.shader == dissolveShader && createdDissolveMaterials.Contains(mat))
                    {
                        Destroy(mat); 
                        createdDissolveMaterials.Remove(mat);
                    }
                }
                renderer.materials = kvp.Value;
            }
        }
        originalMaterials.Clear();
        affectedRenderers.Clear();
    }
}
