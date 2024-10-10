using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtlePatrolState : ITurtleStateMachine
{
    TurtleStateMachine controller;
    float time;
    public TurtlePatrolState(TurtleStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        time = Time.time + controller.patrollingCooldown;
        controller.Patrolling(controller.patrolCenter);
        Debug.Log("Comecei a patrulha");
    }

    public void OnExit()
    {
        Debug.Log("Parei de patrulhar");
        controller.agent.angularSpeed = 0f;
    }

    public void OnUpdate()
    {
        if(controller.rb.velocity.magnitude >= 0)
        {
            controller.animator.SetFloat("speed", 0.03f);
        }
        else
        {
            controller.animator.SetFloat("speed", 0f);
        }
        if (Time.time > time)
        {
            controller.Patrolling(controller.patrolCenter);
            time = Time.time + controller.patrollingCooldown;
        }
        if (controller.TargetDir().magnitude <= controller.minCannonRange)
        {
            controller.animator.SetBool("patrolling", false);
            controller.SetState(new TurtleCombatIdleState(controller));
        }
    }
}
