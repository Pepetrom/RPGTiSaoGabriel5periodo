using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroAttController : IKrokodil
{
    KrokodilFSM controller;
    public KroAttController(KrokodilFSM controller) { this.controller = controller; }

    public void OnEnter()
    {
        controller.SortNumber();
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if(controller.TargetDir().magnitude < controller.meleeRange + 4)
        {
            controller.animator.SetBool("kick", true);
            controller.SetState(new KroKick(controller));
        }
        else
        {
            controller.animator.SetBool("isAttack", false);
            controller.SetState(new KroIdle(controller));
        }
    }
}
