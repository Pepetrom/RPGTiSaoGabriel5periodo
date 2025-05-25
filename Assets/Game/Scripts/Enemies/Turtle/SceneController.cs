using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController sceneController;
    public GameObject tutorial, pageA, pageB, options, bonfire, pause, changeLog, videoPanel, geral;
    public GameObject runePanel, cheatMenu;
    private void Awake()
    {
        sceneController = this;
    }
    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
            geral.SetActive(true);
            if (pause.activeSelf) return;
            OptionsPanel();
            BonfirePanel();
            RunesPanel();
            VideoPanel();
        }
        TutorialPanel();
    }
    public void OpenCheatMenu(bool open)
    {
        cheatMenu.SetActive(open);
    }
    public void Imortal()
    {
        PlayerController.instance.imortal = !PlayerController.instance.imortal;
    }
    public void KillPlayer()
    {
        PlayerController.instance.imortal = false;
        PlayerController.instance.canTakeDamage = true;
        HPBar.instance.Die();
    }
    void Pause()
    {
        if (pause.activeSelf)
        {
            Debug.Log("Foi isso");
            pause.SetActive(false);
            PlayerController.instance.ResetAllActions();
            GameManager.instance.UnPause();
        }
        else if (!bonfire.activeSelf && !options.activeSelf && !pause.activeSelf && !runePanel.activeSelf)
        {
            pause.SetActive(true);
            PlayerController.instance.StopAllActions();
            GameManager.instance.Pause();
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
                GameManager.instance.UnPause();
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
            options.SetActive(false);
            PlayerController.instance.ResetAllActions();
            GameManager.instance.UnPause();
        }
    }
    void BonfirePanel()
    {
        if (bonfire.activeSelf)
        {
            bonfire.SetActive(false);
            PlayerController.instance.ResetAllActions();
            GameManager.instance.UnPause();
        }
    }
    public void OpenRunePanel()
    {
        GameManager.instance.Pause();
        PlayerController.instance.StopAllActions();
        runePanel.SetActive(true);
    }
    public void RunesPanel()
    {
        if (runePanel.activeSelf)
        {
            runePanel.SetActive(false);
            PlayerController.instance.ResetAllActions();
            GameManager.instance.UnPause();
        }
    }
    public void VideoPanel()
    {
        if (videoPanel.activeSelf)
        {
            videoPanel.SetActive(false);
            PlayerController.instance.ResetAllActions();
            GameManager.instance.UnPause();
        }
    }
}
