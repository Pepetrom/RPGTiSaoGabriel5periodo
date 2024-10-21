using UnityEngine;

public class A_SideStep : IAction
{
    public int slot;
    float dashForce;
    float dashCooldown;
    float dashTime;
    float dashTimer;
    bool dashing;
    Vector3 direction;
    public void SetSlot(int slot)
    {
        this.slot = slot;
    }
    public void ActionStart()
    {
        HPBar.instance.StartCoroutine(HPBar.instance.InvulnableTime());
        dashForce = PlayerController.instance.baseDashForce;
        dashCooldown = PlayerController.instance.baseDashCooldown;
        PlayerController.instance.canDoAction[slot] = false;
        dashTime = 1;
        dashing = true;
        dashTimer = 0;
        PlayerController.instance.dustParticle.Play();
        if (PlayerController.instance.moveDirection == Vector3.zero)
        {
            direction = -PlayerController.instance.model.transform.forward * PlayerController.instance.moveSpeed * dashForce * Time.fixedDeltaTime ;
        }
        else
        {
            direction = PlayerController.instance.moveDirection * dashForce * Time.fixedDeltaTime;
        }
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
            PlayerController.instance.canDoAction[slot] = false;
            PlayerController.instance.moveDirection += direction;
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
                    Debug.Log("CanDo");
                    dashTimer = 0;
                    PlayerController.instance.canDoAction[slot] = true;
                }
            }
        }
    }
    public void ActionEnd()
    {
        dashing = false;
        dashTimer = 0;
    }

}
