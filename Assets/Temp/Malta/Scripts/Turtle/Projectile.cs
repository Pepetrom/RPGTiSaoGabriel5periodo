using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public ProjectilesSO p;
    private void Update()
    {
        transform.Translate(Vector3.forward * p.speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HPBar.instance.TakeDamage(p.damage, this.transform);
        }
    }
}
