using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroKick : IKrokodil
{
    KrokodilFSM controller;
    public KroKick(KrokodilFSM controller) { this.controller = controller; }

    public void OnEnter()
    {

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
        if (controller.end)
        {
            controller.animator.SetBool("kick", false);
            controller.SetState(new KroAttController(controller));
        }
    }
}
