using UnityEngine;

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
        if (!PlayerController.instance.canDoAtack || StaminaBar.intance.currentStam < PlayerController.instance.stamPerHit)
        {
            StoreCommand(0);
            return;
        }
        isHeavyAtack = heavy;
        StaminaBar.intance.DrainStamina(PlayerController.instance.stamPerHit);
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
                if (isHeavyAtack)
                {
                    AtackHeavyStart();
                }
                else
                {
                    switch (PlayerController.instance.comboCounter)
                    {
                        case 1:
                            Atack1Start();
                            break;
                        case 2:
                            Atack2Start();
                            break;
                        case 3:
                            Atack3Start();
                            break;
                    }
                }
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
        PlayerController.instance.runes[PlayerController.instance.actualRune].HitEffect();
    }
    public void OpenComboWindow()
    {
        if (interrupted) return;
        PlayerController.instance.comboCounter++;
        if (PlayerController.instance.comboCounter > comboSize) PlayerController.instance.comboCounter = 1;
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
        if (interrupted) return;
        PlayerController.instance.canDoAtack = true;
        PlayerController.instance.canMove = true;
        //PlayerController.instance.comboCounter = 1;
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
        PlayerController.instance.canDoAtack = true;
        PlayerController.instance.animator.SetBool("Atacking", false);
        PlayerController.instance.comboCounter = 1;
        storedCommand = -1;
        PlayerController.instance.swordTrail.emitting = false;
    }
    public void StoreCommand(int which)
    {
        storedCommand = which;
    }
    public void Atack1Start()
    {
        PlayerController.instance.actions[1].ActionStart();
    }
    public void Atack2Start()
    {
        PlayerController.instance.actions[1].ActionStart();
    }
    public void Atack3Start()
    {
    }
    public void AtackHeavyStart()
    {
        if (firstAtack)
        {
            PlayerController.instance.actions[1].ActionStart();
            Debug.Log("AtackHeavyBaseStart");
        }
        else
        {
            switch (PlayerController.instance.comboCounter)
            {
                case 1:
                    PlayerController.instance.actions[1].ActionStart();
                    Debug.Log("AtackHeavyCombo1Start");
                    break;
                case 2:
                    PlayerController.instance.actions[1].ActionStart();
                    Debug.Log("AtackHeavyCombo2Start");
                    break;
                case 3:
                    PlayerController.instance.actions[1].ActionStart();
                    Debug.Log("AtackHeavyCombo3Start");
                    break;
            }
        }
    }

    public void Atack1Hit()
    {
    }

    public void Atack2Hit()
    {
    }

    public void Atack3Hit()
    {
    }

    public void AtackHeavyHit()
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
                    Atack1Start();
                    break;
                case 2:
                    Atack2Start();
                    break;
                case 3:
                    Atack3Start();
                    break;
            }
        }
    }
}
