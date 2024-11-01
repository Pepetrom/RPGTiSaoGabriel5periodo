using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int lifeTotal;
    int lifeActual;
    public Slider hpbar;
    public float lerpSpeed;
    public TurtleStateMachine turtle;
    //float knockbackTime = 0;
    //Vector3 knockBackDirection;
    //bool playhit = false;
    public bool playerHit = false;
    public ParticleSystem hit;
    private void Start()
    {
        lifeActual = lifeTotal;
        hpbar.maxValue = lifeTotal;
        hpbar.value = lifeTotal;
    }
    private void FixedUpdate()
    {
        UpdateHPBar();
    }
    public void TakeDamage(int damage, float knockbackStrenght)
    {
        //Por favor, implementar o estado de levando dano do inimigo e o knockback
        turtle?.Impulse(turtle.kbforce * knockbackStrenght);
        lifeActual -= damage;
        playerHit = true;
        hit.Play();
        GameManager.instance.SpawnNumber((int)damage, Color.yellow, transform);
        if (lifeActual <= 0)
        {
            turtle?.animator.SetBool("Dead", true);
            turtle?.Die();

        }
    }
    public void UpdateHPBar()
    {
        hpbar.value = Mathf.Lerp(hpbar.value, lifeActual, lerpSpeed);
    }
}
