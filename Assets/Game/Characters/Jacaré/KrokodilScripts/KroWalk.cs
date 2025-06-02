using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroWalk : IKrokodil
{
    KrokodilFSM controller;
    public KroWalk(KrokodilFSM controller) {  this.controller = controller; }

    public void OnEnter()
    {
        controller.agent.speed = 10f;
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
            controller.agent.speed = 0f;
            controller.animator.SetBool("isWalking", false);
            controller.SetState(new KroMoveAtt(controller));
        }
    }
}
