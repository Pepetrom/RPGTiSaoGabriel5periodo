using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabStartState : ICrabInterface
{
    CrabFSM controller;
    public CrabStartState(CrabFSM controller)
    {
        UIItems.instance.FadeInFadeOut(false);
        this.controller = controller;
    }
    public void OnEnter()
    {
        GameManager.instance.AddEnemy(controller.gameObject);
        controller.ActivateTrails(false, false);
        controller.ownCollider.enabled = false;
    }

    public void OnExit()
    {
        controller.end = false;
    }

    public void OnUpdate()
    {
        CameraScript.instance.CombatCamera(150, 0.6f, 1.2f);
        if (controller.end)
        {
            controller.animator.SetBool("isJumping", true);
            controller.SetState(new CrabJumpState(controller));
        }
    }
}
