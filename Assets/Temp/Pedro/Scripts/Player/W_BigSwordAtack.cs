using UnityEngine;

public class W_BigSwordAtack : IWeapon
{
    public int slot;
    bool canBeInterupted = false, interrupted = false;
    bool atacking = false;
    float positionTimer = 0;
    Vector3 atackDirection;
    Quaternion newRotation;
    int storedCommand = -1;
    bool firstAtack = true, isHeavyAtack = false;
    public bool CanBeInterupted()
    {
        return canBeInterupted;
    }
    public void SetSlot(int slot)
    {
        this.slot = slot;
    }
    public void AtackStart(bool heavy)
    {
        if (!PlayerController.instance.canDoAtack || StaminaBar.instance.currentStam < PlayerController.instance.stamPerHit)
        {
            if (heavy )
            {
                StoreCommand(1);
            }
            else
            {
                StoreCommand(0);
            }
            return;
        }
        isHeavyAtack = heavy;
        StaminaBar.instance.DrainStamina(PlayerController.instance.stamPerHit);
        PlayerController.instance.canMove = false;
        PlayerController.instance.canDoAtack = false;
        PlayerController.instance.moveDirection = Vector3.zero;
        PlayerController.instance.animator.SetBool("Atacking", true);
        PlayerController.instance.animator.SetBool("Walk", false);
        interrupted = false;
        atacking = true;
        if (isHeavyAtack)
        {
            PlayerController.instance.animator.SetTrigger(("AtackHeavy"));
        }
        else
        {
            PlayerController.instance.animator.SetTrigger(("Atack" + PlayerController.instance.comboCounter));
        }
        PlayerController.instance.swordTrail.emitting = true;
        PlayerController.instance.swordTrail.startColor = Color.white;
        PlayerController.instance.isAttacking = true;
        positionTimer = 0;
        canBeInterupted = false;
        if(PlayerController.instance.comboCounter > 1 && firstAtack)
        {
            firstAtack = false;
        }
    }
    public void AtackUpdate()
    {
        if (PlayerController.instance.animator.GetBool("Walk") == true)
        {
            storedCommand = -1;
            PlayerController.instance.comboCounter = 1;
            firstAtack = true;
            /*
            PlayerController.instance.temporaryDamageAdd = 0;
            PlayerController.instance.temporaryDamageMultiplier = 1;
        */
            }
        if (interrupted) return;
        if (atacking)
        {
            if (PlayerController.instance.target != null)
            {
                FacePlayerTarget();
            }
            else
            {
                FacePlayerMouse();
            }
            if (positionTimer >= 1)
            {
                atacking = false;
                /*
                if (isHeavyAtack)
                {
                    AtackHeavyStartAction();
                }
                else
                {
                    AtackStartAction();
                }*/
            }
        }
    }
    public void FacePlayerTarget()
    {
        atackDirection = (PlayerController.instance.target.position - PlayerController.instance.model.transform.position);
        atackDirection.y = 0;
        atackDirection = atackDirection.normalized;
        PlayerController.instance.model.transform.rotation = Quaternion.LookRotation(atackDirection);
        positionTimer = 2;
    }
    public void FacePlayerMouse()
    {
        atackDirection = PlayerController.instance.GetMousePosition();
        atackDirection = (atackDirection - PlayerController.instance.model.transform.position).normalized;
        atackDirection.y = 0;
        newRotation = Quaternion.LookRotation(atackDirection);
        PlayerController.instance.model.transform.rotation = Quaternion.Slerp(PlayerController.instance.model.transform.rotation, newRotation, positionTimer);
        positionTimer += (Time.fixedDeltaTime * 2f);
    }
    public void StartRegisterHit()
    {
        if (interrupted) return;
        PlayerController.instance.atackCollider.gameObject.SetActive(true);
        if (isHeavyAtack)
        {
            AtackHeavyEffect();
        }
    }
    public void StopRegisterHit()
    {
        if (interrupted) return;
        PlayerController.instance.atackCollider.gameObject.SetActive(false);
    }
    public void Hit(Collider other)
    {
        if (interrupted) return;
        if(storedCommand == -1 && !firstAtack)
        {
            if (isHeavyAtack)
            {
                AtackHeavyHit(other);
            }
            else
            {
                AtackCriticalHit(other);
            }
        }
        else
        {
            if (isHeavyAtack)
            {
                AtackHeavyHit(other);
            }
            else
            {
                AtackHit(other);
            }
        }
        PlayerController.instance.runes[PlayerController.instance.equipedTerciaryRune].HitEffect();
        PlayerController.instance.DamageAdd = 0;
        PlayerController.instance.DamageMultiplier = 1;
    }
    public void OpenComboWindow()
    {
        if (interrupted) return;
        PlayerController.instance.comboCounter++;
        if (PlayerController.instance.comboCounter > 3) PlayerController.instance.comboCounter = 1;
        canBeInterupted = true;
        PlayerController.instance.canDoAtack = true;
        PlayerController.instance.swordTrail.startColor = Color.yellow;
        if (storedCommand != -1)
        {
            AtackStart(storedCommand == 1);
            storedCommand = -1;
        }
    }
    public void CloseComboWindow()
    {
        if (interrupted) return;
        PlayerController.instance.canDoAtack = false;
        canBeInterupted = false;
        PlayerController.instance.swordTrail.startColor = Color.white;
        PlayerController.instance.swordTrail.emitting = false;
    }
    public void AtackEnd()
    {
        //if (interrupted) return;
        canBeInterupted = false;
        PlayerController.instance.isAttacking = false; 
        PlayerController.instance.canMove = true;
        PlayerController.instance.canDoAtack = true;
        PlayerController.instance.animator.SetBool("Atacking", false);
        PlayerController.instance.swordTrail.emitting = false;
    }
    public void InteruptAtack()
    {
        if (interrupted) return;
        interrupted = true;
        AtackEnd();
        /*
        canBeInterupted = false;
        PlayerController.instance.isAttacking = false;
        PlayerController.instance.canMove = true;
        PlayerController.instance.canDoAtack = true;
        PlayerController.instance.animator.SetBool("Atacking", false);
        PlayerController.instance.swordTrail.emitting = false;*/
        PlayerController.instance.comboCounter = 1;
        storedCommand = -1;
    }
    public void StoreCommand(int which)
    {
        storedCommand = which;
    }
    public void AtackStartAction()
    {
        if (isHeavyAtack)
        {
            switch (PlayerController.instance.comboCounter)
            {
                case 1:
                    //PlayerController.instance.actions[1].ActionStart();
                    break;
                case 2:
                    //PlayerController.instance.actions[1].ActionStart();
                    break;
                case 3:
                    PlayerController.instance.actions[1].ActionStart();
                    break;
            }
        }
        else
        {
            switch (PlayerController.instance.comboCounter)
            {
                case 1:
                    PlayerController.instance.actions[1].ActionStart();
                    break;
                case 2:
                    //PlayerController.instance.actions[1].ActionStart();
                    break;
                case 3:
                    PlayerController.instance.actions[1].ActionStart();
                    break;
            }
        }
    }

