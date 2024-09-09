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
        UpdateHPBarTakingDamage();
        RecoverLifebyHit();
    }

    private void UpdateHPBarTakingDamage()
    {
        if (hpbar.value != currentHP)
        {
            UpdateHealthBar();
        }

        // Smoothly decrease ease bar to current HP
        if (easebar.value != hpbar.value)
        {
            easebar.value = Mathf.MoveTowards(easebar.value, hpbar.value, moveSpeed * Time.deltaTime);
        }
    }
    private void RecoverLifebyHit()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            float d = 8f;
            if(hpbar.value != easebar.value && easebar.value < maxHP && (d <= easebar.value - currentHP))
            {
                currentHP += d;
            }
            Debug.Log("Q");
        }
    }
    public void UpdateHealthBar()
    {
        hpbar.value = Mathf.Lerp(hpbar.value, currentHP, lerpSpeed);
        //hpbar.value = easebar.value;
        Debug.Log("Atualizando a barra de vida");
    }

    public void TakeDamage(float damage)
    {
        // If the ease bar is still higher than the HP bar (mid-transition), snap it to the current HP value
        if (easebar.value != hpbar.value)
        {
            easebar.value = hpbar.value; // Force the easebar to catch up before applying new damage
        }

        // Now apply the new damage
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0); // Ensure currentHP doesn't go below 0

        // Immediately set the HP bar to reflect the new current HP
        hpbar.value = currentHP;
    }

}
