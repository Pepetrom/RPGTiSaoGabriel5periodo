using UnityEngine;

public class A_Dash : IAction
{
    public int slot;
    public float dashForce;
    public float dashCooldown;
    float dashTime;
    float dashTimer;
    bool dashing;
    PlayerController player;
    public void SetSlot(int slot)
    {
        this.slot = slot;
        player = PlayerController.instance;
        dashForce = player.baseDashForce;
        dashCooldown = player.baseDashCooldown;
    }
    public void ActionStart()
    {
        if (!player.canDoAction[slot]) return;
        if (StaminaBar.instance.currentStam < player.stamPerHit) return;
        StaminaBar.instance.DrainStamina(player.stamPerHit * 2);
        player.canDoAction[slot] = false;
        dashTime = 1;
        dashing = true;
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
            player.moveDirection += player.model.transform.forward * dashForce * 5 * Time.fixedDeltaTime;
            dashTime -= Time.fixedDeltaTime * 4;
        }
        else
        {
            if (dashTimer < dashCooldown)
            {
                dashTimer += Time.fixedDeltaTime;
                if (dashTimer >= dashCooldown)
                {
                    player.canDoAction[slot] = true;
                }
            }
        }
    }
    public void ActionEnd()
    {
        player.canDoAction[slot] = true;
        dashing = false;
        dashTimer = 0;
    }
}
