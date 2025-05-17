using UnityEngine;

public class A_KnockBack : IAction
{
    public int slot;
    float knockBackForce;
    float knockBackTime;
    bool knockBacking = false;
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
        Debug.Log("sou eu dio!");
        PlayerController.instance.cc.Move(direction * Time.fixedDeltaTime);
        knockBackTime -= Time.fixedDeltaTime;
        if (knockBackTime <= 0)
        {
            //PlayerController.instance.ResetAllActions();
            ActionEnd();
            return;
        }
    }
    public void ActionEnd()
    {
        PlayerController.instance.ResetAllActions();
        knockBacking = false;
    }

}
