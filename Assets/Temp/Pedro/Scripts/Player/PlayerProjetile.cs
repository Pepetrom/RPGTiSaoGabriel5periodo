using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjetile : MonoBehaviour
{
    public ParticleSystem particle;
    float timer = 0;
    public float speed, duration;
    public bool endEffect = false;
    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if( timer >= duration)
        {
            if (endEffect)
            {
                particle.Play();
            }
            Invoke("EndEffect", 0.2f);
        }
        transform.position += transform.forward * Time.fixedDeltaTime * speed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().TakeDamage(PlayerController.instance.baseDamage, PlayerController.instance.comboCounter);
            HPBar.hpbarInstance.RecoverLifebyHit(PlayerController.instance.baseDamage / 10);
            if (endEffect)
            {
                particle.Play();
                Invoke("EndEffect", 0.2f);
            }
        }
    }
    void EndEffect()
    {
        Destroy(this.gameObject);
    }
}
