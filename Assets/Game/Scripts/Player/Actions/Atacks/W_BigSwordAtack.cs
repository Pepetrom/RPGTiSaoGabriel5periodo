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
    PlayerController player;

    public bool CanBeInterupted()
    {
        return canBeInterupted;
    }
    public void SetSlot(int slot)
    {
        this.slot = slot;
        player = PlayerController.instance;
    }
    public void AtackStart(bool heavy)
    {
        if(!player.masterCanDo || StaminaBar.instance.currentStam < player.stamPerHit) return;
        if (!player.canDoAtack )
        {
            if (!player.holdingToAtack) return;
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
        StaminaBar.instance.DrainStamina(player.stamPerHit);
        player.canMove = false;
        player.canDoAtack = false;
        player.moveDirection = Vector3.zero;
        player.animator.SetBool("Atacking", true);
        player.animator.SetBool("Walk", false);
        interrupted = false;
        atacking = true;
        if (isHeavyAtack)
        {
            player.animator.SetTrigger(("AtackHeavy"));
        }
        else
        {
            player.animator.SetTrigger(("Atack" + player.comboCounter));
        }
        player.swordTrail.emitting = true;
        player.swordTrail.startColor = Color.white;
        player.isAttacking = true;
        positionTimer = 0;
        canBeInterupted = false;
        if(player.comboCounter > 1 && firstAtack)
        {
            firstAtack = false;
        }
    }
    public void AtackUpdate()
    {
        if (player.animator.GetBool("Walk") == true)
        {
            storedCommand = -1;
            player.comboCounter = 1;
            firstAtack = true;
            }
        if (interrupted) return;
        if (atacking)
        {
            if (player.target != null)
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
        atackDirection = (player.target.position - player.model.transform.position);
        atackDirection.y = 0;
        atackDirection = atackDirection.normalized;
        player.model.transform.rotation = Quaternion.LookRotation(atackDirection);
        positionTimer = 2;
    }
    public void FacePlayerMouse()
    {
        atackDirection = player.GetMousePosition();
        atackDirection = (atackDirection - player.model.transform.position).normalized;
        atackDirection.y = 0;
        newRotation = Quaternion.LookRotation(atackDirection);
        player.model.transform.rotation = Quaternion.Slerp(player.model.transform.rotation, newRotation, positionTimer);
        positionTimer += (Time.fixedDeltaTime * 2f);
    }
    public void StartRegisterHit()
    {
        if (interrupted) return;
        player.atackCollider.gameObject.SetActive(true);
        if (isHeavyAtack)
        {
            AtackHeavyEffect();
        }
    }
    public void StopRegisterHit()
    {
        if (interrupted) return;
        player.atackCollider.gameObject.SetActive(false);
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
        player.runes[player.equipedTerciaryRune].HitEffect();
        player.damageAdd = 0;
        player.damageMultiplier = 1;
    }
    public void OpenComboWindow()
    {
        if (interrupted) return;
        player.comboCounter++;
        if (player.comboCounter > 3) player.comboCounter = 1;
        canBeInterupted = true;
        player.canDoAtack = true;
        player.swordTrail.startColor = Color.yellow;
        if (storedCommand != -1)
        {
            AtackStart(storedCommand == 1);
            storedCommand = -1;
        }
    }
    public void CloseComboWindow()
    {
        if (interrupted) return;
        player.canDoAtack = false;
        canBeInterupted = false;
        player.swordTrail.startColor = Color.white;
        player.swordTrail.emitting = false;
    }
    public void AtackEnd()
    {
        canBeInterupted = false;
        player.isAttacking = false;
        player.canMove = true;
        player.canDoAtack = true;
        player.animator.SetBool("Atacking", false);
        player.swordTrail.emitting = false;
        //player.ResetAllActions();
    }
    public void InteruptAtack()
    {
        if (interrupted) return;
        interrupted = true;
        storedCommand = -1;
        player.comboCounter = 1;
        AtackEnd();
    }
    public void StoreCommand(int which)
    {
        storedCommand = which;
    }
    //Chamado Pela Anima��o
    public void AtackStartAction()
    {
        if (isHeavyAtack)
        {
            switch (player.comboCounter)
            {
                case 1:
                    FMODAudioManager.instance.PlayOneShot(FMODAudioManager.instance.sword[2], player.transform.position);
                    player.audioMan.PlayAudio(2);
                    player.actions[1].ActionStart();
                    break;
                case 2:
                    FMODAudioManager.instance.PlayOneShot(FMODAudioManager.instance.sword[2], player.transform.position);
                    player.audioMan.PlayAudio(2);
                    player.actions[1].ActionStart();
                    break;
                case 3:
                    FMODAudioManager.instance.PlayOneShot(FMODAudioManager.instance.sword[2], player.transform.position);
                    player.audioMan.PlayAudio(2);
                    player.actions[1].ActionStart();
                    break;
            }
        }
        else
        {
            switch (player.comboCounter)
            {
                case 1:
                    FMODAudioManager.instance.PlayOneShot(FMODAudioManager.instance.sword[0], player.transform.position);
                    player.actions[1].ActionStart();
                    break;
                case 2:
                    FMODAudioManager.instance.PlayOneShot(FMODAudioManager.instance.sword[0], player.transform.position);
                    //player.actions[1].ActionStart();
                    break;
                case 3:
                    FMODAudioManager.instance.PlayOneShot(FMODAudioManager.instance.sword[1], player.transform.position);
                    player.audioMan.PlayAudio(1);
                    player.actions[1].ActionStart();
                    break;
            }
        }
    }
    // acerto de ataque normal
    public void AtackHit(Collider other)
    {
        FMODAudioManager.instance.PlayOneShot(FMODAudioManager.instance.sword[3], player.transform.position);
        player.audioMan.PlayAudio(3);
        GameManager.instance.CallHitStop(0.2f);
        player.swordTrail.startColor = Color.green;
        switch (player.comboCounter)
        {
            case 1:
                other.GetComponent<IDamageable>().TakeDamage(damageCalc(player.baseDamage, 0.3f, 2f, 10f, 4f), 1);
                break;
            case 2:
                other.GetComponent<IDamageable>().TakeDamage(damageCalc(player.baseDamage, 0.4f, 2f, 10f, 4f), 1);
                break;
            case 3:
                other.GetComponent<IDamageable>().TakeDamage(damageCalc(player.baseDamage, 0.5f, 2f, 10f, 4.5f), 1);
                break;
        }
        
    }
    //acerto de ataque cr�tico
    public void AtackCriticalHit(Collider other)
    {
        FMODAudioManager.instance.PlayOneShot(FMODAudioManager.instance.sword[4], player.transform.position);
        player.audioMan.PlayAudio(4);
        GameManager.instance.CallHitStop(0.35f);
        player.swordTrail.startColor = Color.red;
        player.critical.Play();
        switch (player.comboCounter)
        {
            case 1:
                other.GetComponent<IDamageable>().TakeDamage(damageCalc(player.baseDamage, 0.5f, 4f, 7.5f, 2.5f), 1);
                player.runes[player.equipedPrimaryRune].AtackCriticalEffect1();
                break;
            case 2:
                other.GetComponent<IDamageable>().TakeDamage(damageCalc(player.baseDamage, 0.6f, 4f, 7.5f, 2.5f), 1);
                player.runes[player.equipedPrimaryRune].AtackCriticalEffect2();
                break;
            case 3:
                other.GetComponent<IDamageable>().TakeDamage(damageCalc(player.baseDamage, 0.7f, 4f, 7.5f, 3.5f), 1);
                player.runes[player.equipedPrimaryRune].AtackCriticalEffect3();
                break;
        }

    }
    public void AtackHeavyEffect()
    {
        switch (player.comboCounter)
        {
            case 1:
                player.runes[player.equipedSecondaryRune].HeavyEffect1();
                break;
            case 2:
                player.runes[player.equipedSecondaryRune].HeavyEffect2();
                break;
            case 3:
                player.runes[player.equipedSecondaryRune].HeavyEffect3();
                break;
        }
    }
    //acerto de ataque pesado
    public void AtackHeavyHit(Collider other)
    {
        if (firstAtack)
        {
            switch (player.comboCounter)
            {
                case 1:
                    other.GetComponent<IDamageable>().TakeDamage(damageCalc(player.baseDamage, 0.4f, 3f, 7.5f, 6f), 1);
                    break;
                case 2:
                    other.GetComponent<IDamageable>().TakeDamage(damageCalc(player.baseDamage, 0.4f, 3f, 7.5f, 6f), 1);
                    break;
                case 3:
                    other.GetComponent<IDamageable>().TakeDamage(damageCalc(player.baseDamage, 0.4f, 3f, 7.5f, 6f), 1);
                    break;
            }
        }
        else
        {
            switch (player.comboCounter)
            {
                case 1:
                    other.GetComponent<IDamageable>().TakeDamage(damageCalc(player.baseDamage, 0.5f, 3f, 5f, 6f), 1);
                    break;
                case 2:
                    other.GetComponent<IDamageable>().TakeDamage(damageCalc(player.baseDamage, 0.6f, 3f, 5f, 6f), 1);
                    break;
                case 3:
                    other.GetComponent<IDamageable>().TakeDamage(damageCalc(player.baseDamage, 0.7f, 3f, 5f, 6f), 1);
                    break;
            }
        }
    }
    public int damageCalc(float baseDamage, float baseMultiplier, float agilityModifier, float strenghtModifier, float resistanceMultiplier)
    {
        return (int)
            ((player.damageMultiplier * baseMultiplier) *
            (baseDamage + (baseMultiplier + (player.strength * strenghtModifier) + (player.agility * agilityModifier) + (player.constitution * resistanceMultiplier))
            + player.damageAdd));
    }
}
