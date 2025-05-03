using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabTeleporter : MonoBehaviour
{
    public Transform whereToGo;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BossManager.instance.StartBossFight(BossManager.instance.bosses[0], BossManager.instance.positions[0]);
            PlayerController.instance.cc.enabled = false;
            PlayerController.instance.transform.position = whereToGo.transform.position;
            PlayerController.instance.cc.enabled = true;
        }
    }
}
