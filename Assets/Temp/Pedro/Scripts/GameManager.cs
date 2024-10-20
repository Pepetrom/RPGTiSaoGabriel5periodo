using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject damagePopUp;
    public static GameManager instance;
    public float actionTime = 1;
    public bool isCombat = false;

    private List<GameObject> enemiesInCombat = new List<GameObject>(); // Lista para checar inimigos no combate
    private void Awake()
    {
        instance = this;
        actionTime = 1;
    }
    public void SpawnNumber(int damageNumber, Color color, Transform targetLocation)
    {
        GameObject temp = Instantiate(damagePopUp, targetLocation.position, transform.rotation);
        DamagePopUp dmp = temp.GetComponent<DamagePopUp>();
        dmp.StartNumber(damageNumber, color);
    }
    public void ChangeRune(int rune)
    {
        PlayerController.instance.actualRune = rune;
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
}
