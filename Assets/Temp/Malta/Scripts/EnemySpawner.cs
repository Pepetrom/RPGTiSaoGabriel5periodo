using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;
    public GameObject []enemyPrefabs;
    public PatrolData[] patrolDataArray;

    //[SerializeField] Dictionary<>

    void SpawnEnemy(int patrolDataIndex, GameObject enemyPrefabs, string name)
    {
        if (patrolDataIndex >= 0 && patrolDataIndex < patrolDataArray.Length)
        {
            PatrolData patrolData = patrolDataArray[patrolDataIndex];

            GameObject enemy = Instantiate(enemyPrefabs, patrolData.patrolPositions[1], Quaternion.identity);
            switch (name)
            {
                case "turtle":
                    TurtleStateMachine turtleScript = enemy.GetComponent<TurtleStateMachine>();
                    turtleScript.patrolData = patrolData;
                    break;
                case "porquin":
                    PorquinStateMachine porquin = enemy.GetComponent<PorquinStateMachine>();
                    porquin.patrolData = patrolData;
                    break;

            }
        }
        else
        {
            Debug.LogError("Desgrama alada alegrada entristecida");
        }
    }
    void DestroyAllEnemies()
    {
        
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            //Debug.Log(enemies);
            Destroy(enemy.gameObject);
        }
        /*
        for(int i = enemiesInZone.Count; i >= 0; i--)
        {
            RemoveEnemy(enemiesInZone[i]);
        }*/
    }
    public void AllEnemies()
    {
        DestroyAllEnemies();
        SpawnEnemy(0, enemyPrefabs[1], "porquin");
        SpawnEnemy(1, enemyPrefabs[1], "porquin");
        SpawnEnemy(2, enemyPrefabs[1], "porquin");
        SpawnEnemy(3, enemyPrefabs[1], "porquin");
        SpawnEnemy(4, enemyPrefabs[0], "turtle");
        SpawnEnemy(5, enemyPrefabs[0], "turtle");
        SpawnEnemy(6, enemyPrefabs[1], "porquin");
        SpawnEnemy(7, enemyPrefabs[0], "turtle");
        SpawnEnemy(8, enemyPrefabs[1], "porquin");
    }
    /*
    public void RegisterEnemy(Enemy target)
    {
        enemiesInZone.Add(target);
    }
    public void RemoveEnemy(Enemy target)
    {
        enemiesInZone.Remove(target);
        Destroy(target.gameObject);
    }*/
}

