using UnityEngine;

public class A_SideStep : IAction
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
        PlayerController.instance.particle.gameObject.SetActive(true);
    }
    public void DoAction()
    {
        if (dashing)
        {
            if (dashTime <= 0)
            {
                ActionEnd();
                return;
            }
            PlayerController.instance.canDoAction[slot] = false;
            PlayerController.instance.moveDirection += PlayerController.instance.moveDirection * dashForce * Time.fixedDeltaTime;
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
        PlayerController.instance.particle.gameObject.SetActive(false);
        dashTimer = 0;
    }

}
