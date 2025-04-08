using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    public Image blackImage;
    public static ScreenFade instance;
    bool canFade = true;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Color color = blackImage.color;
        color.a = 0f;
        blackImage.color = color;
    }
    public void StartFadeToBlackAndBack()
    {
        StartCoroutine(FadeToBlackAndBack());
    }
    private IEnumerator FadeToBlackAndBack()
    {
        if (canFade)
        {
            canFade = false;
            yield return StartCoroutine(Fade(1, 0.1f));
            yield return new WaitForSeconds(0.8f);
            yield return StartCoroutine(Fade(0,1));
            canFade = true;
        }
    }
    private IEnumerator Fade(float target, float fadeDuration)
    {
        float alpha = blackImage.color.a;
        float e = 0;
        while(e < fadeDuration)
        {
            e += Time.deltaTime;
            float alphaLerped = Mathf.Lerp(alpha,target,e/fadeDuration);
            Color color = blackImage.color;
            color.a = alphaLerped;
            blackImage.color= color;
            yield return null;
        }
    }
}
