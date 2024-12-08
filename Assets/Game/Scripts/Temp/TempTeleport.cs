using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempTeleport : MonoBehaviour
{
    public Transform whereToGo;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.instance.cc.enabled = false;
            PlayerController.instance.transform.position = whereToGo.transform.position;
            PlayerController.instance.cc.enabled = true;

        }
    }
}
