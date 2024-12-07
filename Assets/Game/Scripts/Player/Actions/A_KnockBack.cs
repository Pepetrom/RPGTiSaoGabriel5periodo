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
        PlayerController.instance.StopAllActions();
        PlayerController.instance.animator.SetTrigger("TakeDamage");
        knockBackTime = 0.5f;
        knockBacking = true;
        knockBackForce = PlayerController.instance.baseDashForce;
        PlayerController.instance.bloodParticle.Play();
        direction = PlayerController.instance.transform.position - PlayerController.instance.damageFont.transform.position ;
        direction.y = 0;
        direction = direction.normalized * knockBackForce * Time.fixedDeltaTime * 8;
    }
    public void ActionUpdate()
    {
        if (!knockBacking) return;

        if(knockBackTime <= knockBackTime * 0.75f)
        {
            knockBacking = false;
        }
        if (knockBackTime <= 0)
        {
            ActionEnd();
            return;
        }
        PlayerController.instance.moveDirection += direction;
        knockBackTime -= Time.fixedDeltaTime;
    }
    public void ActionEnd()
    {
        knockBacking = false;
        PlayerController.instance.ResetAllActions();
    }

}
