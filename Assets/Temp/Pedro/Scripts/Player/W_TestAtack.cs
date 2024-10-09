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
    float positionTimer = 0;
    Vector3 atackDirection;
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

        atacking = true;
        PlayerController.instance.animator.SetTrigger(("Atack" + PlayerController.instance.comboCounter));

        atackDirection = PlayerController.instance.GetMousePosition();
        atackDirection = (atackDirection - PlayerController.instance.model.transform.position).normalized;

        atackDirection.y = 0;
        positionTimer = 0;
        
        canBeInterupted = false;
        interupted = false;
        PlayerController.instance.canMove = false;
        PlayerController.instance.isAttacking = true;         
    }
    public void AtackUpdate()
    {
        if(atacking)
        {
            FacePlayerMouse();
        }
    }
    void FacePlayerMouse()
    {
        newRotation = Quaternion.LookRotation(atackDirection);
        PlayerController.instance.model.transform.rotation = Quaternion.Slerp(PlayerController.instance.model.transform.rotation, newRotation, positionTimer);
        positionTimer += (Time.fixedDeltaTime * 2f);

        if (positionTimer >= 1)
        {
            atacking = false;
            switch (PlayerController.instance.comboCounter)
            {
                case 1:
                    PlayerController.instance.actions[1].ActionStart();
                    break;
                case 2:
                    PlayerController.instance.actions[1].ActionStart();
                    break;
                case 3:
                    break;
            }
            
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
            GameManager.instance.CallHitStop(0.2f);
            other.GetComponent<EnemyHealth>().TakeDamage(PlayerController.instance.baseDamage, PlayerController.instance.comboCounter);
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
