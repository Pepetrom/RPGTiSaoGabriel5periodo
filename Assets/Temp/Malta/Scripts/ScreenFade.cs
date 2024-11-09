using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    public Image blackImage;
    public float fadeDuration;
    public static ScreenFade instance;
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
    private void Update()
    {
        if (GameManager.instance.canFade)
        {
            StartFadeToBlackAndBack();
        }
    }
    public void StartFadeToBlackAndBack()
    {
        StartCoroutine(FadeToBlackAndBack());
    }
    private IEnumerator FadeToBlackAndBack()
    {
        yield return StartCoroutine(Fade(1));
        yield return new WaitForSeconds(0.8f);
        GameManager.instance.canFade = false;
        yield return StartCoroutine(Fade(0));
    }
    private IEnumerator Fade(float target)
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
