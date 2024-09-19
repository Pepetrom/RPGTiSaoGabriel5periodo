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
        controller.agent.speed = 5f;
    }

    public void OnExit()
    {
        controller.animator.SetBool("IsWalking", false);
    }

    public void OnUpdate()
    {
        controller.agent.SetDestination(controller.player.transform.position);
        if(controller.TargetDir().magnitude <= controller.meleeRange)
        {
            controller.animator.SetBool("Attack1", true);
            controller.agent.speed = 1.5f;
            controller.SetState(new TurtleAtt1State(controller));
        }
        if (controller.TargetDir().magnitude >= controller.minCannonRange && controller.TargetDir().magnitude <= controller.maxCannonRange)
        {
            controller.agent.speed = 0f;
            controller.SetState(new TurtleCannonState(controller));
        }
    }
}
