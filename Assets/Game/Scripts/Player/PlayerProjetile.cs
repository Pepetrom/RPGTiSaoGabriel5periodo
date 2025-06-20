using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerProjetile : MonoBehaviour
{
    public ParticleSystem particle;
    float timer = 0;
    public float speed, duration, endEffectTimer = 0.3f;
    public bool endEffect = false, MultiHit = false;
    public SphereCollider colisor;
    private void FixedUpdate() {
        timer += Time.fixedDeltaTime;
        if (timer >= duration) {
            if (endEffect) {
                colisor.enabled = false;
                if (particle) particle.Play();
                Invoke("EndEffect", endEffectTimer);
            }
            else {
                colisor.enabled = false;
                EndEffect();
            }
        }
        else {
            transform.position += transform.forward * Time.fixedDeltaTime * speed;
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
