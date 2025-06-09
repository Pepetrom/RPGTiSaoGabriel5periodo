using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabTeleporter : MonoBehaviour
{
    public Transform whereToGo;
    public int index;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(index == 0)
            {
                UIItems.instance.FadeInFadeOut(true);
                Invoke("SpawnBoss", 2);
                PlayerController.instance.cc.enabled = false;
                PlayerController.instance.transform.position = whereToGo.transform.position;
                PlayerController.instance.cc.enabled = true;
            }
            else
            {
                SpawnBoss();
                Destroy(gameObject);
            }
        }
    }
    void SpawnBoss()
    {
        BossManager.instance.StartBossFight(BossManager.instance.bosses[index], BossManager.instance.positions[index]);
    }
}
