using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroAtt2 : IKrokodil
{
    KrokodilFSM controller;
    public KroAtt2(KrokodilFSM controller) { this.controller = controller; }

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
            controller.animator.SetBool("att2", false);
            controller.SetState(new KroAttController(controller));
        }
    }
}
