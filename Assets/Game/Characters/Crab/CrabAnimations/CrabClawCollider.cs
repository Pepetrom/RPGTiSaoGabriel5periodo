using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabClawCollider : MonoBehaviour
{
    public CrabFSM crab;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !crab.hashitted)
        {
            HPBar.instance.TakeDamage(crab.damage, crab.transform);
            crab.claw1.enabled = false;
            crab.claw2.enabled = false;
            crab.hashitted = true;
        }
    }
}
