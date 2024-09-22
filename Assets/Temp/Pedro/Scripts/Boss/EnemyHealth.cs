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
    public void TakeDamage(int damage)
    {
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
