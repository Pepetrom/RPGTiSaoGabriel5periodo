using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemiesInfo
{
    public GameObject enemyPrefab;
    public string enemyName;
    public PatrolData patrolDatas;
}
public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;
    public EnemiesInfo[] enemiesInfo;
    void SpawnEnemy(PatrolData patrolData, GameObject enemyPrefabs, string name, PatrolData rotation)
    {
        if (patrolData != null)
        {
            GameObject enemy = Instantiate(enemyPrefabs, patrolData.patrolPositions[0], rotation.rotation);
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
    }
    void DestroyAllEnemies()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
    }
    public void AllEnemies()
    {
        DestroyAllEnemies();
        foreach(EnemiesInfo info in enemiesInfo)
        {
            SpawnEnemy(info.patrolDatas, info.enemyPrefab, info.enemyName, info.patrolDatas);
        }
    }
}

