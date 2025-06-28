using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEscudoPatrol : IPorquinEscudo
{
    PorquinEscudoFSM controller;
    float time;
    float timer;
    public PEscudoPatrol(PorquinEscudoFSM controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.animator.SetBool("patrol", true);
        controller.agent.speed = 3.5f;
        controller.agent.angularSpeed = 70f;
        time = controller.patrollingCooldown;
        controller.agent.SetDestination(controller.patrolPoints[controller.currentPatrolIndex].position);
        GameManager.instance.RemoveEnemy(controller.gameObject);
    }

    public void OnExit()
    {
        controller.animator.SetBool("patrol", false);
        controller.agent.angularSpeed = 0f;
        controller.agent.speed = 0f;
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
        if (controller.TargetDir().magnitude < controller.patrolRange)
        {
            controller.animator.SetBool("patrol", false);
            controller.SetState(new PEscudoIdle(controller));
        }
    }
}
