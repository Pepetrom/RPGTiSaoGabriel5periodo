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
        controller.animator.SetBool("attack3", true);
        controller.damage = 40f;
        controller.agent.speed = 0f;
        impulseApplied = false;
    }

    public void OnExit()
    {
        controller.hashitted = false;
    }

    public void OnUpdate()
    {
        if (controller.playerHit)
        {
            controller.SetState(new PorquinStunState(controller));
            controller.playerHit = false;
            return;
        }
        if (controller.active)
        {
            controller.sword.SetActive(true);
            controller.sword2.SetActive(true);
        }
        else
        {
            controller.sword.SetActive(false);
            controller.sword2.SetActive(false);
        }
        if (!controller.antecipation)
            controller.RotateTowardsPlayer();
        if (controller.fullCombatCounter >= 2)
            controller.SetState(new PorquinDashState(controller));
        if (controller.attIdle)
        {
            controller.animator.SetBool("attack3", false);
            controller.SetState(new PorquinCombatIdleState(controller));
        }

    }
}
