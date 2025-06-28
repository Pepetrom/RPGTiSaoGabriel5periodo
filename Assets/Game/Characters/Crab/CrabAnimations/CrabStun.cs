using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabStun : ICrabInterface
{
    CrabFSM controller;
    public CrabStun(CrabFSM controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        if(UIItems.instance.bossCurrentHP <= 0)
        {
            controller.animator.Play("Crab_Death");
            controller.SetState(new CrabDeath(controller));
            return;
        }
        controller.posture = controller.maxPosture;
        controller.animator.SetBool("att1", false);
        controller.animator.SetBool("att2", false);
        controller.animator.SetBool("att3", false);
        controller.animator.SetBool("att1att2", false);
        controller.animator.SetBool("att2att3", false);
        controller.animator.SetBool("attFurnace", false);
        controller.animator.SetBool("isJumping", false);
        controller.agent.enabled = true;
        controller.rb.isKinematic = true;
    }

    public void OnExit()
    {
        controller.end = false;
    }

    public void OnUpdate()
    {
        if (controller.end)
        {
            controller.animator.SetBool("isStunned", false);
            controller.SetState(new CrabIdleState(controller));
        }
    }
}
