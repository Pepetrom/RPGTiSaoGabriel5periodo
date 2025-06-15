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
        controller.selfCollider.enabled = false;
        controller.sword.enabled = false;
    }

    public void OnExit()
    {
        controller.attIdle = false;
        controller.active = false;
    }

    public void OnUpdate()
    {
        if (controller.active)
        {
            controller.agent.enabled = false;
            controller.rb.isKinematic = false;
        }
        else
        {
            controller.agent.enabled = true;
            controller.rb.isKinematic = true;
        }
        if (controller.attIdle)
        {
            controller.selfCollider.enabled = true;
            if (controller.sortedNumber >= 0.6)
            {
                controller.sword.enabled = false;
                controller.animator.SetBool("stun", false);
                controller.SetState(new PorquinCombatIdleState(controller));
            }
            else
            {
                if(controller.sortedNumber < 0.2)
                {
                    controller.sword.enabled = false;
                    controller.animator.SetBool("isDashing", true);
                    controller.SetState(new PorquinDashState(controller));
                }
                else
                {
                    controller.sword.enabled = false;
                    controller.animator.SetBool("stun", false);
                    controller.SetState(new PorquinCombatIdleState(controller));
                }
            }
        }

    }
}
