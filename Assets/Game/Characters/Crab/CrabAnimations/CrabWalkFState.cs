using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabWalkFState : ICrabInterface
{
    CrabFSM controller;
    public CrabWalkFState(CrabFSM controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.agent.speed = 6;
        controller.animator.SetBool("isWalking", true);
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        controller.agent.SetDestination(controller.player.transform.position);
        controller.RotateTowardsPlayer(6);

        if(controller.TargetDir().magnitude <= controller.meleeRange)
        {
            controller.agent.speed = 0;
            controller.animator.SetBool("isWalking", false);
            controller.SetState(new CrabIdleState(controller));
        }
    }
}
