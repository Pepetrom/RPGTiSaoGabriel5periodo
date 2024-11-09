using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    public GameObject turtlePrefab;
    public PatrolData[] patrolDataArray;

    void SpawnEnemy(int patrolDataIndex)
    {
        if (patrolDataIndex >= 0 && patrolDataIndex < patrolDataArray.Length)
        {
            PatrolData patrolData = patrolDataArray[patrolDataIndex];

            GameObject enemy = Instantiate(turtlePrefab, patrolData.patrolPositions[1], Quaternion.identity);
            TurtleStateMachine turtleScript = enemy.GetComponent<TurtleStateMachine>();
            turtleScript.patrolData = patrolData;
        }
        else
        {
            Debug.LogError("Desgrama alada alegrada entristecida");
        }
    }
    public void AllEnemies()
    {
        SpawnEnemy(0);
        SpawnEnemy(1);
    }
}

