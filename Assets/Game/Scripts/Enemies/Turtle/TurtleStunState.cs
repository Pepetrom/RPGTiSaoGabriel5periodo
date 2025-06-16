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
        controller.animator.SetTrigger("stun");
        controller.ownCollider.enabled = false;
    }

    public void OnExit()
    {
        controller.end = false;
        controller.playerHit = false;
        controller.ownCollider.enabled = true;
    }

    public void OnUpdate()
    {
        if (controller.end)
        {
            controller.SetState(new TurtleCombatIdleState(controller));
        }
        
    }
}
