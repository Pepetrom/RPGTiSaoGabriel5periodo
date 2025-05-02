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
        }
        else
        {
            controller.claw1.enabled = false;
            controller.claw2.enabled = false;
        }
        if (controller.end)
        {
            controller.animator.SetBool("att3", false);
            controller.SetState(new CrabAttController(controller));
        }
    }
}
