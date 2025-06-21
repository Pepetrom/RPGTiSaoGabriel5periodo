using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthCollider : MonoBehaviour
{
    public KrokodilFSM kro;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") /*&& !kro.hashitted*/)
        {
            kro.hoffMesh.SetActive(true);
            PlayerController.instance.model.gameObject.SetActive(false);
            Invoke("GiveDamage",1);
            kro.grabbed = true;
        }
    }
    void GiveDamage()
    {
        HPBar.instance.TakeDamage(kro.damage, kro.transform);
    }
    private void Update()
    {
        if (kro.grabbed)
        {
            CameraScript.instance.CombatCamera(40, 0.6f, 2.5f);
        }
    }
}
