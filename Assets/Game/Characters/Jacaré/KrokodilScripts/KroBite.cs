using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroBite : IKrokodil
{
    KrokodilFSM controller;
    public KroBite(KrokodilFSM controller) { this.controller = controller; }

    public void OnEnter()
    {
        controller.animator.SetTrigger("startBite");
        controller.animator.SetBool("isWalking",false);
        controller.damage = 500;
        controller.action = false;
        controller.action2 = false;
    }

    public void OnExit()
    {
        controller.action = false;
        controller.action2 = false;
        controller.antecipation = false;
        controller.end = false;
        controller.activate = false;
        controller.grabbed = false;
        controller.agent.speed = 0;
        controller.agent.acceleration = 8;
    }

    public void OnUpdate()
    {
        if (!controller.antecipation)
            controller.RotateTowardsPlayer(6);
        if (controller.action && controller.runningCo == null)
        {
            controller.runningCo = controller.StartCoroutine(controller.RunningTime());
            controller.RotateTowardsPlayer(2);
            controller.StartCoroutine(controller.RunningTime());
        }
        if (controller.action2)
        {
            controller.animator.SetBool("bite",true);
        }
        if (controller.activate)
            controller.mouthCollider.enabled = true;
        else
            controller.mouthCollider.enabled = false;
        if (controller.end)
        {
            controller.hoffMesh.SetActive(false);
            controller.animator.SetBool("bite", false);
            controller.SetState(new KroIdle(controller));
        }
    }
}
