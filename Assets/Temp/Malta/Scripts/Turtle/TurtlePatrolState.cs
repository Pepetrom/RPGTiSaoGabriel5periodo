using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtlePatrolState : ITurtleStateMachine
{
    TurtleStateMachine controller;
    float time;
    float timer;
    public TurtlePatrolState(TurtleStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        time = controller.patrollingCooldown;
        controller.agent.SetDestination(controller.patrolPoints[controller.currentPatrolIndex].position);
        Debug.Log("Comecei a patrulha");
        controller.animator.SetBool("patrolling", true);
        controller.agent.angularSpeed = 70f;
    }

    public void OnExit()
    {
        Debug.Log("Parei de patrulhar");
        controller.agent.angularSpeed = 0f;
    }

    public void OnUpdate()
    {
        controller.Patrol();
        //controller.RotateTowards();
        if (controller.agent.velocity.magnitude > 0)
        {
            timer = 0;
            controller.animator.SetFloat("speed", 0.03f);
        }
        else
        {
            timer += Time.deltaTime;
            controller.animator.SetFloat("speed", 0f);
            if (timer > time)
            {
                controller.Patrolling();
                timer = 0;
            }
        }
        if (controller.TargetDir().magnitude <= controller.minCannonRange)
        {
            GameManager.instance.StartCombat();
            controller.animator.SetBool("patrolling", false);
            controller.SetState(new TurtleCombatIdleState(controller));
        }
    }
}
