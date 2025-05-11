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
        controller.sword.enabled = false;
        controller.playerHit = false;
        controller.animator.SetBool("stun", true);
        controller.SortNumber();
    }

    public void OnExit()
    {
        controller.attIdle = false;
    }

    public void OnUpdate()
    {
        if (controller.attIdle)
        {
            if(controller.sortedNumber >= 0.6)
            {
                controller.sword.enabled = false;
                controller.animator.SetBool("stun", false);
                controller.SetState(new PorquinCombatIdleState(controller));
            }
            else
            {
                controller.sword.enabled = false;
                controller.animator.SetBool("isDashing", true);
                controller.SetState(new PorquinDashState(controller));
            }
            //controller.animator.SetBool("stun", false);
            //controller.SetState(new PorquinCombatIdleState(controller));
        }

    }
}
