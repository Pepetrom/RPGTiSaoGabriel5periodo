using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int lifeTotal;
    public int lifeActual;
    public Slider hpbar;
    public float lerpSpeed;
    public TurtleStateMachine turtle;
    float knockbackTime = 0;
    Vector3 knockBackDirection;
    public bool playerHit = false;
    public ParticleSystem hit;
    bool playhit = false;

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
    public void TakeDamage(int damage, int knockbackStrenght)
    {
        turtle.Impulse(turtle.kbforce);
        lifeActual -= damage;
        GameManager.instance.SpawnNumber((int)damage, Color.yellow, transform);
        hit.Play();
        if(lifeActual <= 0)
        {
            turtle.animator.SetBool("Dead", true);
            turtle.Die();
        }
    }
    public void UpdateHPBar()
    {
        hpbar.value = Mathf.Lerp(hpbar.value, lifeActual, lerpSpeed);
    }
}
