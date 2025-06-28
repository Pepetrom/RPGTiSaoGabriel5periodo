using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
public class TurtleCombatIdleState : ITurtleStateMachine
{
    TurtleStateMachine controller;
    float a, fuzzificado;
    public TurtleCombatIdleState(TurtleStateMachine controller)
    {
        this.controller = controller;
    }

    public void OnEnter()
    {
        controller.animator.SetBool("isRunning", false);
        controller.animator.SetBool("att1", false);
        controller.animator.SetBool("att2", false);
        controller.animator.SetBool("isAttack", false);
        controller.SortNumber();
        controller.rightHand.enabled = false;
        controller.leftHand.enabled = false;
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if (!controller.isInCombat)
        {
            controller.isInCombat = true;
            controller.SetState(new TurtlePatrolState(controller));
            return;
        }
        if (controller.playerHit)
        {
            controller.SetState(new TurtleStunState(controller));
            controller.playerHit = false;
        }
        if (controller.TargetDir().magnitude > controller.patrolDistance)
        {
            controller.SetState(new TurtlePatrolState(controller));
        }
        if (controller.TargetDir().magnitude <= controller.meleeRange)
        {
            controller.animator.SetBool("isAttack", true);
            controller.SetState(new TurtleAttController(controller));
        }
        else if(controller.TargetDir().magnitude < controller.minCannonRange)
        {
            controller.animator.SetBool("isWalking", true);
            controller.SetState(new TurtleWalkState(controller));
        }
        else if (controller.TargetDir().magnitude >= controller.minCannonRange && controller.TargetDir().magnitude < controller.patrolDistance)
        {
            if (controller.fuzzyCannon < controller.maxCannonRange)
            {
                controller.SetState(new TurtleCannonState(controller));
            }
            else if (controller.fuzzyCannon > controller.maxCannonRange + 4)
            {
                controller.animator.SetBool("isRunning", true);
                controller.SetState(new TurtleRunState(controller));
            }
            else
            {
                a = Random.Range(0.0f, 1.0f);
                fuzzificado = controller.FuzzyLogic(controller.fuzzyCannon, 16, 36);
                if (a > fuzzificado)
                {
                    controller.SetState(new TurtleCannonState(controller));
                }
                else
                {
                    controller.animator.SetBool("isRunning", true);
                    controller.SetState(new TurtleRunState(controller));
                }
            }
        }
    }
}
