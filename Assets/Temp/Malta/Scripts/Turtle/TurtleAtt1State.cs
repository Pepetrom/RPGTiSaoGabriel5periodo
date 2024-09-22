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
        controller.attackSpeed = 400f;
        controller.animator.SetBool("Attack1", true);
        controller.SortNumber();
        impulseApplied = false;
        controller.damage = 20;
    }

    public void OnExit()
    {
        controller.impulse = false;
        controller.antecipation = false;
        controller.attIdle = false;
        controller.combo = false;
        controller.animator.SetBool("att1att2", true);
        controller.hashitted = false;
    }

    public void OnUpdate()
    {
        if (controller.active)
        {
            controller.rightHand.gameObject.SetActive(true);
        }
        else
        {
            controller.rightHand.gameObject.SetActive(false);
        }
        if (controller.impulse && !impulseApplied)
        {
            //controller.Impulse();
            impulseApplied = true;
        }
        if (!controller.antecipation)
        {
            controller.RotateTowardsPlayer();
        }

        if (controller.sortedNumber < 0.8f)
        {
            if (controller.combo && controller.TargetDir().magnitude <= controller.meleeRange)
            {
                controller.combed = true;
                controller.SetState(new TurtleAtt2State(controller));
            }
            else if(controller.combo && controller.TargetDir().magnitude >= controller.meleeRange)
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
