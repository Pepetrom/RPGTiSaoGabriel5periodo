using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleCannonState : ITurtleStateMachine
{
    TurtleStateMachine controller;

    public TurtleCannonState(TurtleStateMachine controller)
    {
        this.controller = controller;
    }

    public void OnEnter()
    {
        controller.animator.SetTrigger("cannon");
        controller.agent.enabled = false;
        controller.rb.isKinematic = false;
    }

    public void OnExit()
    {
        controller.end = false;
        controller.antecipation = false;
        controller.agent.enabled = true;
        controller.rb.isKinematic = true;
    }

    public void OnUpdate()
    {
        if (!controller.antecipation)
        {
            controller.RotateTowardsPlayer(10);
        }
        if (controller.end)
        {
            controller.SetState(new TurtleCombatIdleState(controller));
        }
    }
}
