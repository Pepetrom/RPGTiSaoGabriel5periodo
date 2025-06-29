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
        if(controller.posture <= 0)
        {
            controller.animator.Play("Crab_Stun");
            controller.SetState(new CrabStun(controller));
            return;
        }
        controller.rb.isKinematic = true;
        controller.ownCollider.enabled = false;
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
        controller.ownCollider.enabled = true;

    }

    public void OnUpdate()
    {
        if (controller.jump)
        {
            controller.agent.enabled = false;
            controller.rb.isKinematic = false;
            controller.KB(-200);
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
