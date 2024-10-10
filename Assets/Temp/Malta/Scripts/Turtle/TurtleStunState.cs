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
        controller.animator.SetBool("Stun", false);
    }

    public void OnUpdate()
    {
        if (controller.attIdle)
        {
            controller.animator.SetBool("Stun", false);
            controller.SetState(new TurtleCombatIdleState(controller));
        }
        
    }
}
