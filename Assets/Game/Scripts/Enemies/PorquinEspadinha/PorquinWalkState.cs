using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorquinWalkState : IPorquinStateMachine
{
    PorquinStateMachine controller;
    float a;
    float fuzzificado;
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
    }

    public void OnUpdate()
    {
        if (controller.playerHit)
        {
            controller.SetState(new PorquinStunState(controller));
            return;
        }
        controller.agent.SetDestination(controller.player.transform.position);
        controller.RotateTowardsPlayer();
        if (controller.TargetDir().magnitude > controller.meleeRange && controller.TargetDir().magnitude < controller.meleeRange + 5)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(controller.fuzzyDash < controller.minDash)
                {
                    controller.animator.SetBool("isWalking", false);
                    controller.SetState(new PorquinCombatIdleState(controller));
                }
                else if(controller.fuzzyDash > controller.maxDash)
                {
                    controller.animator.SetBool("isDashing", true);
                    controller.SetState(new PorquinDashState(controller));
                }
                else 
                {
                    a = Random.Range(0f, 1f);
                    Debug.Log(a);
                    fuzzificado = controller.FuzzyLogic(controller.fuzzyDash,controller.minDash, controller.maxDash);
                    if(a > fuzzificado)
                    {
                        controller.animator.SetBool("isWalking", false);
                        controller.SetState(new PorquinCombatIdleState(controller));
                    }
                    else
                    {
                        controller.animator.SetBool("isDashing", true);
                        controller.SetState(new PorquinDashState(controller));
                    }

                }
            }      
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                controller.animator.SetBool("isDashing", true);
                controller.SetState(new PorquinDashState(controller));
            }
        }
        if(controller.TargetDir().magnitude < controller.meleeRange)
        {
            controller.animator.SetBool("isWalking", false);
            controller.SetState(new PorquinCombatIdleState(controller));
        }
        if (controller.TargetDir().magnitude > controller.patrolRange * 2)
        {
            controller.SetState(new PorquinPatrolState(controller));
        }
    }
}
