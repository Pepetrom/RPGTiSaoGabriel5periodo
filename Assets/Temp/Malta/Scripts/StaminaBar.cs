using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider stambar;
    public Slider easebar;
    public float baseStam = 50;
    float maxStam;
    public float currentStam;
    public float moveSpeedBase;
    public float lerpSpeed;
    public float staminaRecover;
    public static StaminaBar instance;

    float moveSpeed;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        UpdateMaxStamina();
    }

    private void FixedUpdate()
    {
        UpdateDrainStamina();
        RecoverStamina(staminaRecover);
    }
    public void UpdateMaxStamina()
    {
        maxStam = baseStam + PlayerController.instance.resistance * 10;
        currentStam = maxStam;
        stambar.maxValue = maxStam;
        easebar.maxValue = maxStam;
        stambar.value = currentStam;
        easebar.value = currentStam;
    }
    private void UpdateDrainStamina()
    {
        if (stambar.value != currentStam)
        {
            UpdateStamina();
        }
        if (easebar.value != stambar.value)
        {
            easebar.value = Mathf.MoveTowards(easebar.value, stambar.value, moveSpeed * Time.fixedDeltaTime);
        }
    }
    public void UpdateStamina()
    {
        stambar.value = Mathf.Lerp(stambar.value, currentStam, lerpSpeed);
        //Debug.Log("Atualizando a barra de estamina");
    }

    public void DrainStamina(float value)
    {
        if(currentStam >= 0)
        {
            if (easebar.value != stambar.value)
            {
                easebar.value = stambar.value;
            }
            //Coloquei um Clamp para estamina n�o ficar negativa
            currentStam -= Mathf.Clamp(value,0, maxStam);
            stambar.value = currentStam;
            moveSpeed = moveSpeedBase * value;
        }
        else
        {
            currentStam = 0f;
            stambar.value = currentStam;
        }
    }
    public void RecoverRecoverStaminabyHit()
    {
        if (easebar.value >= stambar.value)
        {
            currentStam += (easebar.value - currentStam) / 2;
            easebar.value -= (easebar.value - currentStam) / 2;
        }
    }
    public void RecoverStamina(float value)
    {
        if (!PlayerController.instance.isAttacking)
        {
            currentStam += value * Time.deltaTime;
            if (currentStam > maxStam)
            {
                currentStam = maxStam;
            }
        }
    }


}
