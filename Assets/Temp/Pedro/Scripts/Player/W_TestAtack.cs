using System;
using UnityEngine;
using UnityEngine.EventSystems;
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
        PlayerController.instance.moveDirection = Vector3.zero;
        PlayerController.instance.runningMultiplier = 1;
        PlayerController.instance.animator.SetBool("Atacking", true);
        PlayerController.instance.animator.SetBool("Walk", false);
        //PlayerController.instance.animator.SetBool("Run", false);
        PlayerController.instance.canMove = false;
        canBeInterupted = true;
        interupted = false;
        PlayerController.instance.animator.SetTrigger("Atack"+ PlayerController.instance.comboCounter);
    }
    public void AtackUpdate()
    {
        if(atacking)
        {

        }
    }
    public void StartRegisterHit()
    {
        if (!interupted)
        {
        PlayerController.instance.atackCollider.gameObject.SetActive(true); 
        }
    }
    public void StopRegisterHit()
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
    public void OpenComboWindow()
    {
        if (!interupted)
        {
            PlayerController.instance.comboCounter ++;
            PlayerController.instance.comboCounter = Mathf.Clamp(PlayerController.instance.comboCounter, 1, 3);
            PlayerController.instance.canDoAtack[slot] = true;
        }
    }
    public void CloseComboWindow()
    {
        PlayerController.instance.comboCounter = 1;
        PlayerController.instance.canDoAtack[slot] = false;
    }
    public void AtackEnd()
    {
        PlayerController.instance.canDoAtack[slot] = true;
        PlayerController.instance.canMove = true;
        PlayerController.instance.comboCounter = 1;
        PlayerController.instance.animator.SetBool("Atacking", false);
        interupted = true;
    }

    public void InteruptAtack()
    {
        interupted = true;
        PlayerController.instance.animator.SetTrigger("stopAnim");
        AtackEnd();
        PlayerController.instance.atackCollider.gameObject.SetActive(false);
    }
}
