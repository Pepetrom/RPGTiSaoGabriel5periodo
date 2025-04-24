using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorquinCombatIdleState : IPorquinStateMachine
{
    PorquinStateMachine controller;
    float a;
    float fuzzificado;
    public PorquinCombatIdleState(PorquinStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.SortNumber();
        #region reset de bools
        controller.antecipation = false;
        controller.attIdle = false;
        controller.impulse = false;
        controller.combo = false;
        controller.active = false;
        controller.animator.SetBool("att1att2", false);
        controller.animator.SetBool("att2att3", false);
        controller.animator.SetBool("attack1", false);
        controller.animator.SetBool("attack2", false);
        controller.animator.SetBool("attack3", false);
        controller.animator.SetBool("stun", false);
        controller.animator.SetBool("patrolling", false);
        controller.animator.SetBool("isWalking", false);
        controller.animator.SetBool("isDashing", false);
        #endregion
        controller.agent.angularSpeed = 0f;
        controller.agent.speed = 0f;
        controller.fullCombatCounter = 0;

    }
    public void OnExit()
    {

    }
    public void OnUpdate()
    {
        if (controller.playerHit)
        {
            controller.SetState(new PorquinStunState(controller));
            controller.playerHit = false;
            return;
        }
        if (!controller.isInCombat)
        {
            controller.isInCombat = true;
            controller.animator.SetBool("patrolling", true);
            controller.SetState(new PorquinPatrolState(controller));
            return;
        }
        if(controller.TargetDir().magnitude >= controller.runRange && controller.TargetDir().magnitude < controller.patrolRange * 2)
        {
            if (controller.sortedNumber < 0.1)
            {
                controller.animator.SetBool("runAttack", true);
                controller.SetState(new PorquinRunAttackState(controller));
            }
            else
            {
                Fuzzy();
            }
        }
        else if (controller.TargetDir().magnitude > controller.patrolRange * 2)
        {
            controller.SetState(new PorquinPatrolState(controller));
        }
        else if(controller.TargetDir().magnitude > controller.meleeRange && controller.TargetDir().magnitude < controller.runRange)
        {
            Fuzzy();
        }
        else if(controller.TargetDir().magnitude < controller.meleeRange)
        {
            //Debug.Log("Ele ta no update");
            controller.SetState(new PorquinCombatControllerState(controller));
        }
    }
    private void Fuzzy()
    {
        if (controller.fuzzySwing < controller.minSwing)
        {
            controller.animator.SetBool("isWalking", true);
            controller.SetState(new PorquinWalkState(controller));
        }
        else if (controller.fuzzySwing > controller.maxSwing)
        {
            controller.animator.SetBool("isSwing", true);
            controller.SetState(new PorquinSwingState(controller));
        }
        else
        {
            a = Random.Range(0f, 1f);
            fuzzificado = controller.FuzzyLogic(controller.fuzzySwing, controller.minSwing, controller.maxSwing);
            if (a > fuzzificado)
            {
                controller.animator.SetBool("isWalking", true);
                controller.SetState(new PorquinWalkState(controller));
            }
            else
            {
                controller.animator.SetBool("isSwing", true);
                controller.SetState(new PorquinSwingState(controller));
            }
        }
    }
}
