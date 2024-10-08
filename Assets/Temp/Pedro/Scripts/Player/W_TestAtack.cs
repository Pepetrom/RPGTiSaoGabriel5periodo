using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class W_TestAtack : IWeapon
{
    public int slot;
    public float atackDamage;
    bool canBeInterupted = false;
    bool interupted = true;
    bool atacking = false;
    int comboSize = 3;
    float positionTimer = 0, turnSpeed;
    Vector3 atackDirection, newAtackDirection;
    Quaternion newRotation;
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
        PlayerController.instance.animator.SetBool("Atacking", true);
        PlayerController.instance.animator.SetBool("Walk", false);

        PlayerController.instance.animator.SetTrigger(("Atack" + PlayerController.instance.comboCounter));

        atackDirection = PlayerController.instance.GetMousePosition();
        newAtackDirection = PlayerController.instance.model.transform.forward;

        atackDirection = (atackDirection - PlayerController.instance.model.transform.position).normalized;

        atackDirection.y = 0;
        newAtackDirection.y = 0;
        positionTimer = 0;
        turnSpeed = Vector3.Distance(newAtackDirection, atackDirection);
        
        atacking = true;
        PlayerController.instance.canMove = false;
        canBeInterupted = false;
        interupted = false;
        PlayerController.instance.isAttacking = true;         
    }
    public void AtackUpdate()
    {
        if(atacking)
        {
            if( atackDirection != Vector3.zero)
            {
                FacePlayerMouse();
            }
        }
    }
    void FacePlayerMouse()
    {
        newRotation = Quaternion.LookRotation(atackDirection);
        PlayerController.instance.model.transform.rotation = Quaternion.Slerp(PlayerController.instance.model.transform.rotation, newRotation, positionTimer);
        positionTimer += (Time.fixedDeltaTime * 2f) / turnSpeed;

        if (positionTimer >= 1)
        {
            atacking = false;
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
            other.GetComponent<EnemyHealth>().TakeDamage(PlayerController.instance.baseDamage);
            HPBar.hpbarInstance.RecoverLifebyHit(PlayerController.instance.baseDamage/10);
        }
    }
    public void OpenComboWindow()
    {
        if (!interupted)
        {
            PlayerController.instance.comboCounter ++;
            if(PlayerController.instance.comboCounter > comboSize) PlayerController.instance.comboCounter = 1;
            PlayerController.instance.canDoAtack[slot] = true;
            canBeInterupted = true;
        }
    }
    public void CloseComboWindow()
    {
        PlayerController.instance.comboCounter = 1;
        PlayerController.instance.canDoAtack[slot] = false;
        canBeInterupted = false;
    }
    public void AtackEnd()
    {
        PlayerController.instance.canDoAtack[slot] = true;
        PlayerController.instance.canMove = true;
        PlayerController.instance.comboCounter = 1;
        canBeInterupted = false;
        PlayerController.instance.isAttacking = false; // Parou de atacar
        PlayerController.instance.animator.SetBool("Atacking", false);
    }

    public void InteruptAtack()
    {
        PlayerController.instance.animator.SetTrigger("stopAnim");
        interupted = true;
        StopRegisterHit();
        AtackEnd();
    }
}
