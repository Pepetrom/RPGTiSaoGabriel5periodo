using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroKick : IKrokodil
{
    KrokodilFSM controller;
    public KroKick(KrokodilFSM controller) { this.controller = controller; }

    public void OnEnter()
    {
        controller.animator.SetBool("att2", false);
        controller.damage = 30;
        controller.moveAtt += 20;
    }

    public void OnExit()
    {
        controller.antecipation = false;
        controller.end = false;
    }

    public void OnUpdate()
    {
        if (!controller.antecipation)
            controller.RotateTowardsPlayer(10);
        if (controller.activate)
            controller.footCollider.enabled = true;
        else
            controller.footCollider.enabled = false;
        if (controller.end)
        {
            controller.SetState(new KroIdle(controller));
        }
    }
}
