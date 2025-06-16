using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleRunState : ITurtleStateMachine
{
    TurtleStateMachine controller;
    public TurtleRunState( TurtleStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.agent.speed = 12f;
        controller.agent.angularSpeed = 70f;
        controller.SortNumber();
    }

    public void OnExit()
    {
        controller.agent.speed = 0f;
        controller.agent.angularSpeed = 0f;
    }

    public void OnUpdate()
    {
        controller.agent.SetDestination(controller.player.transform.position);
        if (controller.TargetDir().magnitude > controller.patrolDistance)
        {
            controller.animator.SetBool("isRunning", false);
            controller.SetState(new TurtleCombatIdleState(controller));
        }
        if (controller.sortedNumber < 0.3f)
        {
            controller.SetState(new TurtleCannonState(controller));
        }
        else
        {
            if(controller.TargetDir().magnitude <= controller.meleeRange)
            {
                controller.animator.SetBool("isRunning", false);
                controller.SetState(new TurtleCombatIdleState(controller));
            }
        }
    }
}
