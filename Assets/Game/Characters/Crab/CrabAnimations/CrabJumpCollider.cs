using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabJumpCollider : MonoBehaviour
{
    public CrabFSM crab;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !crab.hashitted)
        {
            HPBar.instance.TakeDamage(crab.damage, crab.transform);
            crab.jumpCollider.enabled = false;
            crab.hashitted = true;
        }
    }
}
