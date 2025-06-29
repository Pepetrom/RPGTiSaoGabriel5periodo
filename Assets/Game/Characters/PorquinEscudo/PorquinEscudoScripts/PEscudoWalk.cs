using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEscudoWalk : IPorquinEscudo
{
    PorquinEscudoFSM controller;
    public PEscudoWalk(PorquinEscudoFSM controller) { this.controller = controller; }

    public void OnEnter()
    {
        controller.animator.SetBool("isWalking", true);
        controller.agent.speed = 3f;
        controller.agent.angularSpeed = 70f;
    }

    public void OnExit()
    {
        controller.agent.speed = 0;
        controller.agent.angularSpeed = 0;
        controller.animator.SetBool("isWalking", false);
    }

    public void OnUpdate()
    {
        controller.agent.SetDestination(controller.player.transform.position);
        if (controller.TargetDir().magnitude <= controller.meleeRange)
        {
            controller.SetState(new PEscudoIdle(controller));
        }
        else if (controller.TargetDir().magnitude >= controller.runRange)
        {
            controller.SetState(new PEscudoIdle(controller));
        }
    }
}
