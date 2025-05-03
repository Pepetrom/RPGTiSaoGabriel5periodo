using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabAttRun : ICrabInterface
{
    CrabFSM controller;
    public CrabAttRun(CrabFSM controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.rb.isKinematic = true;
        controller.damage = 40;
        controller.agent.speed = 8;
    }

    public void OnExit()
    {
        controller.end = false;
        controller.antecipation = false;
        controller.combo = false;
        controller.hashitted = false;
        controller.jump = false;
    }

    public void OnUpdate()
    {
        if (!controller.antecipation)
        {
            controller.agent.SetDestination(controller.player.transform.position);
        }
        if (!controller.jump)
        {
            controller.RotateTowardsPlayer(6);
        }
        if (controller.activate)
        {
            controller.claw2.enabled = true;
            controller.agent.enabled = false;
            controller.rb.isKinematic = false;
            controller.KB(900);
        }
        else
        {
            controller.claw2.enabled = false;
            controller.agent.enabled = true;
            controller.rb.isKinematic = true;
        }
        if (controller.end)
        {
            controller.animator.SetBool("attRun", false);
            controller.SetState(new CrabAttController(controller));
        }
    }
}
