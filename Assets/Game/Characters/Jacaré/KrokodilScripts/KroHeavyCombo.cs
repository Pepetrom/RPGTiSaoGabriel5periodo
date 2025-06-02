using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroHeavyCombo : IKrokodil
{
    KrokodilFSM controller;
    public KroHeavyCombo(KrokodilFSM controller) { this.controller = controller; }

    public void OnEnter()
    {

    }

    public void OnExit()
    {
        controller.antecipation = false;
        controller.end = false;
        controller.combo = false;
    }

    public void OnUpdate()
    {
        if (!controller.antecipation)
            controller.RotateTowardsPlayer(10);
        if (controller.end)
        {
            controller.animator.SetBool("att2comb2", false);
            controller.SetState(new KroAttController(controller));
        }
    }
}
