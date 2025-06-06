using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleAtt3State : ITurtleStateMachine
{
    TurtleStateMachine controller;
    public TurtleAtt3State(TurtleStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.damage = 45;
        controller.animator.SetBool("Attack3", true);
        controller.rb.isKinematic = false;
    }

    public void OnExit()
    {
        controller.impulse = false;
        controller.attIdle = false;
        controller.antecipation = false;
        controller.hashitted = false;
    }

    public void OnUpdate()
    {
        if (controller.active)
        {
            controller.rightHand.gameObject.SetActive(true);
            controller.rb.isKinematic = false;
            controller.agent.enabled = false;
            controller.KB(60);
        }
        else
        {
            controller.rightHand.gameObject.SetActive(false);
            controller.rb.isKinematic = true;
            controller.agent.enabled = true;
        }
        if (!controller.antecipation)
        {
            controller.RotateTowardsPlayer(8);
        }
        if (controller.attIdle)
        {
            controller.SetState(new TurtleCombatIdleState(controller));
            controller.animator.SetBool("Attack3", false);
        }
    }
}
