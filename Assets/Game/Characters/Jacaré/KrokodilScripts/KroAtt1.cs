using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroAtt1 : IKrokodil
{
    KrokodilFSM controller;
    public KroAtt1(KrokodilFSM controller) { this.controller = controller; }

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
            controller.animator.SetBool("att1", false);
            controller.SetState(new KroAttController(controller));
        }
    }
}
