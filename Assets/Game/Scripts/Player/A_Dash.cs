using UnityEngine;

public class A_Dash : IAction
{
    public int slot;
    public float dashForce;
    public float dashCooldown;
    float dashTime;
    float dashTimer;
    bool dashing;
    public void SetSlot(int slot)
    {
        this.slot = slot;
        dashForce = PlayerController.instance.baseDashForce;
        dashCooldown = PlayerController.instance.baseDashCooldown;
    }
    public void ActionStart()
    {
        PlayerController.instance.canDoAction[slot] = false;
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
            PlayerController.instance.canDoAction[slot] = false;
            PlayerController.instance.moveDirection += PlayerController.instance.model.transform.forward * dashForce * 5 * Time.fixedDeltaTime;
            dashTime -= Time.fixedDeltaTime * 4;
        }
        else
        {
            if (dashTimer < dashCooldown)
            {
                dashTimer += Time.fixedDeltaTime;
                if (dashTimer >= dashCooldown)
                {
                    PlayerController.instance.canDoAction[slot] = true;
                }
            }
        }
    }
    public void ActionEnd()
    {
        PlayerController.instance.canDoAction[slot] = true;
        dashing = false;
        dashTimer = 0;
    }
}
