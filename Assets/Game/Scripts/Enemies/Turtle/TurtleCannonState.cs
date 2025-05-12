using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleCannonState : ITurtleStateMachine
{
    TurtleStateMachine controller;
    //private bool hasFired = false;

    public TurtleCannonState(TurtleStateMachine controller)
    {
        this.controller = controller;
    }

    public void OnEnter()
    {
        //hasFired = false;
        controller.animator.SetBool("Cannon", true);
        controller.animator.SetBool("IsWalking", false);
        controller.agent.enabled = false;
        controller.rb.isKinematic = false;
    }

    public void OnExit()
    {
        controller.attIdle = false;
        controller.combo = false;
        controller.antecipation = false;
        controller.animator.SetBool("Cannonatt3", false);
        controller.cannonFire = false;
        controller.agent.enabled = true;
        controller.rb.isKinematic = true;
    }

    public void OnUpdate()
    {
        if (!controller.antecipation)
        {
            controller.RotateTowardsPlayer(10);
        }
        if (!controller.antecipation && controller.TargetDir().magnitude < controller.minCannonRange)
        {
            controller.animator.SetBool("Cannonatt3", true);
            controller.SetState(new TurtleAtt3State(controller));
        }
        if (controller.attIdle)
        {
            controller.SetState(new TurtleCombatIdleState(controller));
            controller.animator.SetBool("Cannon", false);
        }
    }
}
