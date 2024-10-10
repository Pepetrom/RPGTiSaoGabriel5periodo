using UnityEngine;

public class A_Jump : IAction
{
    int slot;
    float jumpForce;
    float airTime;
    float jumpTime;
    bool jumping;
    public void SetSlot(int slot)
    {
        this.slot = slot;
        airTime = PlayerController.instance.baseAirTime;
        jumpForce = PlayerController.instance.baseJumpForce;
    }
    public void ActionStart()
    {
        PlayerController.instance.jumping = true;
        PlayerController.instance.canDoAction[slot] = false;
        jumpTime = airTime;
        jumping = true;

    }
    public void ActionEnd()
    {
        PlayerController.instance.jumping = false;
        jumpTime = 0;
        jumping = false;
    }

    public void ActionUpdate()
    {
        if (jumping)
        {
            if (jumpTime <= 0)
            {
                ActionEnd();
                return;
            }
            PlayerController.instance.canDoAction[slot] = false;
            PlayerController.instance.moveDirection.y += jumpForce * Time.fixedDeltaTime;
            jumpTime -= Time.fixedDeltaTime;
        }
    }
}
