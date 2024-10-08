using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjetile : MonoBehaviour
{
    public ParticleSystem particle;
    float timer = 0;
    public float speed, duration;
    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if( timer >= duration)
        {
            particle.Play();
            Invoke("EndEffect", 0.2f);

        }
        transform.position += transform.forward * Time.fixedDeltaTime * speed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            PlayerController.instance.atacks[0].Hit(other);
            particle.Play();
            Invoke("EndEffect", 0.2f);
        }
    }
    void EndEffect()
    {
        Destroy(this.gameObject);
    }
}
