using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           GameManager.instance.isOnWater = true;
        }
    }
    private void Update()
    {
        if(GameManager.instance.isOnWater)
            GameManager.instance.Respawn();
    }
}
