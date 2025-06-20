using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroSecondStage : IKrokodil
{
    KrokodilFSM controller;
    float nextFireTime;
    public KroSecondStage(KrokodilFSM controller) { this.controller = controller; }

    public void OnEnter()
    {
        controller.isSecondStage = true;
        controller.end = false;
        controller.animator.SetBool("isAttack", false);
    }

    public void OnExit()
    {
        controller.end = false;
        controller.action = false;
        controller.action2 = false;
    }

    public void OnUpdate()
    {
        if (controller.action)
        {
            controller.agent.speed = 25;
            controller.agent.acceleration = 16;
            controller.agent.angularSpeed = 100;
            controller.agent.SetDestination(controller.jumpLocation.transform.position);
            if (controller.HasReachedDestination())
            {
                controller.animator.SetTrigger("waterJump");
                controller.agent.enabled = false;
            }
        }
        if (controller.end)
        {
            controller.armor.SetActive(false);
            controller.croc.SetActive(false);
            while(controller.bombCount < 60)
            {
                if (Time.time >= nextFireTime)
                {
                    controller.DropBombs();
                    nextFireTime = Time.time + controller.bombFireRate;
                }
            }
        }
        if(controller.bombCount >= 60)
        {
            controller.armor.SetActive(true);
            controller.croc.SetActive(true);
            controller.animator.SetTrigger("backToArena");
        }
    }

}
