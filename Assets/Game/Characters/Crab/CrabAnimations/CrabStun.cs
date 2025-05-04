using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabStun : ICrabInterface
{
    CrabFSM controller;
    public CrabStun(CrabFSM controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.posture = controller.maxPosture;
    }

    public void OnExit()
    {
        controller.end = false;
    }

    public void OnUpdate()
    {
        if (controller.end)
        {
            controller.animator.SetBool("isStunned", false);
            controller.SetState(new CrabIdleState(controller));
        }
    }
}
