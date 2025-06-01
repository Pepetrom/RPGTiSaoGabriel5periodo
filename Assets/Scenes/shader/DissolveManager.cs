using UnityEngine;
using System.Collections.Generic;

public class DissolveManager : MonoBehaviour
{
 public Transform player;
    public Camera mainCamera;
    // Referência ao NOVO shader screen-space
    public Shader dissolveShaderScreenSpace; 
    public Texture2D dissolveTexture;
    public Texture2D noiseTexture;

    // Propriedades ajustadas para screen space
    [Range(0, 1)] public float dissolveScreenRadius = 0.1f;
    [Range(0, 1)] public float dissolveSoftness = 0.1f;
    [Range(0, 0.1f)] public float edgeWidth = 0.01f;
    public Color edgeColor = Color.red;

    public float noiseScale = 1.0f;
    public Vector2 noiseSpeed = new Vector2(1.0f, 0.0f);

    public LayerMask dissolveLayerMask = ~0; 

    private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();
    private HashSet<Renderer> affectedRenderers = new HashSet<Renderer>();
    private List<Material> createdDissolveMaterials = new List<Material>();

    void Update()
    {
        if (player == null || mainCamera == null || dissolveShaderScreenSpace == null)
        {
            Debug.LogWarning("DissolveManager_ScreenSpace: Player, MainCamera ou DissolveShaderScreenSpace não atribuídos.");
            return;
        }
         
        Vector3 playerScreenPoint = mainCamera.WorldToScreenPoint(player.position);
        // Normaliza para coordenadas de viewport (0 a 1)
        Vector4 playerScreenPosNormalized = new Vector4(
            playerScreenPoint.x / Screen.width,
            playerScreenPoint.y / Screen.height,
            0, 0
        );
        // Garante que esteja dentro dos limites (caso o player saia da tela)
        playerScreenPosNormalized.x = Mathf.Clamp01(playerScreenPosNormalized.x);
        playerScreenPosNormalized.y = Mathf.Clamp01(playerScreenPosNormalized.y);
        // --------------------------------------------------------------------------

        Vector3 direction = (player.position - mainCamera.transform.position).normalized;
        float distance = Vector3.Distance(player.position, mainCamera.transform.position);

        Ray ray = new Ray(mainCamera.transform.position, direction);
        RaycastHit[] hits = Physics.RaycastAll(ray, distance, dissolveLayerMask, QueryTriggerInteraction.Ignore);

        HashSet<Renderer> currentFrameRenderers = new HashSet<Renderer>();

        foreach (var hit in hits)
        {
            if (hit.transform == player)
            {
                continue; 
            }

            Renderer renderer = hit.collider.GetComponent<Renderer>();

            if (renderer != null)
            {
                currentFrameRenderers.Add(renderer);

                if (!originalMaterials.ContainsKey(renderer))
                {
                    originalMaterials[renderer] = renderer.sharedMaterials;
                    Material[] dissolveMaterials = new Material[renderer.sharedMaterials.Length];
                    for (int i = 0; i < renderer.sharedMaterials.Length; i++)
                    {
                        Material originalMat = renderer.sharedMaterials[i];
                        
                        Material dissolveMaterial = new Material(dissolveShaderScreenSpace); 
                        
                        // Copia propriedades básicas
                        if (originalMat.HasProperty("_MainTex"))
                            dissolveMaterial.SetTexture("_MainTex", originalMat.GetTexture("_MainTex"));
                        else 
                            dissolveMaterial.SetTexture("_MainTex", Texture2D.whiteTexture);
                        if (originalMat.HasProperty("_Color"))
                            dissolveMaterial.SetColor("_Color", originalMat.color);
                        else
                            dissolveMaterial.SetColor("_Color", Color.white);

                        dissolveMaterial.SetTexture("_DissolveTex", dissolveTexture);
                        dissolveMaterial.SetTexture("_NoiseTex", noiseTexture);
                        
                        // Copia outras propriedades relevantes
                        if (originalMat.HasProperty("_NormalMap"))
                           dissolveMaterial.SetTexture("_NormalMap", originalMat.GetTexture("_NormalMap"));
                        if (originalMat.HasProperty("_RoughnessMap"))
                           dissolveMaterial.SetTexture("_RoughnessMap", originalMat.GetTexture("_RoughnessMap"));
                         if (originalMat.HasProperty("_HeightMap"))
                           dissolveMaterial.SetTexture("_HeightMap", originalMat.GetTexture("_HeightMap"));

                        dissolveMaterials[i] = dissolveMaterial;
                        createdDissolveMaterials.Add(dissolveMaterial);
                    }
                    renderer.materials = dissolveMaterials;
                }

                // Atualiza os parâmetros dos materiais de dissolve
                foreach (Material mat in renderer.materials)
                {
                    // Verifica se o material usa o shader correto
                    if (mat.shader == dissolveShaderScreenSpace)
                    {                       
                        mat.SetVector("_PlayerScreenPos", playerScreenPosNormalized);
                        // --------------------------------------------------

                        // Usa as novas propriedades do shader
                        mat.SetFloat("_DissolveScreenRadius", dissolveScreenRadius);
                        mat.SetFloat("_DissolveSoftness", dissolveSoftness);
                        mat.SetFloat("_EdgeWidth", edgeWidth);
                        mat.SetColor("_EdgeColor", edgeColor);
                        mat.SetFloat("_NoiseScale", noiseScale);
                        mat.SetVector("_NoiseSpeed", new Vector4(noiseSpeed.x, noiseSpeed.y, 0, 0));
                    }
                }
            }
        }

        List<Renderer> renderersToRemove = new List<Renderer>();
        foreach (var kvp in originalMaterials)
        {
            Renderer renderer = kvp.Key;
            if (renderer == null) 
            {
                renderersToRemove.Add(renderer); 
                continue;
            }

            if (!currentFrameRenderers.Contains(renderer))
            {
                foreach(Material mat in renderer.materials)
                {
                    // Verifica se é um material de dissolve criado por este script
                    if(createdDissolveMaterials.Contains(mat))
                    {
                        Destroy(mat); 
                        createdDissolveMaterials.Remove(mat);
                    }
                }
                
                renderer.materials = kvp.Value; 
                renderersToRemove.Add(renderer);
            }
        }

        foreach(var renderer in renderersToRemove)
        {
            originalMaterials.Remove(renderer);
        }

        affectedRenderers = currentFrameRenderers;
    }

    void OnDisable()
    {
        RestoreAllMaterials();
    }

    void OnDestroy()
    {
        RestoreAllMaterials();
        foreach (var mat in createdDissolveMaterials)
        {
            if (mat != null) Destroy(mat);
        }
        createdDissolveMaterials.Clear();
    }

    void RestoreAllMaterials()
    {
        foreach (var kvp in originalMaterials)
        {
            Renderer renderer = kvp.Key;
            if (renderer != null)
            {
                foreach(Material mat in renderer.materials)
                {
                    if(createdDissolveMaterials.Contains(mat))
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

