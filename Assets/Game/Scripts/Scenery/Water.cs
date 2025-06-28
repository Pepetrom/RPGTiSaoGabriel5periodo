using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HPBar.instance.FallDamage(9999f);
        }
    }
    /*private void Update()
    {
        if(GameManager.instance.isOnWater)
        {
            //GameManager.instance.Respawn();
            PlayerController.instance.Die();
        }
    }*/
}
