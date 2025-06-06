using System.Collections;
using UnityEngine;

public class LoadSword : MonoBehaviour
{
    public static LoadSword instance;
    public Renderer swordRenderer;
    public string pathMaterials = "MateriaisSword";
    public string[] pathIndividualMaterials = new string[4];
    public Material[] swordMaterials = new Material[4];
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        LoadTheSword();
    }
    public void ChangeSword(int which)
    {
        swordRenderer.material = swordMaterials[which];
    }
    public void LoadTheSword()
    {
        StartCoroutine(LoadTheMaterial(pathMaterials));
    }
    IEnumerator LoadTheMaterial(string resourcePath)
    {
        ResourceRequest resourceRequest = Resources.LoadAsync<Material>(resourcePath + pathIndividualMaterials[0]);
        for (int i = 0; i < swordMaterials.Length; i++) {
            resourceRequest = Resources.LoadAsync<Material>(resourcePath + pathIndividualMaterials[i]);
            while (!resourceRequest.isDone)
            {
                Debug.Log("Processo carregamento " + resourceRequest.progress + $" do item { i }");
                yield return null;
            }
            swordMaterials[i] = resourceRequest.asset as Material;
        }
        yield return true;
    }
}
