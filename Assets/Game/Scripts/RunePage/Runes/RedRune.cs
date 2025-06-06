using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedRune : MonoBehaviour,IRune
{
    // Massive damage, low survavibility in battle, but great recover
    //Primary
    [SerializeField] GameObject[] runeEffect;
    int random;
    public void ProjectileHitEffect(Collider other)
    {
        //adds a chance to regain one cheese for each enemy hit with the skill
        other.GetComponent<IDamageable>().TakeDamage((int)(PlayerController.instance.baseDamage * (2f + PlayerController.instance.strength * 0.2f)), 0);
        random = Random.Range(0, 20);
        if(random == 1)
        {
            Estus.instance.AddEstus();
        }
    }
    public void AtackCriticalEffect1()
    {
        PlayerController.instance.damageMultiplier = 1 + (0.1f * (PlayerController.instance.strength +1 ));
    }
    public void AtackCriticalEffect2()
    {
        PlayerController.instance.damageMultiplier = 1 + (0.15f * (PlayerController.instance.strength + 1));
    }
    public void AtackCriticalEffect3()
    {
        PlayerController.instance.damageMultiplier = 1 + (0.2f * (PlayerController.instance.strength + 1));
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
        //aumenta seu dano no próximo hit
        PlayerController.instance.damageAdd += PlayerController.instance.strength * 1.5f;
    }
}
