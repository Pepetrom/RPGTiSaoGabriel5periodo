using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRightHand : MonoBehaviour
{
    public Player player;
    public BossStateMachine boss;
    private void OnCollisionEnter(Collision collision)
    {
        //O attHit e o colisor precisam ir para false quando o boss acerta o player para que não haja ataques consecutivos no mesmo golpe
        if (collision.gameObject.CompareTag("Player") && boss.attHit == true)
        {
            HPBar.instance.TakeDamage(boss.damage, this.transform);
            Debug.Log("Tomou porrada");
            boss.rightHand.enabled = false;
            boss.attHit = false;
        }
    }
}
