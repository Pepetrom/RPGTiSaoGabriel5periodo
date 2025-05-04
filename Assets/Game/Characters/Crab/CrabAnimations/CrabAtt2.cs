using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class CrabAtt2 : ICrabInterface
{
    CrabFSM controller;
    public CrabAtt2(CrabFSM controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.animator.SetBool("att2", true);
        controller.rb.isKinematic = true;
        controller.damage = 30;
        controller.ActivateTrails(true,false);
    }

    public void OnExit()
    {
        controller.end = false;
        controller.antecipation = false;
        controller.combo = false;
        controller.hashitted = false;
        controller.ActivateTrails(false, false);
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
            controller.KB(600);
        }
        else
        {
            controller.claw1.enabled = false;
            controller.agent.enabled = true;
            controller.rb.isKinematic = true;
        }
        if (controller.combo)
        {
            if (controller.TargetDir().magnitude <= controller.minRange)
            {
                controller.animator.SetBool("att2", false);
                controller.SetState(new CrabAttController(controller));
            }
            else if (controller.TargetDir().magnitude <= controller.meleeRange + controller.meleeRange / 2)
            {
                if (controller.randomValue <= 100)
                {
                    controller.animator.SetBool("att2att3", true);
                    controller.SetState(new CrabAtt3(controller));
                }
            }
        }
        if (controller.end)
        {
            controller.animator.SetBool("att2", false);
            controller.SetState(new CrabAttController(controller));
        }
    }
}
