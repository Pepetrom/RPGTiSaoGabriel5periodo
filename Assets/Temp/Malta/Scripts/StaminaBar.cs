using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider stambar;
    public Slider easebar;
    public float maxStam;
    public float currentStam;
    public float moveSpeedBase;
    public float lerpSpeed;
    public float staminaRecover;
    public static StaminaBar stambarInstance;

    float moveSpeed;

    private void Awake()
    {
        stambarInstance = this;
    }
    private void Start()
    {
        currentStam = maxStam;
        stambar.maxValue = maxStam;
        easebar.maxValue = maxStam;
        stambar.value = currentStam;
        easebar.value = currentStam;
    }

    private void FixedUpdate()
    {
        UpdateDrainStamina();
        RecoverStamina(staminaRecover);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            DrainStamina(10);
        }
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
            //Coloquei um Clamp para estamina não ficar negativa
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
