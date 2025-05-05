using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController sceneController;
    public GameObject tutorial, pageA, pageB, options, bonfire, pause, changeLog;
    private void Awake()
    {
        sceneController = this;
    }
    private void Start()
    {
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !bonfire.activeSelf && !options.activeSelf && !pause.activeSelf)
        {
            pause.SetActive(true);
            PlayerController.instance.StopAllActions();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pause.activeSelf)
        {
            pause.SetActive(false);
            PlayerController.instance.ResetAllActions();
        }
        TutorialPanel();
        OptionsPanel();
        BonfirePanel();
        GameManager.instance.ExitAllMenus();

    }
    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void Quit()
    {
        Application.Quit();
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
    public void Version()
    {
        changeLog.SetActive(!changeLog.activeSelf);
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
    void BonfirePanel()
    {
        if (bonfire.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                bonfire.SetActive(false);
                PlayerController.instance.ResetAllActions();
            }
        }
    }
}
