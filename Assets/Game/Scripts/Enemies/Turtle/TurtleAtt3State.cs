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
        controller.damage = 35;
        controller.rb.isKinematic = false;
    }

    public void OnExit()
    {
        controller.active = false;
        controller.end = false;
        controller.antecipation = false;
        controller.hashitted = false;
    }

    public void OnUpdate()
    {
        if (!controller.antecipation)
        {
            controller.RotateTowardsPlayer(8);
        }
        if (controller.active)
        {
            controller.rightHand.enabled = true;
            controller.rb.isKinematic = false;
            controller.agent.enabled = false;
            controller.KB(40);
        }
        else
        {
            controller.rightHand.enabled = false;
            controller.leftHand.enabled = false;
            controller.rb.isKinematic = true;
            controller.agent.enabled = true;
        }
        if (controller.end)
        {
            controller.SetState(new TurtleCombatIdleState(controller));
        }
    }
}
