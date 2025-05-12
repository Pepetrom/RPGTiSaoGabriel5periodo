using UnityEngine;

public class A_AtackDash : IAction
{
    public int slot;
    float dashForce;
    float dashTime;
    bool dashing;
    Vector3 direction;
    public void SetSlot(int slot)
    {
        this.slot = slot;
    }
    public void ActionStart()
    {
        dashTime = 1;
        dashing = true;
        dashForce = PlayerController.instance.baseDashForce;
        PlayerController.instance.dustParticle.Play();
        if(PlayerController.instance.target != null)
        {
            direction = PlayerController.instance.target.position - PlayerController.instance.model.transform.position;
            direction.y = 0;
            direction = direction.normalized * dashForce * Time.fixedDeltaTime * PlayerController.instance.moveSpeed;
        }
        else
        {
            direction = PlayerController.instance.model.transform.forward * dashForce * Time.fixedDeltaTime * PlayerController.instance.moveSpeed;
        }
    }
    public void ActionUpdate()
    {
        if (!dashing) return;

        if (dashTime <= 0)
        {
            ActionEnd();
            return;
        }
        PlayerController.instance.moveDirection += direction;
        dashTime -= Time.fixedDeltaTime * 8;
    }
    public void ActionEnd()
    {
        dashing = false;
    }

}
