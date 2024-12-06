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

        if (isHeavyAtack)
        {
            AtackHeavyHit(other);
        }
        else
        {
            if(storedCommand == -1 && !firstAtack)
            {
                AtackCriticalHit(other);
            }
            else
            {
                AtackHit(other);
            }
        }
        PlayerController.instance.runes[PlayerController.instance.equipedTerciaryRune].HitEffect();
        PlayerController.instance.damageAdd = 0;
        PlayerController.instance.damageMultiplier = 1;
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
        PlayerController.instance.comboCounter = 1;
        storedCommand = -1;
    }
    public void StoreCommand(int which)
    {
        storedCommand = which;
    }
    //Chamado Pela Animação
    public void AtackStartAction()
    {
        if (isHeavyAtack)
        {
            switch (PlayerController.instance.comboCounter)
            {
                case 1:
                    PlayerController.instance.actions[1].ActionStart();
                    break;
                case 2:
                    PlayerController.instance.actions[1].ActionStart();
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
                other.GetComponent<IDamageable>().TakeDamage(damageCalc(5, 1, 0.4f, 0.1f, 0), 1);
                break;
            case 2:
                other.GetComponent<IDamageable>().TakeDamage(damageCalc(7, 1.5f, 0.6f, 0.15f, 0), 1);
                break;
            case 3:
                other.GetComponent<IDamageable>().TakeDamage(damageCalc(10, 2f, 0.8f, 0.2f, 0), 1);
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
                other.GetComponent<IDamageable>().TakeDamage(damageCalc(5, 1.75f, 0.4f, 0.1f, 0), 1);
                PlayerController.instance.runes[PlayerController.instance.equipedPrimaryRune].AtackCriticalEffect1();
                break;
            case 2:
                other.GetComponent<IDamageable>().TakeDamage(damageCalc(7, 2.25f, 0.6f, 0.15f, 0), 1);
                PlayerController.instance.runes[PlayerController.instance.equipedPrimaryRune].AtackCriticalEffect2();
                break;
            case 3:
                other.GetComponent<IDamageable>().TakeDamage(damageCalc(10, 3f, 0.8f, 0.2f, 0), 1);
                PlayerController.instance.runes[PlayerController.instance.equipedPrimaryRune].AtackCriticalEffect3();
                break;
        }

    }
    public void AtackHeavyEffect()
    {
        switch (PlayerController.instance.comboCounter)
        {
            case 1:
                PlayerController.instance.runes[PlayerController.instance.equipedSecondaryRune].HeavyEffect1();
                break;
            case 2:
                PlayerController.instance.runes[PlayerController.instance.equipedSecondaryRune].HeavyEffect2();
                break;
            case 3:
                PlayerController.instance.runes[PlayerController.instance.equipedSecondaryRune].HeavyEffect3();
                break;
        }
    }
    public void AtackHeavyHit(Collider other)
    {
        if (firstAtack)
        {
            switch (PlayerController.instance.comboCounter)
            {
                case 1:
                    other.GetComponent<IDamageable>().TakeDamage(damageCalc(15, 1.3f, 0.1f, 0.4f, 0), 1);
                    break;
                case 2:
                    other.GetComponent<IDamageable>().TakeDamage(damageCalc(15, 1.3f, 0.15f, 0.6f, 0), 1);
                    break;
                case 3:
                    other.GetComponent<IDamageable>().TakeDamage(damageCalc(15, 1.3f, 0.2f, 0.8f, 0), 1);
                    break;
            }
        }
        else
        {
            switch (PlayerController.instance.comboCounter)
            {
                case 1:
                    other.GetComponent<IDamageable>().TakeDamage(damageCalc(20, 1.3f, 0.1f, 0.4f, 0), 1);
                    break;
                case 2:
                    other.GetComponent<IDamageable>().TakeDamage(damageCalc(20, 1.3f, 0.15f, 0.6f, 0), 1);
                    break;
                case 3:
                    other.GetComponent<IDamageable>().TakeDamage(damageCalc(20, 1.3f, 0.2f, 0.8f, 0), 1);
                    break;
            }
        }
    }
    public int damageCalc(float baseDamage, float baseMultiplier, float agilityModifier, float strenghtModifier, float resistanceMultiplier)
    {
        return (int)
            ((PlayerController.instance.damageMultiplier + baseMultiplier) *
            (baseDamage + (baseMultiplier + (PlayerController.instance.strength * strenghtModifier) + (PlayerController.instance.agility * agilityModifier) + (PlayerController.instance.resistance * resistanceMultiplier))
            + PlayerController.instance.damageAdd));
    }
}
