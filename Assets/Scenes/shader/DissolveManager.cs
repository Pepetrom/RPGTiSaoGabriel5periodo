using UnityEngine;
using System.Collections.Generic;

public class DissolveManager : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Camera mainCamera;
    public Shader dissolveShaderScreenSpace;
    public Texture2D dissolveTexture;
    public Texture2D noiseTexture;

    [Header("Dissolve Parameters")]
    [Range(0, 1)] public float dissolveScreenRadius = 0.1f;
    [Range(0, 1)] public float dissolveSoftness = 0.1f;
    [Range(0, 0.1f)] public float edgeWidth = 0.01f;
    [Range(0, 2)] public float noiseEdgeStrength = 1f;
    public Color edgeColor = Color.red;

    public float noiseScale = 1.0f;
    public Vector2 noiseSpeed = new Vector2(1.0f, 0.0f);
    public LayerMask dissolveLayerMask = ~0;

    private readonly Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();
    private readonly List<Renderer> renderersToRemove = new List<Renderer>();
    private readonly HashSet<Renderer> currentFrameRenderers = new HashSet<Renderer>();
    private readonly List<Material> createdDissolveMaterials = new List<Material>();

    void Update()
    {
        if (!player || !mainCamera || !dissolveShaderScreenSpace)
        {
            Debug.LogWarning("DissolveManager: Missing references.");
            return;
        }

        Vector3 playerScreenPoint = mainCamera.WorldToScreenPoint(player.position);
        Vector4 playerScreenPosNormalized = new Vector4(
            Mathf.Clamp01(playerScreenPoint.x / Screen.width),
            Mathf.Clamp01(playerScreenPoint.y / Screen.height),
            0, 0
        );

        Vector3 direction = (player.position - mainCamera.transform.position).normalized;
        float distance = Vector3.Distance(player.position, mainCamera.transform.position);

        Ray ray = new Ray(mainCamera.transform.position, direction);
        RaycastHit[] hits = Physics.RaycastAll(ray, distance, dissolveLayerMask, QueryTriggerInteraction.Ignore);

        currentFrameRenderers.Clear();

        foreach (var hit in hits)
        {
            if (hit.transform == player) continue;

            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (!renderer) continue;

            currentFrameRenderers.Add(renderer);

            if (!originalMaterials.TryGetValue(renderer, out var originalMats))
            {
                originalMats = renderer.sharedMaterials;
                originalMaterials[renderer] = originalMats;

                Material[] newMats = new Material[originalMats.Length];
                for (int i = 0; i < originalMats.Length; i++)
                {
                    var originalMat = originalMats[i];
                    var dissolveMat = new Material(dissolveShaderScreenSpace);

                    dissolveMat.SetTexture("_MainTex", originalMat.HasProperty("_MainTex") ? originalMat.GetTexture("_MainTex") : Texture2D.whiteTexture);
                    dissolveMat.SetColor("_Color", originalMat.HasProperty("_Color") ? originalMat.GetColor("_Color") : Color.white);
                    dissolveMat.SetTexture("_DissolveTex", dissolveTexture);
                    dissolveMat.SetTexture("_NoiseTex", noiseTexture);

                    if (originalMat.HasProperty("_NormalMap"))
                        dissolveMat.SetTexture("_NormalMap", originalMat.GetTexture("_NormalMap"));
                    if (originalMat.HasProperty("_RoughnessMap"))
                        dissolveMat.SetTexture("_RoughnessMap", originalMat.GetTexture("_RoughnessMap"));
                    if (originalMat.HasProperty("_HeightMap"))
                        dissolveMat.SetTexture("_HeightMap", originalMat.GetTexture("_HeightMap"));

                    newMats[i] = dissolveMat;
                    createdDissolveMaterials.Add(dissolveMat);
                }
                renderer.materials = newMats;
            }

            foreach (var mat in renderer.materials)
            {
                if (mat.shader == dissolveShaderScreenSpace)
                {
                    mat.SetVector("_PlayerScreenPos", playerScreenPosNormalized);
                    mat.SetFloat("_DissolveScreenRadius", dissolveScreenRadius);
                    mat.SetFloat("_DissolveSoftness", dissolveSoftness);
                    mat.SetFloat("_EdgeWidth", edgeWidth);
                    mat.SetColor("_EdgeColor", edgeColor);
                    mat.SetFloat("_NoiseScale", noiseScale);
                    mat.SetVector("_NoiseSpeed", new Vector4(noiseSpeed.x, noiseSpeed.y, 0, 0));
                    mat.SetFloat("_NoiseEdgeStrength", noiseEdgeStrength);
                }
            }
        }

        renderersToRemove.Clear();
        foreach (var kvp in originalMaterials)
        {
            var renderer = kvp.Key;
            if (!renderer || currentFrameRenderers.Contains(renderer)) continue;

            foreach (var mat in renderer.materials)
            {
                if (createdDissolveMaterials.Contains(mat))
                {
                    Destroy(mat);
                }
            }

            renderer.materials = kvp.Value;
            renderersToRemove.Add(renderer);
        }

        foreach (var renderer in renderersToRemove)
        {
            originalMaterials.Remove(renderer);
        }
    }

    void OnDisable() => RestoreAllMaterials();
    void OnDestroy()
    {
        RestoreAllMaterials();
        foreach (var mat in createdDissolveMaterials)
        {
            if (mat) Destroy(mat);
        }
        createdDissolveMaterials.Clear();
    }

    private void RestoreAllMaterials()
    {
        foreach (var kvp in originalMaterials)
        {
            var renderer = kvp.Key;
            if (!renderer) continue;

            foreach (var mat in renderer.materials)
            {
                if (createdDissolveMaterials.Contains(mat))
                {
                    Destroy(mat);
                }
            }
            renderer.materials = kvp.Value;
        }
        originalMaterials.Clear();
        currentFrameRenderers.Clear();
    }
}
