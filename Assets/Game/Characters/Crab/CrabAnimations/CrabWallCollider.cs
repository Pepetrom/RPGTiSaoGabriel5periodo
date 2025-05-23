using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CrabWallCollider : MonoBehaviour
{
    public CrabFSM crab;
    Collider c;
    public GameObject antecipation, wallFire;
    public SkinnedMeshRenderer mesh;
    private void Start()
    {
        wallFire.GetComponent<VisualEffect>().Stop();
        c = GetComponent<Collider>();
        c.enabled = false;
        mesh.enabled = false;
        antecipation.GetComponent<VisualEffect>().Play();
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
        wallFire.GetComponent<VisualEffect>().Play();
        c.enabled = true;
        mesh.enabled = true;
    }
}
