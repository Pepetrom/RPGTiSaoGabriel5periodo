using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEscudoCollider : MonoBehaviour
{
    public PorquinEscudoFSM porquin;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !porquin.hashitted)
        {
            PlayerController.instance.ResetAllActions();
            HPBar.instance.TakeDamage(porquin.damage, porquin.transform);
            porquin.hashitted = true;
        }
    }
}
