using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorquinDashState : IPorquinStateMachine
{
    PorquinStateMachine controller;
    public PorquinDashState(PorquinStateMachine controller)
    {
        this.controller = controller;
    }

    public void OnEnter()
    {
        controller.animator.SetBool("stun", false);
        Debug.Log("Entrei no dash");
        controller.isDashing = true;
        controller.rb.isKinematic = true;
        controller.playerHit = false;
        controller.selfCollider.enabled = false;
        controller.sword.enabled = false;
        controller.active = false;
    }

    public void OnExit()
    {
        Debug.Log("Saí do dash");
        controller.selfCollider.enabled = true;
        controller.isDashing = false;
    }

    public void OnUpdate()
    {
        if (controller.active)
        {
            Debug.Log("ativo");
            controller.agent.enabled = false;
            controller.rb.isKinematic = false;
            controller.KB(-20);
        }
        else
        {
            controller.agent.enabled = true;
            controller.rb.isKinematic = true;
            Debug.Log("inativo");
        }
        if (controller.attIdle)
        {
            Debug.Log("Era pra sair daqui");
            controller.animator.SetBool("isDashing", false);
            controller.SetState(new PorquinCombatIdleState(controller));
        }
    }
}
