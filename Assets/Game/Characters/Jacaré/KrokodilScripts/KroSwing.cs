using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroSwing : IKrokodil
{
    KrokodilFSM controller;
    Vector3 swingPos;
    public KroSwing(KrokodilFSM controller) {  this.controller = controller; }

    public void OnEnter()
    {
        controller.SortNumber();
        controller.agent.speed = 2.3f;
        swingPos = controller.Swing();
        controller.swingRate += 30;
    }

    public void OnExit()
    {
        controller.agent.speed = 0f;
    }

    public void OnUpdate()
    {
        controller.agent.SetDestination(swingPos);
        controller.RotateTowardsPlayer(8);
        controller.SwingMove();
        if (controller.TargetDir().magnitude < controller.meleeRange)
        {
            controller.animator.SetBool("isAttack", true);
            controller.SetState(new KroAttController(controller));
        }
        else
        {
            if (controller.HasReachedDestination())
            {
                if (controller.randomValue > controller.swingRate)
                {
                    swingPos = controller.Swing();
                    controller.swingRate += 30;
                }
                else
                {
                    controller.animator.SetBool("isWalking", true);
                    controller.SetState(new KroWalk(controller));
                }
            }
        }
    }
}
