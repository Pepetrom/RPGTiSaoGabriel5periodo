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
            if(controller.att2Count > 2)
            {
                controller.animator.SetBool("att1", true);
                controller.SetState(new KroAtt1(controller));
            }
            else
            {
                if(controller.randomValue > 40)
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
