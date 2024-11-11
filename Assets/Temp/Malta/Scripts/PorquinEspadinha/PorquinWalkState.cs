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
        controller.agent.speed = 3.5f;
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        controller.agent.SetDestination(controller.player.transform.position);
        controller.RotateTowardsPlayer();
        if(controller.TargetDir().magnitude < controller.meleeRange)
        {
            controller.animator.SetBool("isWalking", false);
            controller.SetState(new PorquinCombatIdleState(controller));
        }
        else if (controller.TargetDir().magnitude > controller.patrolRange * 2)
        {
            Debug.Log("Patrulha");
            controller.SetState(new PorquinPatrolState(controller));
        }
    }
}
