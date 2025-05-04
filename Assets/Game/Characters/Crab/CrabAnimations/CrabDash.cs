using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabDash : ICrabInterface
{
    CrabFSM controller;
    int count;
    int value;
    public CrabDash(CrabFSM controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.rb.isKinematic = true;
    }

    public void OnExit()
    {
        controller.jump = false;
        controller.end = false;
        controller.eventS = false;
        controller.maxDash = 95;
        if (count >= 2)
        {
            controller.maxDash -= 30;
            count = 0;
        }

    }

    public void OnUpdate()
    {
        if (controller.jump)
        {
            controller.agent.enabled = false;
            controller.rb.isKinematic = false;
            controller.KB(-1600);
        }
        else
        {
            controller.agent.enabled = true;
            controller.rb.isKinematic = true;
        }
        if (controller.eventS)
        {
            controller.VFXSmallConcreteBL.Play();
            controller.VFXSmallConcreteBR.Play();
        }
        if (controller.end)
        {
            controller.animator.SetBool("isDashing", false);
            controller.SetState(new CrabAttController(controller));
        }
    }
}
