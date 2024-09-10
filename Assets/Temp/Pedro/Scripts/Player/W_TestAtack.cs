using System;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class W_TestAtack : IWeapon
{
    public int slot;
    public float atackDamage;
    bool canBeInterupted = false;
    bool interupted = true;
    bool atacking = false;

    public bool CanBeInterupted()
    {
        return canBeInterupted;
    }
    public void SetSlot(int slot)
    {
        this.slot = slot;
        atackDamage = PlayerController.instance.baseDamage;
    }
    public void AtackStart()
    {
        PlayerController.instance.canDoAtack[slot] = false;
        canBeInterupted = true;
        interupted = false;
        PlayerController.instance.tempSwordAnimator.SetTrigger("atack1");
    }

    public void AtackUpdate()
    {
        if(atacking)
        {

        }
    }
    public void EventOne()
    {
        if (!interupted)
        {
        PlayerController.instance.atackCollider.gameObject.SetActive(true); 
        }
    }
    public void EventTwo()
    {
        if (!interupted)
        {
            PlayerController.instance.atackCollider.gameObject.SetActive(false);
        }
    }
    public void Hit(Collider other)
    {
        if (!interupted)
        {
            other.GetComponent<EnemyHealth>().TakeDamage(1);
        }
    }
    public void EventThree()
    {
        if (!interupted)
        {
            
        }
    }
    public void AtackEnd()
    {
        PlayerController.instance.canDoAtack[slot] = true;
        interupted = true;
    }

    public void InteruptAtack()
    {
        interupted = true;
        PlayerController.instance.tempSwordAnimator.SetTrigger("stopAnim");
        AtackEnd();
        PlayerController.instance.atackCollider.gameObject.SetActive(false);
    }
}
