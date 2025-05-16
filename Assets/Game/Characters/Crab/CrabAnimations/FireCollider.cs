using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCollider : MonoBehaviour
{
    public CrabFSM crab;
    Collider c;
    public ParticleSystem antecipation;
    private void Start()
    {
        c = GetComponent<Collider>();
        c.enabled = false;
        Invoke("ActivateMyCollider",1f);
    }
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
            HPBar.instance.TakeDamage(crab.damage, crab.transform);
            crab.hashitted = true;
        }
    }
    void ActivateMyCollider()
    {
        c.enabled = true;
        antecipation.Play();
    }
}
