using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorquinWalkState : IPorquinStateMachine
{
    PorquinStateMachine controller;
    public PorquinWalkState(PorquinStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.SortNumber();
        controller.agent.speed = 3.5f;
    }

    public void OnExit()
    {
        controller.agent.speed = 0f;
        //Debug.Log("Sai");
    }

    public void OnUpdate()
    {
        controller.agent.SetDestination(controller.player.transform.position);
        controller.RotateTowardsPlayer();
        if(controller.TargetDir().magnitude < controller.meleeRange)
        {
            if (controller.sortedNumber <= 0.3f)
            {
                controller.SetState(new PorquinAtt3State(controller));
            }
            else if (controller.sortedNumber > 0.3f && controller.sortedNumber < 0.6f)
            {
                controller.SetState(new PorquinAtt1State(controller));
            }
            else
            {
                controller.SetState(new PorquinAtt2State(controller));
            }
        }
        if (controller.TargetDir().magnitude > controller.patrolRange * 2)
        {
            controller.SetState(new PorquinPatrolState(controller));
        }
    }
}
