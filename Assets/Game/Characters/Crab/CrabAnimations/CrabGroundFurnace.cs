using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabGroundFurnace : ICrabInterface
{
    CrabFSM controller;
    public CrabGroundFurnace( CrabFSM controller)
    {
        this.controller = controller;   
    }

    public void OnEnter()
    {
        controller.jumpCount = 0;
        controller.damage = 40;
    }

    public void OnExit()
    {
        controller.antecipation = false;
        controller.end = false;
        controller.hashitted = false;
    }

    public void OnUpdate()
    {
        if (!controller.antecipation)
        {
            controller.RotateTowardsPlayer(3);
        }
        if (controller.activate)
        {
            controller.jumpCollider.enabled = true;
        }
        else
        {
            controller.jumpCollider.enabled = false;
        }
        if (controller.end)
        {
            controller.animator.SetBool("attFurnace", false);
            controller.SetState(new CrabAttController(controller));
        }
    }
}
