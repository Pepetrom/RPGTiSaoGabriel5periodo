using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabSpinState : ICrabInterface
{
    CrabFSM controller;
    public CrabSpinState(CrabFSM controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.rb.isKinematic = true;
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
            controller.RotateTowardsPlayer(6);
        }
        if (controller.jump)
        {
            controller.agent.enabled = false;
            controller.rb.isKinematic = false;
            controller.KB(10);
        }
        else
        {
            controller.agent.enabled = true;
            controller.rb.isKinematic = true;
        }
        if (controller.activate)
        {
            controller.jumpCollider.enabled = true;
        }
        else
        {
            controller.jumpCollider.enabled = false;
        }
        if (controller.end)
        {
            controller.animator.SetBool("isSpin", false);
            controller.SetState(new CrabAttController(controller));
        }
    }
}
