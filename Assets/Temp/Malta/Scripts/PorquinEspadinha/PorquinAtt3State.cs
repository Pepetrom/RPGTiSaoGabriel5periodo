using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorquinAtt3State : IPorquinStateMachine
{
    PorquinStateMachine controller;
    private bool impulseApplied;
    public PorquinAtt3State(PorquinStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.agent.speed = 0f;
        impulseApplied = false;
        controller.animator.SetBool("attack3", true);
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
        if (controller.playerHit)
        {
            controller.SetState(new PorquinStunState(controller));
            controller.playerHit = false;
            return;
        }
        if (!controller.antecipation)
            controller.RotateTowardsPlayer();
        if (controller.attIdle)
        {
            controller.animator.SetBool("attack3", false);
            controller.SetState(new PorquinCombatIdleState(controller));
        }

    }
}
