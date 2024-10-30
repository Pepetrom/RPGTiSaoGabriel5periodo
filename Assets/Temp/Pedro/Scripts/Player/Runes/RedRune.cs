using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedRune : MonoBehaviour,IRune
{
    //Primary
    [SerializeField] GameObject[] runeEffect;
    public void ProjectileHitEffect(Collider other)
    {
        other.GetComponent<EnemyHealth>().TakeDamage((int)(PlayerController.instance.baseDamage * (2f + PlayerController.instance.strength * 0.2f)), 0);
    }
    public void AtackCriticalEffect1()
    {
        PlayerController.instance.temporaryDamageAdd = PlayerController.instance.strength * 2;
    }
    public void AtackCriticalEffect2()
    {
        PlayerController.instance.temporaryDamageAdd = PlayerController.instance.strength * 2.5f;
    }
    public void AtackCriticalEffect3()
    {
        PlayerController.instance.temporaryDamageAdd = PlayerController.instance.strength * 3;
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
        PlayerController.instance.temporaryDamageMultiplier = 1.25f;
    }
}
