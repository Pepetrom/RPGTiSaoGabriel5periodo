using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerProjetile : MonoBehaviour
{
    public ParticleSystem endParticle;
    public GameObject endParticleGameObj;
    float timer = 0;
    public float speed, duration, endEffectTimer = 0.3f;
    public bool hasEndEffect = false, MultiHit = false, endEffectDoesDamage = false;
    bool ended = false;
    public SphereCollider colisor;
    
    private void FixedUpdate() {
        if( ended ) return;
        timer += Time.fixedDeltaTime;
        if (timer >= duration) {
            FinalPartEffect();
        }
        else {
            transform.position += transform.forward * Time.fixedDeltaTime * speed;
        }
    }
    void FinalPartEffect()
    {
        ended = true;
        if (hasEndEffect)
        {
            if(!endEffectDoesDamage) colisor.enabled = false;

            if (endParticleGameObj) endParticleGameObj.SetActive(true);
            if (endParticle) endParticle.Play();
            Invoke("EndEffect", endEffectTimer);
        }
        else
        {
            colisor.enabled = false;
            EndEffect();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if(colisor.enabled) PlayerController.instance.runes[PlayerController.instance.equipedPrimaryRune].ProjectileHitEffect(other);
            if (!MultiHit)
            {
                FinalPartEffect();
            }
        }
    }
    void EndEffect()
    {
        Destroy(this.gameObject);
    }
}
