using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager instance;
    public GameObject[] bosses;
    public Transform[] positions;
    public bool antiqueFurnace = true, krokodil = true;
    public GameObject[] spawns;
    public GameObject wall1, wall2;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        krokodil = true;
        antiqueFurnace = true;
    }
    public void StartBossFight(GameObject boss, Transform bossLocation)
    {
        Instantiate(boss, bossLocation.position, bossLocation.rotation);
    }
    public void CheckIfBossWasKilled()
    {
        UIItems.instance.ShowBOSSHUD(false);
        KrokodilSetUp(false);
        if (krokodil)
        {
            spawns[1].SetActive(true);
        }
        else
        {
            spawns[1].SetActive(false);
        } 
    }
    public void KrokodilSetUp(bool state)
    {
        wall1.SetActive(state);
        wall2.SetActive(state);
    }
}
