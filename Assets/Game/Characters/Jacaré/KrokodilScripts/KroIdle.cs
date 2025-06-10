using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroIdle : IKrokodil
{
    KrokodilFSM controller;
    public KroIdle(KrokodilFSM controller) { this.controller = controller; }
    public void OnEnter()
    {
        controller.end = false;
        controller.ownCollider.enabled = false;
        controller.SortNumber();
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if (controller.TargetDir().magnitude <= controller.meleeRange)
        {
            controller.animator.SetBool("isAttack", true);
            controller.SetState(new KroAttController(controller));
        }
        else
        {
            if(controller.randomValue > controller.swingRate)
            {
                controller.animator.SetBool("swing", true);
                controller.SetState(new KroSwing(controller));
            }
            else
            {
                controller.animator.SetBool("isWalking", true);
                controller.SetState(new KroWalk(controller));
            }
        }
    }
}
