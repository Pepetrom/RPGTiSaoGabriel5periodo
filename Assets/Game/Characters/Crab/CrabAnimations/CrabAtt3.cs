using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabAtt3 : ICrabInterface
{
    CrabFSM controller;
    public CrabAtt3(CrabFSM controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.animator.SetBool("att3", true);
        controller.rb.isKinematic = true;
        controller.damage = 40;
    }
    public void OnExit()
    {
        controller.end = false;
        controller.antecipation = false;
        controller.hashitted = false;
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
            controller.claw2.enabled = true;
            controller.agent.enabled = false;
            controller.rb.isKinematic = false;
            controller.KB(1200);
        }
        else
        {
            controller.claw1.enabled = false;
            controller.claw2.enabled = false;
            controller.agent.enabled = true;
            controller.rb.isKinematic = true;
        }
        if (controller.end)
        {
            controller.animator.SetBool("att3", false);
            controller.SetState(new CrabAttController(controller));
        }
    }
}
