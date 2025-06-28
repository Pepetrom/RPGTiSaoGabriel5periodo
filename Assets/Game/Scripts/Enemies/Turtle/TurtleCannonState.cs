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
    }

    public void OnExit()
    {
        controller.end = false;
        controller.antecipation = false;
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
