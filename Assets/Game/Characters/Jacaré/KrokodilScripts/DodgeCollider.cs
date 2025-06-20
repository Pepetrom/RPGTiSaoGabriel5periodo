using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeCollider : MonoBehaviour
{
    public KrokodilFSM kro;

    private void OnTriggerEnter(Collider other)
    {
        if (CompareTag("fog"))
        {
            kro.canDash = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (CompareTag("fog"))
        {
            kro.canDash = true;
        }
    }
}
