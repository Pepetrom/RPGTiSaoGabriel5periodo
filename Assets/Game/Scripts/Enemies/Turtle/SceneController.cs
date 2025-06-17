using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController sceneController;
    public GameObject tutorial, pageA, pageB, options, bonfire, pause, changeLog, videoPanel, audioPanel, geral, travelPanel;
    public GameObject runePanel, cheatMenu, commands;
    public GameObject botaoContinuar;
    bool naoPause = false;
    private void Awake()
    {
        sceneController = this;
    }
    private void Start()
    {
        commands.SetActive(SaveLoad.instance.saveData.player.commandsOpen);
        SaveLoad.instance.ShowContinuar(botaoContinuar);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OptionsPanel();
            BonfirePanel();
            RunesPanel();
            VideoPanel();
            AudioPanel();
            TravelPanel();
            Pause();
            geral.SetActive(true);
            naoPause = false;
        }
        TutorialPanel();
    }
    public void OpenCheatMenu(bool open)
    {
        cheatMenu.SetActive(open);
        SaveLoad.instance.saveData.player.commandsOpen = commands.activeSelf;
    }
    public void OpenCommands(bool open)
    {
        commands.SetActive(open);
    }
    #region cheats
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
    public void AddMoney()
    {
        GameManager.instance.Score(99999);
    }
    #endregion
    void Pause()
    {
        if (pause.activeSelf)
        {
            pause.SetActive(false);
            PlayerController.instance.ResetAllActions();
            GameManager.instance.UnPause();
        }
        else if (!naoPause)
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
            //PlayerController.instance.ResetAllActions();
            naoPause = true;
            //GameManager.instance.UnPause();
        }
    }
    void BonfirePanel()
    {
        if (bonfire.activeSelf)
        {
            bonfire.SetActive(false);
            naoPause = true;
            if (!runePanel.activeSelf && !travelPanel.activeSelf)
            {
                PlayerController.instance.ResetAllActions();
                GameManager.instance.UnPause();
            }
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
            bonfire.SetActive(true);
            naoPause = true;
            //PlayerController.instance.ResetAllActions();
            //GameManager.instance.UnPause();
        }
    }
    public void VideoPanel()
    {
        if (videoPanel.activeSelf)
        {
            videoPanel.SetActive(false);
            naoPause = true;
            //PlayerController.instance.ResetAllActions();
            //GameManager.instance.UnPause();
        }
    }
    public void AudioPanel()
    {
        if (audioPanel.activeSelf)
        {
            audioPanel.SetActive(false);
            naoPause = true;
            //PlayerController.instance.ResetAllActions();
            //GameManager.instance.UnPause();
        }
    }
    public void TravelPanel()
    {
        Debug.Log("AUI");
        if (travelPanel.activeSelf)
        {
            travelPanel.SetActive(false);
            bonfire.SetActive(true);
            naoPause = true;
            //PlayerController.instance.ResetAllActions();
            //GameManager.instance.UnPause();
        }
    }
}
