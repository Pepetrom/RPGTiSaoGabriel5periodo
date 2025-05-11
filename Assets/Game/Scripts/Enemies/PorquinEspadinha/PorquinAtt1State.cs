using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class PorquinAtt1State : IPorquinStateMachine
{
    PorquinStateMachine controller;
    private bool impulseApplied;
    public PorquinAtt1State(PorquinStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.rb.isKinematic = true;
        controller.damage = 30f;
        controller.animator.SetBool("attack1", true);
        impulseApplied = false;
        controller.active = false;
    }

    public void OnExit()
    {
        controller.antecipation = false;
        controller.attIdle = false;
        controller.impulse = false;
        controller.combo = false;
        controller.hashitted = false;
        controller.active = false;
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
            controller.sword.enabled = true;
            controller.agent.enabled = false;
            controller.rb.isKinematic = false;
            controller.KB(100);
        }
        else
        {
            controller.sword.enabled = false;
            controller.agent.enabled = true;
            controller.rb.isKinematic = true;
        }
        if (!controller.antecipation)
            controller.RotateTowardsPlayer();
        if (controller.combo && controller.TargetDir().magnitude < controller.meleeRange + 5)
        {
            controller.animator.SetBool("att1att2", true);
            controller.SetState(new PorquinAtt2State(controller));
            controller.fullCombatCounter++;
        }
        else if (controller.attIdle)
        {
            controller.animator.SetBool("attack1", false);
            controller.SetState(new PorquinCombatIdleState(controller));
        }

    }
}
