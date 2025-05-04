using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabJumpState : ICrabInterface
{
    CrabFSM controller;
    Vector3 pos;
    public CrabJumpState(CrabFSM controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.jumpCount += 20;
        controller.animator.SetBool("isJumping", true);
        controller.damage = 70;
        controller.ActivateTrails(true,true);
    }

    public void OnExit()
    {
        controller.jump = false;
        controller.fall = false;
        controller.end = false;
        controller.antecipation = false;
        controller.hashitted = false;
        controller.eventS = false;
        controller.agent.enabled = true;
        controller.ActivateTrails(false, false);
    }

    public void OnUpdate()
    {
        pos = controller.transform.position;    
        if (controller.jump)
        {
            controller.agent.enabled = false;
            controller.Impulse(controller.impulse);
        }
        if (controller.eventS)
        {
            CameraScript.instance.CombatCamera(120, 0.6f, 1.2f);
        }
        if (!controller.antecipation)
        {
            controller.RotateTowardsPlayer(5);
        }
        if (controller.fall)
        {
            controller.eventS = false;
            if (controller.transform.position.y >= -2f)
            {
                CameraScript.instance.CombatCamera(60, 0.6f, 2);
                controller.Impulse(-controller.impulse);
                controller.FallTowardsPlayer(200);
                controller.ownCollider.enabled = false;
            }
            else
            {
                controller.fall = false;
                pos.y = -7.5f;
                controller.agent.enabled = true;
                controller.transform.position = new Vector3(pos.x, pos.y, pos.z);
                controller.VFXJumpImpact.Play();
                controller.ownCollider.enabled = true;
                CameraScript.instance.StartShake();
            }
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
            controller.animator.SetBool("isJumping", false);
            controller.SetState(new CrabIdleState(controller));
        }
    }
}
