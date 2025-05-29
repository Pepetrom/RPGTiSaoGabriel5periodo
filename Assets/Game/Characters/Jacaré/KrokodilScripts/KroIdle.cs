using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroIdle : IKrokodil
{
    KrokodilFSM controller;
    public KroIdle(KrokodilFSM controller) { this.controller = controller; }
    public void OnEnter()
    {
        controller.ownCollider.enabled = false;
        controller.agent.speed = 5f;
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if(controller.TargetDir().magnitude > 10)
        {
            controller.animator.SetBool("isWalking",true);

        }
        controller.agent.SetDestination(controller.player.transform.position);
        controller.RotateTowardsPlayer(6);
    }
}
