using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerProjetile : MonoBehaviour
{
    public ParticleSystem particle;
    float timer = 0;
    public float speed, duration;
    public bool endEffect = false, MultiHit = false;
    public SphereCollider colisor;
    private void FixedUpdate()
    {
            timer += Time.fixedDeltaTime;
            transform.position += transform.forward * Time.fixedDeltaTime * speed;
            if (timer >= duration)
            {
                if (endEffect)
                {
                    colisor.enabled = false;
                    particle.Play();
                    Invoke("EndEffect", 0.3f);
                }
                else
                {
                    colisor.enabled = false;
                    EndEffect();
                }
            }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if(colisor.enabled) PlayerController.instance.runes[PlayerController.instance.equipedPrimaryRune].ProjectileHitEffect(other);
            if (!MultiHit)
            {
                colisor.enabled = false;
            }
            if (endEffect)
            {
                particle.Play();
                Invoke("EndEffect", 0.3f);
            }
        }
    }
    void EndEffect()
    {
        Destroy(this.gameObject);
    }
}
