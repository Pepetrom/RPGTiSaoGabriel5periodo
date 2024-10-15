using UnityEngine;

public class ObjectDissolver : MonoBehaviour
{
    public float fadeSpeed = 1f, fadeValue = 0f;
    Material m;
    public bool CanFade;
    float dissolveAmount;

    void Start()
    {
        m = GetComponent<Renderer>().sharedMaterial; // Use sharedMaterial para evitar cópia do material
        dissolveAmount = m.GetFloat("_Cutoff"); // Inicia com o valor atual de dissolução
        m.SetFloat("_Cutoff", dissolveAmount);  // Aplica o valor no material
    }

    void Update()
    {
        Debug.Log("Current Dissolve Value: " + m.GetFloat("_Cutoff")); // Mostra o valor atual de dissolve
        if (CanFade)
            Fade();
        else
            ResetFade();
    }

    void Fade()
    {
        dissolveAmount = Mathf.Lerp(dissolveAmount, fadeValue, fadeSpeed * Time.deltaTime);
        m.SetFloat("_Cutoff", dissolveAmount); // Atualiza o valor de dissolve no material
    }

    void ResetFade()
    {
        dissolveAmount = Mathf.Lerp(dissolveAmount, 0, fadeSpeed * Time.deltaTime);
        m.SetFloat("_Cutoff", dissolveAmount); // Reseta o valor de dissolve
    }
}
