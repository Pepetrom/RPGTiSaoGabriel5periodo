using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorquinSwingState : IPorquinStateMachine
{
    PorquinStateMachine controller;
    Vector3 swingPos;
    float a, fuzzificado;
    public PorquinSwingState(PorquinStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.agent.speed = 2f;
        swingPos = controller.Swing();
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        controller.agent.SetDestination(swingPos);
        controller.RotateTowardsPlayer();
        controller.SwingMove();
        if(controller.TargetDir().magnitude < controller.meleeRange)
        {
            controller.animator.SetBool("isSwing", false);
            controller.SetState(new PorquinCombatControllerState(controller));
        }
        if (controller.HasReachedDestination())
        {
            if(controller.fuzzySwing < controller.minSwing)
            {
                controller.animator.SetBool("isSwing", false);
                controller.animator.SetBool("isWalking", true);
                controller.SetState(new PorquinWalkState(controller));
            }
            else if (controller.fuzzySwing > controller.maxSwing)
            {
                controller.animator.SetBool("isSwing", false);
                controller.SetState(new PorquinCombatIdleState(controller));
            }
            else
            {
                a = Random.Range(0f, 1f);
                fuzzificado = controller.FuzzyLogic(controller.fuzzySwing, controller.minSwing, controller.maxSwing);
                if (a > fuzzificado/controller.TargetDir().magnitude)
                {
                    controller.animator.SetBool("isSwing", false);
                    controller.animator.SetBool("isWalking", true);
                    controller.SetState(new PorquinWalkState(controller));
                }
                else
                {
                    controller.animator.SetBool("isSwing", false);
                    controller.SetState(new PorquinCombatIdleState(controller));
                }
            }
        }
    }
}
