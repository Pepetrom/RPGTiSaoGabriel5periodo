using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroSecondStage : IKrokodil
{
    KrokodilFSM controller;
    public KroSecondStage(KrokodilFSM controller) { this.controller = controller; }

    public void OnEnter()
    {
        controller.isSecondStage = true;
        controller.end = false;
        controller.animator.SetBool("isAttack", false);
    }

    public void OnExit()
    {
        controller.end = false;
    }

    public void OnUpdate()
    {
        if (controller.end)
        {
            Debug.Log("Saí do second stage");
            controller.SetState(new KroIdle(controller));
        }
    }
}
