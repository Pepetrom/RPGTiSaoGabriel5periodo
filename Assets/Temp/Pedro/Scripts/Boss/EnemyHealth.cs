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

    private void Start()
    {
        lifeActual = lifeTotal;
        hpbar.maxValue = lifeTotal;
        hpbar.value = lifeTotal;
    }
    private void FixedUpdate()
    {
        UpdateHPBar();
        //KnockBack();
    }
    void KnockBack()
    {
        if(knockbackTime > 0)
        {
            transform.position += knockBackDirection;
            knockbackTime -= Time.fixedDeltaTime;
        }
    }
    public void TakeDamage(int damage, int knockbackStrenght)
    {
        //knockbackTime = 0.2f;
        //knockBackDirection = PlayerController.instance.moveDirection.normalized * Time.fixedDeltaTime * knockbackStrenght * 50;
        //knockBackDirection.y = 0;
        turtle.Impulse();
        lifeActual -= damage;
        GameManager.instance.SpawnNumber((int)damage, Color.yellow, transform);
        if(lifeActual <= 0)
        {
            Destroy(this.gameObject);
        }
        playerHit = true;
    }
    public void UpdateHPBar()
    {
        hpbar.value = Mathf.Lerp(hpbar.value, lifeActual, lerpSpeed);
    }
}
