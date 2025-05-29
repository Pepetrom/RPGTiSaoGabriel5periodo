using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroIdle : IKrokodil
{
    KrokodilFSM controller;
    public KroIdle(KrokodilFSM controller) { this.controller = controller; }
    public void OnEnter()
    {
        controller.ownCollider.enabled = false;
        controller.SortNumber();
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if (controller.TargetDir().magnitude < controller.meleeRange)
        {
            controller.animator.SetBool("isAttack", true);
            controller.SetState(new KroAttController(controller));
        }
        else if (controller.TargetDir().magnitude > controller.meleeRange && controller.TargetDir().magnitude < controller.maxRange)
        {
            if (controller.randomValue > 70)
            {
                controller.animator.SetBool("isWalking", true);
                controller.SetState(new KroWalk(controller));
            }
            else
            {
                // fuzzy de um ataque específico
                controller.animator.SetBool("isAttack", true);
                controller.SetState(new KroAttController(controller));
            }
        }
        else
        {
            controller.animator.SetBool("isAttack", true);
            controller.SetState(new KroAttController(controller));
        }
    }
}
