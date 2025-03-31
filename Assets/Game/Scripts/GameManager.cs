using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject damagePopUp;
    public static GameManager instance;
    public float actionTime = 1;
    public bool isCombat = false;
    public GameObject runesPanel;
    public bool[] unlockedRunes = new bool[4];
    //SpawnPoint/Checkpoint
    public Transform lastBonfireRestedAt;

    //Runes 
    public float skillPoints;
    public GameObject runePage;
    public GameObject bonfire;
    public GameObject tutorialPanel;
    public bool tutorial;
    public int score;

    private List<GameObject> enemiesInCombat = new List<GameObject>(); // Lista para checar inimigos no combate
    public EnemySpawner enemySpawner;
    //public bool canFade = false;

    public AudioManager audioMan;

    private void Awake()
    {
        instance = this;
        actionTime = 1;
    }
    private void Start()
    {
        enemySpawner.AllEnemies();
        tutorialPanel.gameObject.SetActive(false);
        tutorial = false;
        UIItems.instance.UpdateScoreQUI(0);
        UIItems.instance.UpdateSkillPoints();
    }
    public void SpawnNumber(int damageNumber, Color color, Transform targetLocation)
    {
        GameObject temp = Instantiate(damagePopUp, targetLocation.position, transform.rotation);
        DamagePopUp dmp = temp.GetComponent<DamagePopUp>();
        dmp.StartNumber(damageNumber, color);
    }
    public void CallHitStop(float tempo)
    {
        StartCoroutine(HitStop(tempo));
    }
    IEnumerator HitStop(float tempoHitStop)
    {
        actionTime = 0;
        CameraScript.instance.StartShake();
        yield return new WaitForSeconds(tempoHitStop);
        actionTime = 1;
    }
    public void StartCombat()
    {
        isCombat = true;
    }
    public void EndCombat()
    {
        isCombat = false;
    }
    public void CheckEnemiesOnList()
    {
        if(enemiesInCombat.Count > 0)
        {
            StartCombat();
        }
        else
        {
            EndCombat();
        }
    }
    public void AddEnemy(GameObject enemy)
    {
        if (!enemiesInCombat.Contains(enemy))
        {
            enemiesInCombat.Add(enemy);
            CheckEnemiesOnList();
        }
    }
    public void RemoveEnemy(GameObject enemy)
    {
        if (enemiesInCombat.Contains(enemy))
        {
            enemiesInCombat.Remove(enemy);
            CheckEnemiesOnList();
        }
    }
    //TEMPORÁRIO
    public void RunesPanel()
    {
        if (!runesPanel.activeSelf)
        {
            PlayerController.instance.StopAllActions();
            runesPanel.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            runesPanel.SetActive(false);
            Time.timeScale = 1;
            PlayerController.instance.ResetAllActions();
        }
    }
    public void AddSkillPoints()
    {
        skillPoints += 5000;
        UIItems.instance.UpdateSkillPoints();
    }
    public void OpenRunes(bool open)
    {
        runePage.SetActive(open);
        if (open)
        {
            actionTime = 0;
            bonfire.SetActive(false);
            PlayerController.instance.StopAllActions();
        }
        else
        {
            actionTime = 1;
            PlayerController.instance.ResetAllActions();
        }
    }
    public void Bonfire(bool open)
    {
        bonfire.SetActive(open);
        if (open)
        {
            PlayerController.instance.StopAllActions();
            actionTime = 0;
        }
        else
        {
            actionTime = 1;
            PlayerController.instance.ResetAllActions();
        }
    }
    public void Respawn()
    {
        PlayerController.instance.cc.enabled = false;
        PlayerController.instance.transform.position = lastBonfireRestedAt.position;
        Rest();
        PlayerController.instance.cc.enabled = true;
    }
    public void Rest()
    {
        Estus.instance.flaskQuantity = Estus.instance.maxFlaskQuantity;
        UIItems.instance.UpdateChesseQUI(Estus.instance.flaskQuantity);
        HPBar.instance.currentHP = HPBar.instance.maxHP;
        enemySpawner.AllEnemies();
        PlayerController.instance.ResetAllActions();
        ScreenFade.instance.StartFadeToBlackAndBack();
        Bonfire(false);
    }
    public void Score(int amount)
    {
        skillPoints += amount;
        UIItems.instance.UpdateSkillPoints();
        UIItems.instance.UpdateScoreQUI(skillPoints);
    }
}
