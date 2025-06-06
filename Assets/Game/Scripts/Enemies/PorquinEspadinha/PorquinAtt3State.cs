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
        controller.rb.isKinematic = true;
        controller.animator.SetBool("attack3", true);
        controller.damage = 30f;
        controller.agent.speed = 0f;
        impulseApplied = false;
        controller.active = false;
    }

    public void OnExit()
    {
        controller.hashitted = false;
        controller.active = false;
    }

    public void OnUpdate()
    {
        if (controller.playerHit)
        {
            controller.sword.enabled = false;
            controller.SetState(new PorquinStunState(controller));
            controller.playerHit = false;
            return;
        }
        if (controller.active)
        {
            controller.sword.enabled = true;
            controller.agent.enabled = false;
            controller.rb.isKinematic = false;
            controller.KB(20);
        }
        else
        {
            controller.sword.enabled = false;
            controller.agent.enabled = true;
            controller.rb.isKinematic = true;
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
