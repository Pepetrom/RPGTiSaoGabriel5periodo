using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabIdleState : ICrabInterface
{
    CrabFSM controller;
    public CrabIdleState(CrabFSM controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {

    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if(controller.TargetDir().magnitude < 20 && controller.TargetDir().magnitude > 7)
        {
            controller.SetState(new CrabWalkFState(controller));
        }
        else if(controller.TargetDir().magnitude <= 7)
        {

        }
    }
}
