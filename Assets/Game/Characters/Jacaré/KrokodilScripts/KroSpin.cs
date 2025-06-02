using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroSpin : IKrokodil
{
    KrokodilFSM controller;
    public KroSpin(KrokodilFSM controller) { this.controller = controller; }

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
            controller.RotateTowardsPlayer(8);
        if (controller.end)
        {
            controller.animator.SetBool("spin", false);
            controller.SetState(new KroAttController(controller));
        }
    }
}
