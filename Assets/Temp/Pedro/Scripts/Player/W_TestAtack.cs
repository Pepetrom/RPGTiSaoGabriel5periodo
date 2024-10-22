using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class W_TestAtack : IWeapon
{
    public int slot;
    bool canBeInterupted = false, interrupted = false;
    bool atacking = false;
    int comboSize = 3;
    float positionTimer = 0;
    Vector3 atackDirection;
    Quaternion newRotation;
    int storedCommand = -1;
    public bool CanBeInterupted()
    {
        return canBeInterupted;
    }
    public void SetSlot(int slot)
    {
        this.slot = slot;
    }
    public void AtackStart()
    {
        PlayerController.instance.canDoAtack[slot] = false;
        PlayerController.instance.moveDirection = Vector3.zero;
        PlayerController.instance.animator.SetBool("Atacking", true);
        PlayerController.instance.animator.SetBool("Walk", false);
        interrupted = false;
        atacking = true;
        PlayerController.instance.animator.SetTrigger(("Atack" + PlayerController.instance.comboCounter));
        PlayerController.instance.swordTrail.emitting = true;
        PlayerController.instance.swordTrail.startColor = Color.white;
        atackDirection = PlayerController.instance.GetMousePosition();
        atackDirection = (atackDirection - PlayerController.instance.model.transform.position).normalized;

        atackDirection.y = 0;
        positionTimer = 0;

        canBeInterupted = false;
        PlayerController.instance.canMove = false;
        PlayerController.instance.isAttacking = true;
    }
    public void AtackUpdate()
    {
        if (interrupted) return;
        if (atacking)
        {
            if (PlayerController.instance.target != null)
            {
                atackDirection = (PlayerController.instance.target.position - PlayerController.instance.model.transform.position);
                atackDirection.y = 0;
                atackDirection = atackDirection.normalized;
                PlayerController.instance.model.transform.rotation = Quaternion.LookRotation(atackDirection);
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
            else
            {
                FacePlayerMouse();
            }
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
        if (interrupted) return;
        PlayerController.instance.atackCollider.gameObject.SetActive(true);
    }
    public void StopRegisterHit()
    {
        if (interrupted) return;
        PlayerController.instance.atackCollider.gameObject.SetActive(false);
    }
    public void Hit(Collider other)
    {
        if (interrupted) return;
        if(storedCommand == -1)
        {
            GameManager.instance.CallHitStop(0.35f);
            other.GetComponent<EnemyHealth>().TakeDamage((int)(PlayerController.instance.baseDamage * 2 * (PlayerController.instance.comboCounter * 0.5f + (PlayerController.instance.strength * 0.5f))), PlayerController.instance.comboCounter);
            PlayerController.instance.swordTrail.startColor = Color.red;
            PlayerController.instance.critical.Play();
        }
        else
        {
            GameManager.instance.CallHitStop(0.2f);
            other.GetComponent<EnemyHealth>().TakeDamage((int)(PlayerController.instance.baseDamage * (PlayerController.instance.comboCounter * 0.5f + (PlayerController.instance.strength * 0.5f))), PlayerController.instance.comboCounter);
            PlayerController.instance.swordTrail.startColor = Color.green;
        }
        HPBar.instance.RecoverHPbyHit();
    }
    public void OpenComboWindow()
    {
        if (interrupted) return;
        PlayerController.instance.comboCounter++;
        if (PlayerController.instance.comboCounter > comboSize) PlayerController.instance.comboCounter = 1;
        PlayerController.instance.canDoAtack[slot] = true;
        canBeInterupted = true;
        PlayerController.instance.swordTrail.startColor = Color.yellow;
        switch (storedCommand)
        {
            case 0:
                AtackStart();
                break;
        }
        storedCommand = -1;
    }
    public void CloseComboWindow()
    {
        if (interrupted) return;
        PlayerController.instance.comboCounter = 1;
        PlayerController.instance.canDoAtack[slot] = false;
        canBeInterupted = false;
        PlayerController.instance.swordTrail.startColor = Color.white;
        PlayerController.instance.swordTrail.emitting = false;
    }
    public void AtackEnd()
    {
        if (interrupted) return;
        PlayerController.instance.canDoAtack[slot] = true;
        PlayerController.instance.canMove = true;
        PlayerController.instance.comboCounter = 1;
        canBeInterupted = false;
        PlayerController.instance.isAttacking = false; // Parou de atacar
        PlayerController.instance.animator.SetBool("Atacking", false);
        PlayerController.instance.swordTrail.emitting = false;
    }

    public void InteruptAtack()
    {
        if (interrupted) return;
        interrupted = true;
        canBeInterupted = false;
        PlayerController.instance.isAttacking = false;
        PlayerController.instance.canMove = true;
        PlayerController.instance.canDoAtack[slot] = true;
        PlayerController.instance.animator.SetBool("Atacking", false);
        PlayerController.instance.comboCounter = 1;
        PlayerController.instance.swordTrail.emitting = false;
    }
    public void StoreCommand(int which)
    {
        storedCommand = which;
    }
}
