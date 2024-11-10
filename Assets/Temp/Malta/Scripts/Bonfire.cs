using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    public GameObject []enemyPrefabs;
    public PatrolData[] patrolDataArray;

    void SpawnEnemy(int patrolDataIndex, GameObject enemyPrefabs)
    {
        if (patrolDataIndex >= 0 && patrolDataIndex < patrolDataArray.Length)
        {
            PatrolData patrolData = patrolDataArray[patrolDataIndex];

            GameObject enemy = Instantiate(enemyPrefabs, patrolData.patrolPositions[1], Quaternion.identity);
            TurtleStateMachine turtleScript = enemy.GetComponent<TurtleStateMachine>();
            turtleScript.patrolData = patrolData;
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
            Debug.Log(enemies);
            Destroy(enemy.gameObject);
        }
    }
    public void AllEnemies()
    {
        DestroyAllEnemies();
        SpawnEnemy(0, enemyPrefabs[0]);
        SpawnEnemy(1, enemyPrefabs[1]);
    }
    
}

