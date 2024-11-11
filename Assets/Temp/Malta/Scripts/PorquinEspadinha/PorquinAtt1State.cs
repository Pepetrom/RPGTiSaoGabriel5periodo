using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorquinAtt1State : IPorquinStateMachine
{
    PorquinStateMachine controller;
    private bool impulseApplied;
    public PorquinAtt1State(PorquinStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.animator.SetBool("attack1", true);
        impulseApplied = false;
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if (!controller.antecipation)
            controller.RotateTowardsPlayer();
        /*if (controller.impulse && !impulseApplied)
        {
            impulseApplied = true;
            controller.AttacksKB(controller.kbForce);
        }*/
        if (controller.attIdle)
        {
            controller.animator.SetBool("attack1", false);
            controller.SetState(new PorquinCombatIdleState(controller));
        }

    }
}
