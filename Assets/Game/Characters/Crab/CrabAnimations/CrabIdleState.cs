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
        controller.SortNumber();
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if(controller.TargetDir().magnitude <= controller.meleeRange)
        {
            controller.animator.SetBool("isEnableToAttack", true);
            controller.SetState(new CrabAttController(controller));
        }
    }
}
