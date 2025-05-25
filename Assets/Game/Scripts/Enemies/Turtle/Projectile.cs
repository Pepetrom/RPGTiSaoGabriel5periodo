using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public ProjectilesSO p;
    private void Start()
    {
        Invoke("Destroy", 3);
    }
    private void Update()
    {
        transform.Translate(Vector3.forward * p.speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Colidiu");
            HPBar.instance.TakeDamage(p.damage, this.transform);
            Destroy();
            return;
        }
        if(other.CompareTag("Scenario"))
        {
            Destroy();
        }
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }
}