    public void AtackHit(Collider other)
    {
        GameManager.instance.CallHitStop(0.2f);
        PlayerController.instance.swordTrail.startColor = Color.green;
        switch (PlayerController.instance.comboCounter)
        {
            case 1:
                other.GetComponent<TurtleStateMachine>().TakeDamage((int)(PlayerController.instance.baseDamage * (1 * PlayerController.instance.DamageMultiplier + PlayerController.instance.agility * 0.1f) + PlayerController.instance.DamageAdd), 1f);
                break;
            case 2:
                other.GetComponent<TurtleStateMachine>().TakeDamage((int)(PlayerController.instance.baseDamage * (1.25f * PlayerController.instance.DamageMultiplier + PlayerController.instance.agility * 0.25f) + PlayerController.instance.DamageAdd), 1f);
                break;
            case 3:
                other.GetComponent<TurtleStateMachine>().TakeDamage((int)(PlayerController.instance.baseDamage * (1.5f * PlayerController.instance.DamageMultiplier + PlayerController.instance.agility * 0.35f) + PlayerController.instance.DamageAdd), 1.5f);
                break;
        }
        
    }
    public void AtackCriticalHit(Collider other)
    {
        GameManager.instance.CallHitStop(0.35f);
        PlayerController.instance.swordTrail.startColor = Color.red;
        PlayerController.instance.critical.Play();
        switch (PlayerController.instance.comboCounter)
        {
            case 1:
                other.GetComponent<TurtleStateMachine>().TakeDamage((int)(PlayerController.instance.baseDamage * (1.5f + PlayerController.instance.agility * 0.1f)), 2);
                PlayerController.instance.runes[PlayerController.instance.equipedPrimaryRune].AtackCriticalEffect1();
                break;
            case 2:
                other.GetComponent<TurtleStateMachine>().TakeDamage((int)(PlayerController.instance.baseDamage * (1.75f + PlayerController.instance.agility * 0.25f)), 2.5f);
                PlayerController.instance.runes[PlayerController.instance.equipedPrimaryRune].AtackCriticalEffect2();
                break;
            case 3:
                other.GetComponent<TurtleStateMachine>().TakeDamage((int)(PlayerController.instance.baseDamage * (2.25f + PlayerController.instance.agility * 0.35f)), 3);
                PlayerController.instance.runes[PlayerController.instance.equipedPrimaryRune].AtackCriticalEffect3();
                break;
        }

    }
    public void AtackHeavyEffect()
    {
        switch (PlayerController.instance.comboCounter)
        {
            case 1:
                PlayerController.instance.runes[PlayerController.instance.equipedPrimaryRune].HeavyEffect1();
                break;
            case 2:
                PlayerController.instance.runes[PlayerController.instance.equipedPrimaryRune].HeavyEffect2();
                break;
            case 3:
                PlayerController.instance.runes[PlayerController.instance.equipedPrimaryRune].HeavyEffect3();
                break;
        }
    }
    public void AtackHeavyHit(Collider other)
    {
        if (firstAtack)
        {
            return;
        }
        else
        {
            switch (PlayerController.instance.comboCounter)
            {
                case 1:
                    other.GetComponent<TurtleStateMachine>().TakeDamage((int)(PlayerController.instance.baseDamage * (1.3f + PlayerController.instance.strength * 0.1f)), 1);
                    break;
                case 2:
                    other.GetComponent<TurtleStateMachine>().TakeDamage((int)(PlayerController.instance.baseDamage * (1.3f + PlayerController.instance.strength * 0.25f)), 1);
                    break;
                case 3:
                    other.GetComponent<TurtleStateMachine>().TakeDamage((int)(PlayerController.instance.baseDamage * (1.3f + PlayerController.instance.strength * 0.5f)), 1);
                    break;
            }
        }
    }
}
