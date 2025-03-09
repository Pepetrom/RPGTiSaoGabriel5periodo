using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public GameObject loadingScene;
    public GameObject button;
    public Slider loadingFill;
    public GameObject tutorial, pageA, pageB, options;

    private void Update()
    {
        TutorialPanel();
        OptionsPanel();
    }
    public void LoadingSceneActive(string name)
    {
        StartCoroutine(LoadSceneAsync(name));
    }
    IEnumerator LoadSceneAsync(string name)
    {
        AsyncOperation o = SceneManager.LoadSceneAsync(name);
        loadingScene.SetActive(true);
        while (!o.isDone)
        {
            float loadProgress = Mathf.Clamp01(o.progress/0.2f);
            loadingFill.value = loadProgress;
            yield return null;
        }
        ShowButton();
    }
    void ShowButton()
    {
        button.SetActive(true);
    }
    void TutorialPanel()
    {
        if (tutorial.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (pageA.activeSelf)
                {
                    pageA.SetActive(false);
                    pageB.SetActive(true);
                }
                else
                {
                    pageB.SetActive(false);
                    pageA.SetActive(true);
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                tutorial.SetActive(false);
            }
        }
    }
    void OptionsPanel()
    {
        if (options.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                options.SetActive(false);
            }
        }
    }
}
