using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleAtt2State : ITurtleStateMachine
{
    TurtleStateMachine controller;
    private bool impulseApplied = false;
    public TurtleAtt2State(TurtleStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.damage = 15;
        controller.attackSpeed = 600f;
        controller.animator.SetBool("Attack2", true);
    }

    public void OnExit()
    {
        controller.impulse = false;
        controller.attIdle = false;
        controller.combo = false;
        controller.antecipation = false;
        controller.hashitted = false;
    }

    public void OnUpdate()
    {
        if (controller.active)
        {
            controller.leftHand.gameObject.SetActive(true);
        }
        else
        {
            controller.leftHand.gameObject.SetActive(false);
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
        else if (controller.combo && controller.TargetDir().magnitude >= controller.meleeRange + 2)
        {
            controller.animator.SetBool("Attack2", false);
            controller.SetState(new TurtleCombatIdleState(controller));
        }
        if (!controller.combed)
        {
            if (controller.attIdle) 
            {
                controller.SetState(new TurtleCombatIdleState(controller));
                controller.animator.SetBool("Attack2", false);
            }
        }
        else
        {
            if (controller.combo)
            {
                Debug.Log("Opa");
                controller.animator.SetBool("att2att3", true);
                controller.SetState(new TurtleAtt3State(controller));
            }
        }

    }
}
