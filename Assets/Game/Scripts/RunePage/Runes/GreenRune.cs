using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenRune : MonoBehaviour,IRune
{
    // Faster atacks and stamina recover
    //Primary
    [SerializeField] GameObject[] runeEffect;
    public void ProjectileHitEffect(Collider other)
    {
        //for each hit enemy adds a little multiplier to the next hit
        other.GetComponent<IDamageable>().TakeDamage((int)(PlayerController.instance.baseDamage * (2f + PlayerController.instance.agility * 0.2f)), 0);
        PlayerController.instance.damageMultiplier += (0.1f * (PlayerController.instance.agility));
    }
    public void AtackCriticalEffect1()
    {
        StaminaBar.instance.RecoverStaminabyItem((PlayerController.instance.stamPerHit * (PlayerController.instance.agility + 1)) / 10);
    }
    public void AtackCriticalEffect2()
    {
        StaminaBar.instance.RecoverStaminabyItem((PlayerController.instance.stamPerHit * (PlayerController.instance.agility + 1)) / 7);
    }
    public void AtackCriticalEffect3()
    {
        StaminaBar.instance.RecoverStaminabyItem((PlayerController.instance.stamPerHit * (PlayerController.instance.agility + 1)) / 4);
    }
    //Secondary
    public void HeavyEffect1()
    {
        PlayerController.instance.InstantiateEffect(runeEffect[0]);
    }
    public void HeavyEffect2()
    {
        PlayerController.instance.InstantiateEffect(runeEffect[1]);
    }
    public void HeavyEffect3()
    {
        PlayerController.instance.InstantiateEffect(runeEffect[2]);
    }
    //Terciary
    public void HitEffect()
    {
        //The player recovers part of the spent stamina
        StaminaBar.instance.RecoverRecoverStaminabyHit();
    }
}
