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
    float knockbackTime = 0;
    Vector3 knockBackDirection;
    public GameObject owner;

    private void Start()
    {
        lifeActual = lifeTotal;
        hpbar.maxValue = lifeTotal;
        hpbar.value = lifeTotal;
    }
    private void Update()
    {
        UpdateHPBar();
    }
    private void FixedUpdate()
    {
        KnockBack();
    }
    void KnockBack()
    {
        if(knockbackTime > 0)
        {
            transform.position += knockBackDirection; 
        }
    }
    public void TakeDamage(int damage, int knockbackStrenght)
    {
        knockbackTime = 1;
        knockBackDirection = PlayerController.instance.moveDirection * Time.fixedDeltaTime * knockbackStrenght * 50;
        knockBackDirection.y = 0;
        lifeActual -= damage;
        GameManager.instance.SpawnDamageNumber(damage, transform);
        if(lifeActual <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    public void UpdateHPBar()
    {
        hpbar.value = Mathf.Lerp(hpbar.value, lifeActual, lerpSpeed);
    }
}
