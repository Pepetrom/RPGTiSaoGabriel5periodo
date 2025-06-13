using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroAttController : IKrokodil
{
    KrokodilFSM controller;
    public KroAttController(KrokodilFSM controller) { this.controller = controller; }

    public void OnEnter()
    {
        controller.animator.SetBool("att2", false);
        controller.animator.SetBool("swing", false);
        controller.animator.SetBool("att2comb2", false);
        controller.animator.SetBool("heavy", false);
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
        if (controller.TargetDir().magnitude < controller.meleeRange + 4)
        {
            if (controller.randomValue > controller.jumpRate)
            {
                controller.SetState(new KroJump(controller));
            }
            else
            {
                if (controller.randomValue > controller.basicAtt)
                {
                    controller.animator.SetBool("att2", true);
                    controller.SetState(new KroAtt2(controller));
                }
                else
                {
                    controller.animator.SetBool("att1", true);
                    controller.SetState(new KroAtt1(controller));
                }
            }
        }
        else
        {
            controller.animator.SetBool("isAttack", false);
            controller.SetState(new KroIdle(controller));
        }
    }
}
