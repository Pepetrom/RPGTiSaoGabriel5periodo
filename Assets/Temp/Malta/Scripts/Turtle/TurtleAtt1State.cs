using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TurtleAtt1State : ITurtleStateMachine
{
    TurtleStateMachine controller;
    private bool impulseApplied = false;

    public TurtleAtt1State(TurtleStateMachine controller)
    {
        this.controller = controller;
    }

    public void OnEnter()
    {
        impulseApplied = false;
        controller.attackSpeed = 1000f;
        controller.animator.SetBool("Attack1", true);
        controller.SortNumber();
        impulseApplied = false; 
    }

    public void OnExit()
    {
        controller.impulse = false;
        controller.antecipation = false;
        controller.attIdle = false;
        controller.combo = false;
        controller.animator.SetBool("att1att2", true);
    }

    public void OnUpdate()
    {
        if (controller.impulse && !impulseApplied)
        {
            controller.Impulse();
            impulseApplied = true;
        }
        if (!controller.antecipation)
        {
            controller.RotateTowardsPlayer();
        }

        if (controller.sortedNumber < 0.8f)
        {
            if (controller.combo && controller.TargetDir().magnitude <= controller.minCannonRange)
            {
                controller.combed = true;
                controller.SetState(new TurtleAtt2State(controller));
            }
            else if(controller.attIdle && controller.TargetDir().magnitude >= controller.minCannonRange)
            {
                controller.animator.SetBool("Attack1", false);
                controller.SetState(new TurtleCombatIdleState(controller));
            }
        }
        else
        {
            if (controller.attIdle)
            {
                controller.animator.SetBool("Attack1", false);
                controller.SetState(new TurtleCombatIdleState(controller));
            }
        }
    }
}
