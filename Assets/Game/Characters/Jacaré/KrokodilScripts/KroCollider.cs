using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroCollider : MonoBehaviour
{
    public KrokodilFSM kro;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") /*&& !kro.hashitted*/)
        {
            HPBar.instance.TakeDamage(kro.damage, kro.transform);
        }
    }
}