using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtackCollider : MonoBehaviour
{
    public int slot;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            PlayerController.instance.atacks[slot].Hit(other);
        }
        else if (other.CompareTag("Destroyable"))
        {
            other.GetComponent<DestroyableObjects>().Die();
        }
    }
}
