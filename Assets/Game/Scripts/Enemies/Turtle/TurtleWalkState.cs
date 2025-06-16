using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleWalkState : ITurtleStateMachine
{
    TurtleStateMachine controller;
    public TurtleWalkState(TurtleStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.agent.speed = 5;
    }

    public void OnExit()
    {
        controller.agent.speed = 0;
    }

    public void OnUpdate()
    {
        controller.agent.SetDestination(controller.player.transform.position);
        controller.RotateTowardsPlayer(6);
        if (controller.playerHit)
        {
            controller.SetState(new TurtleStunState(controller));
            controller.playerHit = false;
            return;
        }
        if(controller.TargetDir().magnitude <= controller.meleeRange)
        {
            controller.animator.SetBool("isWalking", false);
            controller.SetState(new TurtleCombatIdleState(controller));
        }
        else if(controller.TargetDir().magnitude > controller.minCannonRange)
        {
            controller.animator.SetBool("isWalking", false);
            controller.SetState(new TurtleCombatIdleState(controller));
        }
    }
}
