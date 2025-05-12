using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleStunState : ITurtleStateMachine
{
    TurtleStateMachine controller;

    public TurtleStunState(TurtleStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.animator.SetBool("Stun", true);
    }

    public void OnExit()
    {
        controller.active = false;
        controller.attIdle = false;
        controller.animator.SetBool("Stun", false);
    }

    public void OnUpdate()
    {
        if (controller.active)
        {
            controller.agent.enabled = false;
            controller.rb.isKinematic = false;
            controller.KB(-300);
            Debug.Log("sTUN");
        }
        else
        {
            controller.agent.enabled = true;
            controller.rb.isKinematic = true;
        }
        if (controller.attIdle)
        {
            controller.animator.SetBool("Stun", false);
            controller.SetState(new TurtleCombatIdleState(controller));
        }
        
    }
}
