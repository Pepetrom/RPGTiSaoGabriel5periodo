using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDisssolveShader : MonoBehaviour
{
    public float fadeSpeed, fadeValue;
    float opacity;
    Material m;
    public bool CanFade;
    

    void Start()
    {
        m = GetComponent<Renderer>().material;
        opacity = m.color.a;
    }
    void Update()
    {
        if (CanFade)
            Fade();
        else
            ResetFade();
    }
    void Fade()
    {
        Color currentColor = m.color;
        Color FinalColor = new Color(currentColor.r,currentColor.g, currentColor.b,Mathf.Lerp(currentColor.a,fadeValue,fadeSpeed * Time.deltaTime));
        m.color = FinalColor;   
    }
    void ResetFade()
    {
        Color currentColor = m.color;
        Color FinalColor = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Lerp(currentColor.a, opacity, fadeSpeed * Time.deltaTime));
        m.color = FinalColor;
    }
}
