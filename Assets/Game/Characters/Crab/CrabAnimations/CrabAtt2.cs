using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class CrabAtt2 : ICrabInterface
{
    CrabFSM controller;
    public CrabAtt2(CrabFSM controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.animator.SetBool("att2", true);
    }

    public void OnExit()
    {
        controller.end = false;
        controller.antecipation = false;
        controller.combo = false;
    }

    public void OnUpdate()
    {
        if (controller.antecipation)
        {
            controller.RotateTowardsPlayer();
        }
        if (controller.end)
        {
            controller.animator.SetBool("att2", false);
            controller.SetState(new CrabAttController(controller));
        }
    }
}
