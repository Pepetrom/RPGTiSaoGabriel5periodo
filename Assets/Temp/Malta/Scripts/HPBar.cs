using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Slider hpbar;
    public Slider easebar;
    public float baseHP;
    float maxHP;
    public float currentHP;
    public float moveSpeedBase;
    public float lerpSpeed;
    public static HPBar instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        UpdateMaxHp();
        ResetBar();
    }
    public void ResetBar()
    {
        currentHP = maxHP;
        hpbar.maxValue = maxHP;
        easebar.maxValue = maxHP;
        hpbar.value = currentHP;
        easebar.value = currentHP;
    }
    public void UpdateMaxHp(float atribute = 1)
    {
        maxHP = baseHP + (20 * atribute);
    }

    private void FixedUpdate()
    {
        UpdateHealthBar();
    }
    public void UpdateHealthBar()
    {
        if (hpbar.value != currentHP)
        {
            hpbar.value = Mathf.Lerp(hpbar.value, currentHP, lerpSpeed);
        }
        if (easebar.value != hpbar.value)
        {
            easebar.value = Mathf.Lerp(easebar.value, hpbar.value, lerpSpeed / 10);
        }
    }
    public void RecoverHPbyHit()
    {
        if (easebar.value >= hpbar.value)
        {
            currentHP += (easebar.value - currentHP) / 2;
            easebar.value -= (easebar.value - currentHP) / 2;
        }
    }
    public void RecoverHPbyItem(int value)
    {
        currentHP = Mathf.Clamp(currentHP + value, 1, maxHP);
        GameManager.instance.SpawnNumber(value, Color.green, PlayerController.instance.transform);
    }
    public void FallDamage(float damage)
    {
        if (PlayerController.instance.imortal) return;
        if (PlayerController.instance.canTakeDamage && currentHP > 0)
        {
            if (easebar.value != hpbar.value)
            {
                easebar.value = hpbar.value;
            }
            currentHP -= damage;
            currentHP = Mathf.Clamp(currentHP, 0, maxHP);
            if (currentHP > 0)
            {
                GameManager.instance.SpawnNumber((int)damage, Color.red, PlayerController.instance.transform);
                CameraScript.instance.TakeHit(CameraScript.instance.targetVigColor);
            }
        }
        if (currentHP <= 0)
        {
            StopCoroutine(InvulnableTime());
            GameManager.instance.Respawn();
        }
    }
    public void TakeDamage(float damage, Transform damageFont)
    {
        if (PlayerController.instance.imortal) return;
        if (PlayerController.instance.canTakeDamage && currentHP > 0)
        {
            if (easebar.value != hpbar.value)
            {
                easebar.value = hpbar.value;
            }
            currentHP -= damage;
            currentHP = Mathf.Clamp(currentHP, 0, maxHP);
            if(currentHP > 0)
            {
                StartCoroutine(InvulnableTime());
                GameManager.instance.SpawnNumber((int)damage, Color.red, PlayerController.instance.transform);
                PlayerController.instance.damageFont = damageFont;
                PlayerController.instance.actions[2].ActionStart();
                CameraScript.instance.TakeHit(CameraScript.instance.targetVigColor);
            }
            else
            {
                Die();
            }
        }
    }
    public void Die()
    {
        PlayerController.instance.Die();
        FMODAudioManager.instance.PlayOneShot(FMODAudioManager.instance.hoffDeath, PlayerController.instance.transform.position);
        UIItems.instance.ShowBOSSHUD(false);
        StopCoroutine(InvulnableTime());
        GameManager.instance.Respawn();
    }
    public IEnumerator InvulnableTime()
    {
        PlayerController.instance.canTakeDamage = false;
        yield return new WaitForSeconds(PlayerController.instance.invencibilityTime);
        PlayerController.instance.canTakeDamage = true;
    }
}
