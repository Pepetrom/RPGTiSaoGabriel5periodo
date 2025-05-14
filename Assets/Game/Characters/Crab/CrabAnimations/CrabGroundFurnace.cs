using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabGroundFurnace : ICrabInterface
{
    CrabFSM controller;
    int count;
    public CrabGroundFurnace( CrabFSM controller)
    {
        this.controller = controller;   
    }

    public void OnEnter()
    {
        controller.jumpCount = 0;
        controller.damage = 40;
        count++;
    }

    public void OnExit()
    {
        controller.antecipation = false;
        controller.end = false;
        controller.hashitted = false;
        controller.minDash = 5;
        if (count >= 2)
        {
            controller.minDash += 30;
            count = 0;
        }
    }

    public void OnUpdate()
    {
        if (!controller.antecipation)
        {
            controller.RotateTowardsPlayer(3);
            CameraScript.instance.CombatCamera(70, 0.6f, 0.5f);
        }
        if (controller.activate)
        {
            controller.furnaceCollider.enabled = true;
            CameraScript.instance.CombatCamera(60, 0.6f, 2);
            CameraScript.instance.StartShake();
            controller.VFXBigConcrete.Play();
        }
        else
        {
            controller.furnaceCollider.enabled = false;
        }
        if (controller.eventS)
        {
            controller.Create(controller.crabCrack.gameObject, controller.crackPosition);
        }
        if (controller.end)
        {
            controller.animator.SetBool("attFurnace", false);
            controller.SetState(new CrabAttController(controller));
        }
    }
}
