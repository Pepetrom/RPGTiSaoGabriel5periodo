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
            if (controller.attack2Counter >= 2)
            {
                controller.attack2Counter = 0;
                controller.fullCombatCounter++;
                controller.SetState(new PorquinAtt1State(controller));
            }
            else
            {
                /*if (controller.sortedNumber <= 0.5f)
                {
                    controller.attack2Counter = 0;
                    controller.SetState(new PorquinAtt2State(controller));
                }
                else if (controller.sortedNumber > 0.5f)
                {
                    controller.animator.SetBool("isWalkingBack", true);
                    controller.SetState(new PorquinWalkBackState(controller));

                }*/
                if (controller.sortedNumber <= 0.2f)
                {
                    controller.attack2Counter = 0;
                    controller.SetState(new PorquinAtt2State(controller));
                }
                else
                {
                    controller.SetState(new PorquinWalkBackState(controller));
                }
            }
        }
    }
}
