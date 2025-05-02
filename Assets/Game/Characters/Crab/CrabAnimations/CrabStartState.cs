using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabStartState : ICrabInterface
{
    CrabFSM controller;
    public CrabStartState(CrabFSM controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        GameManager.instance.AddEnemy(controller.gameObject);
    }

    public void OnExit()
    {
        controller.end = false;
    }

    public void OnUpdate()
    {
        if (controller.end)
        {
            controller.animator.SetBool("isJumping", true);
            controller.SetState(new CrabJumpState(controller));
        }
    }
}
