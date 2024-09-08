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
        if(hpbar.value != easebar.value)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                float r = Random.value;
                easebar.value = hpbar.value;
                TakeDamage(r);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                float r = Random.value;
                TakeDamage(r);
            }
        }
        easebar.value = Mathf.MoveTowards(easebar.value, hpbar.value, moveSpeed * Time.deltaTime);

    }
    private void RecoverLifebyHit()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            float d = 0.3f;
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

    private void TakeDamage(float damage)
    {
        currentHP -= damage;
        // Ensure currentHP doesn't go below 0
        currentHP = Mathf.Max(currentHP, 0);
    }
}
