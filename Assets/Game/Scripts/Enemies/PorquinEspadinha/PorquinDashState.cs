using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorquinDashState : IPorquinStateMachine
{
    PorquinStateMachine controller;
    public PorquinDashState(PorquinStateMachine controller)
    {
        this.controller = controller;
    }

    public void OnEnter()
    {
        controller.rb.isKinematic = true;
        controller.playerHit = false;
        controller.selfCollider.enabled = false;
    }

    public void OnExit()
    {
        controller.selfCollider.enabled = true;
    }

    public void OnUpdate()
    {
        if (controller.active)
        {
            controller.agent.enabled = false;
            controller.rb.isKinematic = false;
            //controller.KB(-20);
        }
        else
        {
            controller.agent.enabled = true;
            controller.rb.isKinematic = true;
        }
        if (controller.attIdle)
        {
            controller.animator.SetBool("isDashing", false);
            controller.SetState(new PorquinCombatIdleState(controller));
        }
    }
}
