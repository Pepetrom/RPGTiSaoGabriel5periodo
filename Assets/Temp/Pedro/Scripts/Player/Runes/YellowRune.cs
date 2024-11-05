using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class YellowRune : MonoBehaviour, IRune
{
    //Primary
    [SerializeField] GameObject[] runeEffect;
    public void ProjectileHitEffect(Collider other)
    {
        other.GetComponent<EnemyHealth>().TakeDamage((int)(PlayerController.instance.baseDamage * (2f + PlayerController.instance.resistance * 0.2f)), 0);
    }
    public void AtackCriticalEffect1()
    {
        PlayerController.instance.DamageAdd = PlayerController.instance.resistance * 2;
        Debug.Log("Crito");
    }
    public void AtackCriticalEffect2()
    {
        PlayerController.instance.DamageAdd = PlayerController.instance.resistance * 2.5f;
        Debug.Log("Crito");
    }
    public void AtackCriticalEffect3()
    {
        PlayerController.instance.DamageAdd = PlayerController.instance.resistance * 3;
        Debug.Log("Crito");
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
        HPBar.instance.RecoverHPbyHit();
    }
}
