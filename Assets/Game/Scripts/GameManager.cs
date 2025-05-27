using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject damagePopUp;
    public static GameManager instance;
    public float actionTime = 1;
    public bool isCombat = false;
    //SpawnPoint/Checkpoint
    public Transform lastBonfireRestedAt;

    //Runes 
    public float skillPoints;
    public GameObject bonfire;
    public bool tutorial;

    public bool spawnEnemies = true;

    private List<GameObject> enemiesInCombat = new List<GameObject>(); // Lista para checar inimigos no combate
    public EnemySpawner enemySpawner;
    //public bool canFade = false;

    public AudioManager audioMan;
    public bool isOnWater;

    private void Awake()
    {
        instance = this;
        UpdateActionTime(1);
        UnPause(); 
    }
    private void Start()
    {
        tutorial = false;
        skillPoints = SaveLoad.instance.saveData.player.skillPoints;
        UIItems.instance.UpdateScoreQUI(skillPoints);
        if (!spawnEnemies) return;
        enemySpawner.AllEnemies();
    }
    public void Pause()
    {
        Time.timeScale = 0;
    }
    public void UnPause()
    {
        Time.timeScale = 1;
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
        UpdateActionTime(0);
        CameraScript.instance.StartShake();
        yield return new WaitForSeconds(tempoHitStop);
        UpdateActionTime(0.5f);
        yield return new WaitForSeconds(tempoHitStop);
        UpdateActionTime(1);
    }
    public void UpdateActionTime(float value)
    {
        actionTime = value;
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
    public void Bonfire(bool open)
    {
        bonfire.SetActive(open);
        if (open)
        {
            PlayerController.instance.StopAllActions();
            Pause();
        }
        else
        {
            PlayerController.instance.ResetAllActions();
            UnPause();
        }
    }
    public void Travel(Transform location)
    {
        PlayerController.instance.cc.enabled = false;
        PlayerController.instance.transform.position = location.position;
        PlayerController.instance.cc.enabled = true;
    }
    public void Respawn()
    {
        UIItems.instance.PlayerIsDead();
        if (UIItems.instance.gearEnd) UIItems.instance.GearLoopAnimation(true);
        if (UIItems.instance.deathAnimationIsOver)
        {
            Debug.Log(UIItems.instance.deathAnimationIsOver);
            UIItems.instance.RespawnButton();
        }
        PlayerController.instance.ForceIddle();
    }
    public void Rest()
    {
        Estus.instance.ResetEstus();
        HPBar.instance.ResetBar();
        enemySpawner.AllEnemies();
        Bonfire(false);
        UIItems.instance.deathAnimationIsOver = false;
        UIItems.instance.gearEnd = false;
        isOnWater = false;
    }
    public void ResetPositionPlayer() 
    {
        PlayerController.instance.moveDirection = Vector3.zero;
        PlayerController.instance.cc.enabled = false;
        PlayerController.instance.transform.position = lastBonfireRestedAt.position;
        PlayerController.instance.cc.enabled = true;
        PlayerController.instance.moveDirection = Vector3.zero;
    }
    public void Score(int amount)
    {
        skillPoints += amount;
        UIItems.instance.UpdateScoreQUI(skillPoints);
        SaveLoad.instance.saveData.player.skillPoints = skillPoints;
        SaveLoad.instance.Save();
    }
}
