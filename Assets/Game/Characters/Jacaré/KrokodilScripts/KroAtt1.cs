using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class KroAtt1 : IKrokodil
{
    KrokodilFSM controller;
    public KroAtt1(KrokodilFSM controller) { this.controller = controller; }

    public void OnEnter()
    {
        controller.basicAtt -= 15;
        controller.damage = 35;
    }

    public void OnExit()
    {
        controller.antecipation = false;
        controller.end = false;
        controller.activate = false;
        controller.hashitted = false;
    }

    public void OnUpdate()
    {
        if (!controller.antecipation)
            controller.RotateTowardsPlayer(8);
        if (controller.activate)
            controller.gunCollider.enabled = true;
        else
            controller.gunCollider.enabled = false;
        if (controller.end)
        {
            controller.animator.SetBool("att1", false);
            controller.SetState(new KroAttController(controller));
        }
    }
}
