using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabAtt1 : ICrabInterface
{
    CrabFSM controller;
    public CrabAtt1(CrabFSM controller)
    {
        this.controller = controller;
    }

    public void OnEnter()
    {
        controller.SortNumber();
    }

    public void OnExit()
    {
        controller.end = false;
        controller.antecipation = false;
        controller.combo = false;
    }

    public void OnUpdate()
    {
        if (!controller.antecipation)
        {
            controller.RotateTowardsPlayer(10);
        }
        if (controller.combo)
        {
            if(controller.TargetDir().magnitude <= controller.meleeRange + controller.meleeRange/2)
            {
                if (controller.randomValue <= 100)
                {
                    controller.animator.SetBool("att1att2", true);
                    controller.SetState(new CrabAtt2(controller));
                }
            }
        }
        if (controller.end)
        {
            controller.animator.SetBool("att1", false);
            controller.SetState(new CrabAttController(controller));
        }
    }
}
