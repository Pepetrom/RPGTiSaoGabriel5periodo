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
        controller.playerHit = false;
        controller.KB(8);
        controller.selfCollider.enabled = false;
    }

    public void OnExit()
    {
        controller.selfCollider.enabled = true;
    }

    public void OnUpdate()
    {
        if (controller.attIdle)
        {
            controller.animator.SetBool("isDashing", false);
            controller.SetState(new PorquinCombatIdleState(controller));
        }
    }
}
