using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorquinPatrolState : IPorquinStateMachine
{
    PorquinStateMachine controller;
    float time;
    float timer;
    public PorquinPatrolState(PorquinStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.agent.speed = 3.5f;
        time = controller.patrollingCooldown;
        controller.agent.SetDestination(controller.patrolPoints[controller.currentPatrolIndex].position);
        controller.animator.SetBool("patrolling", true);
        controller.agent.angularSpeed = 70f;
        GameManager.instance.RemoveEnemy(controller.gameObject);
    }

    public void OnExit()
    {
        controller.agent.angularSpeed = 0f;
        controller.animator.SetBool("patrolling", false);
        GameManager.instance.AddEnemy(controller.gameObject);
    }

    public void OnUpdate()
    {
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
                controller.Patrolling();
                timer = 0;
            }
        }
        if(controller.TargetDir().magnitude < controller.patrolRange)
        {
            controller.animator.SetBool("patrolling", false);
            controller.SetState(new PorquinCombatIdleState(controller));
        }
    }
}
