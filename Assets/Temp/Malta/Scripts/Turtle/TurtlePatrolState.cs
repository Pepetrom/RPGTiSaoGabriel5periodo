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
        controller.RotateTowards();
        controller.Patrol();
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
                controller.Patrolling(controller.patrolCenter);
                Debug.Log("Tempo funfou tb");
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
