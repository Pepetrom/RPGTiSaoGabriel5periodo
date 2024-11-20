using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorquinStunState : IPorquinStateMachine
{
    PorquinStateMachine controller;

    public PorquinStunState(PorquinStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.playerHit = false;
        controller.animator.SetBool("stun", true);
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if (controller.attIdle)
        {
            controller.animator.SetBool("stun", false);
            controller.SetState(new PorquinCombatIdleState(controller));
        }

    }
}
