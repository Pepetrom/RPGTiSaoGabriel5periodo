using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Slider hpbar;
    public Slider easebar;
    public float maxHP;
    public float currentHP;
    public float moveSpeed;
    public float lerpSpeed;
    private float targetHP;
    public static HPBar hpbarInstance;

    private void Awake()
    {
        hpbarInstance = this;
    }
    private void Start()
    {
        currentHP = maxHP;
        hpbar.maxValue = maxHP;
        easebar.maxValue = maxHP;
        hpbar.value = currentHP;
        easebar.value = currentHP;
    }

    private void Update()
    {
        if(currentHP <= 0)
        {
            Debug.Log("Acabou!!!");
        }
        UpdateHPBarTakingDamage();
    }

    private void UpdateHPBarTakingDamage()
    {
        if (hpbar.value != currentHP)
        {
            UpdateHealthBar();
        }
        if (easebar.value != hpbar.value)
        {
            easebar.value = Mathf.MoveTowards(easebar.value, hpbar.value, moveSpeed * Time.deltaTime);
        }
    }
    public void RecoverLifebyHit(int value)
    {
        if(hpbar.value != easebar.value && easebar.value < maxHP && ((value/10) <= easebar.value - currentHP))
        {
            Debug.Log("Recuperei");
            currentHP += value;
        }
    }
    public void UpdateHealthBar()
    {
        hpbar.value = Mathf.Lerp(hpbar.value, currentHP, lerpSpeed);
        Debug.Log("Atualizando a barra de vida");
    }

    public void TakeDamage(float damage)
    {
        GameManager.instance.SpawnDamageNumber((int)damage, PlayerController.instance.transform);
        if (easebar.value != hpbar.value)
        {
            easebar.value = hpbar.value;
        }
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);
        hpbar.value = currentHP;
    }

}
