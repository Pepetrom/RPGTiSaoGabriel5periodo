using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroIdle : IKrokodil
{
    KrokodilFSM controller;
    public KroIdle(KrokodilFSM controller) { this.controller = controller; }
    public void OnEnter()
    {
        controller.animator.SetBool("heavy", false);
        controller.animator.SetBool("att1", false);
        controller.animator.SetBool("att2", false);
        controller.animator.SetBool("isAttack", false);
        controller.end = false;
        controller.SortNumber();
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if (controller.canDoSecondStage)
        {
            controller.maxPosture += controller.maxPosture / 4;
            controller.posture = controller.maxPosture;
            controller.animator.SetTrigger("secondStage");
            controller.SetState(new KroSecondStage(controller));
            controller.canDoSecondStage = false;
            return;
        }
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
