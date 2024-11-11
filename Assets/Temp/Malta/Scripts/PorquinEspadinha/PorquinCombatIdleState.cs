using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorquinCombatIdleState : IPorquinStateMachine
{
    PorquinStateMachine controller;
    public PorquinCombatIdleState(PorquinStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        #region reset de bools
        controller.antecipation = false;
        controller.attIdle = false;
        controller.impulse = false;
        #endregion
        controller.agent.angularSpeed = 0f;
        controller.agent.speed = 0f;
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if (!controller.isInCombat)
        {
            controller.isInCombat = true;
            controller.animator.SetBool("patrolling", true);
            controller.SetState(new PorquinPatrolState(controller));
            return;
        }
        if (controller.TargetDir().magnitude > controller.patrolRange * 2)
        {
            Debug.Log("Patrulha");
            controller.SetState(new PorquinPatrolState(controller));
        }
        else if(controller.TargetDir().magnitude > controller.meleeRange)
        {
            controller.animator.SetBool("isWalking", true);
            controller.SetState(new PorquinWalkState(controller));
        }
        else
        {
            controller.SetState(new PorquinAtt1State(controller));
        }
    }
}
