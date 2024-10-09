using UnityEngine;

public class A_AtackDash : IAction
{
    public int slot;
    float dashForce;
    float dashTime;
    bool dashing;
    public void SetSlot(int slot)
    {
        this.slot = slot;
        dashForce = PlayerController.instance.baseDashForce;
    }
    public void ActionStart()
    {
        dashTime = 1;
        dashing = true;
        PlayerController.instance.particle.Play();
    }
    public void ActionUpdate()
    {
        if (!dashing) return;

        if (dashTime <= 0)
        {
            ActionEnd();
            return;
        }
        PlayerController.instance.moveDirection += PlayerController.instance.model.transform.forward * dashForce * Time.fixedDeltaTime * PlayerController.instance.moveSpeed;
        dashTime -= Time.fixedDeltaTime * 8;
    }
    public void ActionEnd()
    {
        dashing = false;
    }

}
