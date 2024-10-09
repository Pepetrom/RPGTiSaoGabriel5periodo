using UnityEngine;

public class A_KnockBack : IAction
{
    public int slot;
    float knockBackForce;
    float knockBackTime;
    bool knockBacking;
    Vector3 direction;
    public void SetSlot(int slot)
    {
        this.slot = slot;
    }
    public void ActionStart()
    {
        knockBackTime = 1;
        knockBacking = true;
        knockBackForce = PlayerController.instance.baseDashForce;
        PlayerController.instance.bloodParticle.Play();
        direction = PlayerController.instance.transform.position - PlayerController.instance.damageFont.transform.position ;
        direction.y = 0;
        direction = direction.normalized * knockBackForce * Time.fixedDeltaTime * PlayerController.instance.moveSpeed * 2;
    }
    public void ActionUpdate()
    {
        if (!knockBacking) return;

        if (knockBackTime <= 0)
        {
            ActionEnd();
            return;
        }
        PlayerController.instance.moveDirection += direction;
        knockBackTime -= Time.fixedDeltaTime * 8;
    }
    public void ActionEnd()
    {
        knockBacking = false;
    }

}
