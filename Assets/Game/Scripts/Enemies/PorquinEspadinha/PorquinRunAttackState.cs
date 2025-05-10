using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorquinRunAttackState : IPorquinStateMachine
{
    PorquinStateMachine controller;
    public PorquinRunAttackState(PorquinStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.rb.isKinematic = true;
        controller.damage = 35;
        controller.run = true;
    }

    public void OnExit()
    {
        controller.animator.SetBool("runAttack", false);
        controller.run = true;
    }

    public void OnUpdate()
    {
        if (controller.active)
        {
            controller.sword.SetActive(true);
            controller.agent.enabled = false;
            controller.rb.isKinematic = false;
            controller.KB(200);
        }
        else
        {
            controller.sword.SetActive(false);
            controller.agent.enabled = true;
            controller.rb.isKinematic = true;
        }
        if (!controller.run)
        {
            controller.agent.speed = 0.5f;
        }
        else
        {
            controller.agent.speed = 8f;
            controller.agent.SetDestination(controller.player.transform.position);
        }
        if (!controller.antecipation)
        {
            controller.RotateTowardsPlayer();
        }
        if (controller.attIdle)
        {
            controller.SetState(new PorquinCombatIdleState(controller));
        }
    }
}
