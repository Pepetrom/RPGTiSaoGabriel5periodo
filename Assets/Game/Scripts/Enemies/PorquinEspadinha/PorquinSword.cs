using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorquinSword : MonoBehaviour
{
    public PorquinStateMachine porquin;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !porquin.hashitted)
        {
            HPBar.instance.TakeDamage(porquin.damage, porquin.transform);
            porquin.sword.gameObject.SetActive(false);
            porquin.hashitted = true;
        }
    }

}
