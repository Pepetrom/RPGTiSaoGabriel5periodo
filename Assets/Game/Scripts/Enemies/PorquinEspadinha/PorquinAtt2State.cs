using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorquinAtt2State : IPorquinStateMachine
{
    PorquinStateMachine controller;
    private bool impulseApplied;
    public PorquinAtt2State(PorquinStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.damage = 30f;
        controller.SortNumber();
        controller.attack2Counter++;
        controller.agent.speed = 0f;
        controller.animator.SetBool("attack2", true);
        impulseApplied = false;
    }

    public void OnExit()
    {
        controller.antecipation = false;
        controller.attIdle = false;
        controller.impulse = false;
        controller.combo = false;
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
        if(controller.active)
        {
            controller.sword.SetActive(true);
        }
        else
        {
            controller.sword.SetActive(false);
        }
        if (!controller.antecipation)
            controller.RotateTowardsPlayer();
        if(controller.sortedNumber >= 0.6f)
        {
            if(controller.attIdle)
                controller.SetState(new PorquinCombatIdleState(controller));
        }
        else
        {
            if (controller.combo)
            {
                controller.fullCombatCounter++;
                controller.animator.SetBool("att2att3", true);
                controller.SetState(new PorquinAtt3State(controller));
            }
        }

    }
}
