using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleAtt3State : ITurtleStateMachine
{
    TurtleStateMachine controller;
    public TurtleAtt3State(TurtleStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.damage = 30;
        controller.animator.SetBool("Attack3", true);
    }

    public void OnExit()
    {
        controller.impulse = false;
        controller.attIdle = false;
        controller.antecipation = false;
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
        if (!controller.antecipation)
        {
            controller.RotateTowardsPlayer();
        }
        if (controller.attIdle)
        {
            controller.SetState(new TurtleCombatIdleState(controller));
            controller.animator.SetBool("Attack3", false);
        }
    }
}
