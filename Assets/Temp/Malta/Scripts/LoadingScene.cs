using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public GameObject loadingScene;
    public GameObject tutorial, pageA, pageB, options;
    //public GameObject button;
    public Slider loadingFill;
    public void Update()
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
        //ShowButton();
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
                Debug.Log("Isso aqui ta funfando?");
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
    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void Quit()
    {
        Application.Quit();
    }
    /*void ShowButton()
    {
        button.SetActive(true);
    }*/
}
