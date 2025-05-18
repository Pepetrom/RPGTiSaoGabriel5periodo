using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabWallCollider : MonoBehaviour
{
    public CrabFSM crab;
    Collider c;
    public ParticleSystem antecipation;
    private void Start()
    {
        c = GetComponent<Collider>();
        c.enabled = false;
        antecipation.Play();
        Invoke("ActivateMyCollider", 1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HPBar.instance.TakeDamage(30, transform);
            c.enabled = false;
        }
    }
    void ActivateMyCollider()
    {
        c.enabled = true;
    }
}
