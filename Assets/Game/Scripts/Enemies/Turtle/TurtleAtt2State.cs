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
        controller.animator.SetBool("Attack2", true);
        controller.rb.isKinematic = false;
    }

    public void OnExit()
    {
        controller.impulse = false;
        controller.attIdle = false;
        controller.combo = false;
        controller.antecipation = false;
        controller.hashitted = false;
    }

    public void OnUpdate()
    {
        if (controller.playerHit)
        {
            controller.SetState(new TurtleStunState(controller));
            controller.playerHit = false;
            return;
        }
        if (!controller.antecipation)
        {
            controller.RotateTowardsPlayer(8);
        }
        if (controller.active)
        {
            controller.leftHand.gameObject.SetActive(true);
            controller.rb.isKinematic = false;
            controller.agent.enabled = false;
            controller.KB(150);
        }
        else
        {
            controller.leftHand.gameObject.SetActive(false);
            controller.rb.isKinematic = true;
            controller.agent.enabled = true;
        }
        if (controller.combo)
        {
            if (controller.TargetDir().magnitude <= controller.meleeRange + 6)
            {
                controller.combed = true;
                controller.animator.SetBool("att2att3", true);
                controller.SetState(new TurtleAtt3State(controller));
            }
            else
            {
                controller.animator.SetBool("Attack2", false);
                controller.SetState(new TurtleCombatIdleState(controller));
            }
        }
        if (controller.attIdle)
        {
            controller.animator.SetBool("Attack2", false);
            controller.SetState(new TurtleCombatIdleState(controller));
        }
    }
}
