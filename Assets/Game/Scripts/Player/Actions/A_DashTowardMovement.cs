using UnityEngine;

public class A_DashTowardMovement : IAction
{
    public int slot;
    float dashForce;
    float dashCooldown;
    float dashTime;
    float dashTimer;
    bool dashing;
    Vector3 direction;
    PlayerController player;
    public void SetSlot(int slot)
    {
        this.slot = slot;
        player = PlayerController.instance;
    }
    public void ActionStart()
    {
        if (!player.canDoAction[slot]) return;
        if (StaminaBar.instance.currentStam < player.stamPerHit) return;
        if(player.isAttacking && player.atacks[0].CanBeInterupted()) player.atacks[0].InteruptAtack();

        StaminaBar.instance.DrainStamina(player.stamPerHit * 2);

        player.animator.SetTrigger("Dash");
        HPBar.instance.StartCoroutine(HPBar.instance.InvulnableTime());
        dashForce = player.baseDashForce * 12;
        dashCooldown = player.baseDashCooldown;
        player.canDoAction[slot] = false;
        dashTime = 1;
        dashing = true;
        dashTimer = 0;
        player.dustParticle.Play();
        FMODAudioManager.instance.PlayOneShot(FMODAudioManager.instance.dash,player.transform.position);    

        direction = player.moveDirection;
        direction.y = 0;
        direction = direction.normalized;
        if (direction == Vector3.zero)
        {
            direction = -player.model.transform.forward * dashForce * Time.fixedDeltaTime ;
        }
        else
        {
            direction = direction * dashForce * Time.fixedDeltaTime;
        }
            Debug.Log(direction);
        direction.y = 0;
    }
    public void ActionUpdate()
    {
        if (dashing)
        {
            if (dashTime <= 0)
            {
                ActionEnd();
                return;
            }
            player.canDoAction[slot] = false;
            player.moveDirection += direction;
            dashTime -= Time.fixedDeltaTime * 4;
            dashTimer = 0;
        }
        else
        {
            if (dashTimer < dashCooldown)
            {
                dashTimer += Time.fixedDeltaTime;
                if (dashTimer >= dashCooldown)
                {
                    dashTimer = 0;
                    player.canDoAction[slot] = true;
                }
            }
        }
    }
    public void ActionEnd()
    {
        dashing = false;
        dashTimer = 0;
        player.ResetAllActions();
    }

}
