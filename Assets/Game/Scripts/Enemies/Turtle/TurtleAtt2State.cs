using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleAtt2State : ITurtleStateMachine
{
    TurtleStateMachine controller;
    public TurtleAtt2State(TurtleStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.damage = 25;
        controller.attRate -= 0.25f;
        controller.rb.isKinematic = false;
    }

    public void OnExit()
    {
        controller.playerHit = false;
        controller.antecipation = false;
        controller.active = false;
        controller.end = false;
        controller.combo = false;
        controller.hashitted = false;
    }

    public void OnUpdate()
    {
        if (controller.playerHit)
        {
            controller.SetState(new TurtleStunState(controller));
            return;
        }
        if (!controller.antecipation)
        {
            controller.RotateTowardsPlayer(8);
        }
        if (controller.active)
        {
            controller.leftHand.enabled = true;
            controller.rb.isKinematic = false;
            controller.agent.enabled = false;
            controller.KB(30);
        }
        else
        {
            controller.leftHand.enabled = false;
            controller.rb.isKinematic = true;
            controller.agent.enabled = true;
        }
        if (controller.combo)
        {
            if (controller.TargetDir().magnitude <= controller.meleeRange + 6)
            {
                controller.combed = true;
                controller.animator.SetTrigger("att3");
                controller.SetState(new TurtleAtt3State(controller));
            }
            else
            {
                controller.animator.SetBool("att2", false);
                controller.SetState(new TurtleCombatIdleState(controller));
            }
        }
        if (controller.end)
        {
            controller.animator.SetBool("att2", false);
            controller.SetState(new TurtleCombatIdleState(controller));
        }
    }
}
