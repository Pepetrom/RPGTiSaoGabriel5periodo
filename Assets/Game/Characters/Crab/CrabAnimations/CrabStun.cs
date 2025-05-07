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
        controller.animator.SetBool("att1", false);
        controller.animator.SetBool("att2", false);
        controller.animator.SetBool("att3", false);
        controller.animator.SetBool("att1att2", false);
        controller.animator.SetBool("att2att3", false);
        controller.animator.SetBool("attFurnace", false);
        controller.animator.SetBool("isJumping", false);
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
