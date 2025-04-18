using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class YellowRune : MonoBehaviour, IRune
{
    //Makes you more Tanky and heals you, basically, makes it easier to not die
    //Primary
    [SerializeField] GameObject[] runeEffect;
    public void ProjectileHitEffect(Collider other)
    {
        // for each enemy hit, adds a little bit flat damage to the next atack
        other.GetComponent<IDamageable>().TakeDamage((int)(PlayerController.instance.baseDamage * (2f + PlayerController.instance.resistance * 0.2f)), 0);
        PlayerController.instance.damageAdd += PlayerController.instance.resistance * 1.5f;
    }
    public void AtackCriticalEffect1()
    {
        HPBar.instance.RecoverHPbyItem((int)(PlayerController.instance.resistance * 1f));
    }
    public void AtackCriticalEffect2()
    {
        HPBar.instance.RecoverHPbyItem((int)(PlayerController.instance.resistance * 2f));
    }
    public void AtackCriticalEffect3()
    {
        HPBar.instance.RecoverHPbyItem((int)(PlayerController.instance.resistance * 3f));
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
        //recupera parte da barra secundária de vida
        HPBar.instance.RecoverHPbyHit();
    }
}
