using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabWallCollider : MonoBehaviour
{
    public CrabFSM crab;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            crab.canDoFireDamage = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            crab.canDoFireDamage = false;
        }
    }
    private void Update()
    {
        if (crab.canDoFireDamage)
        {
            HPBar.instance.TakeDamage(crab.damage, transform);
            crab.hashitted = true;
        }
    }
}
