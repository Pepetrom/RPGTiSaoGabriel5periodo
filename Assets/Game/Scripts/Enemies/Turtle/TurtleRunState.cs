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
        controller.animator.SetBool("IsRunning", true);
        controller.agent.speed = 12f;
        controller.agent.angularSpeed = 70f;
        controller.SortNumber();
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        controller.agent.SetDestination(controller.player.transform.position);
        if (controller.TargetDir().magnitude > controller.patrolDistance)
        {
            controller.agent.speed = 0f;
            controller.agent.angularSpeed = 0f;
            controller.SetState(new TurtlePatrolState(controller));
        }
        if (controller.sortedNumber < 0.3f)
        {
            controller.agent.speed = 0f;
            controller.agent.angularSpeed = 0f;
            controller.SetState(new TurtleCannonState(controller));
        }
        else
        {
            if(controller.TargetDir().magnitude <= controller.meleeRange)
            {
                controller.animator.SetBool("IsRunning", false);
                controller.agent.speed = 0f;
                controller.agent.angularSpeed = 0f;
                controller.SetState(new TurtleCombatIdleState(controller));
            }
        }
    }
}
