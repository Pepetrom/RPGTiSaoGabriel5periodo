using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager instance;
    public GameObject[] bosses;
    public Transform[] positions;
    private void Awake()
    {
        instance = this;
    }
    public void StartBossFight(GameObject boss, Transform bossLocation)
    {
        Instantiate(boss, bossLocation.position, bossLocation.rotation);
    }
}
