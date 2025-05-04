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
        controller.damage = 35;
        controller.SortNumber();
        controller.rb.isKinematic = true;
        controller.ActivateTrails(true,false);
    }

    public void OnExit()
    {
        controller.end = false;
        controller.antecipation = false;
        controller.combo = false;
        controller.hashitted = false;
        controller.ActivateTrails(false,false);
    }

    public void OnUpdate()
    {
        if (!controller.antecipation)
        {
            controller.RotateTowardsPlayer(10);
        }
        if (controller.activate)
        {
            controller.claw1.enabled = true;
            controller.agent.enabled = false;
            controller.rb.isKinematic = false;
            controller.KB(1300);
        }
        else
        {
            controller.claw1.enabled = false;
            controller.rb.isKinematic = true;
            controller.agent.enabled = true;
        }
        if (controller.combo)
        {
            if (controller.TargetDir().magnitude <= controller.minRange)
            {
                controller.animator.SetBool("att1", false);
                controller.SetState(new CrabAttController(controller));
            }
            else if(controller.TargetDir().magnitude <= controller.meleeRange + controller.meleeRange/2)
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
