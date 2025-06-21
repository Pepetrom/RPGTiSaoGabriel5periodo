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
            CameraScript.instance.CombatCamera(90, 0.6f, 1.2f);
            controller.agent.speed = 25;
            controller.agent.acceleration = 20;
            controller.agent.angularSpeed = 100;
            controller.agent.SetDestination(controller.jumpLocation.transform.position);
            if (controller.HasReachedDestination())
            {
                controller.animator.SetTrigger("waterJump");
                controller.agent.enabled = false;
                controller.action = false;
            }
        }
        if (controller.end)
        {
            controller.armor.SetActive(false);
            controller.croc.SetActive(false);
            if(controller.bombCount <= 60)
            {
                CameraScript.instance.CombatCamera(90, 0.6f, 1.2f);
                controller.CameraShakeKro();
                if (Time.time >= nextFireTime)
                {
                    controller.DropBombs();
                    nextFireTime = Time.time + controller.bombFireRate;
                    controller.bombCount += 6;
                }
            }
        }
        if(controller.bombCount >= 60)
        {
            controller.armor.SetActive(true);
            controller.croc.SetActive(true);
            controller.animator.SetTrigger("backToArena");
            controller.SetState(new KroJumpBack(controller));
        }
    }

}
