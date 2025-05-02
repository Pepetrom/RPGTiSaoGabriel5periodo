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
        controller.VFXjumpImpact.gameObject.SetActive(false);
        controller.jumpCount += 0.2f;
        controller.animator.SetBool("isJumping", true);
        controller.damage = 70;
    }

    public void OnExit()
    {
        controller.jump = false;
        controller.fall = false;
        controller.end = false;
        controller.antecipation = false;
        controller.hashitted = false;
        controller.agent.enabled = true;
        if (controller.jumpCount > 0.4f)
        {
            tradeFuzzy();
        }
    }

    public void OnUpdate()
    {
        pos = controller.transform.position;    
        if (controller.jump)
        {
            CameraScript.instance.CombatCamera(120, 0.6f,2);
            controller.agent.enabled = false;
            controller.Impulse(controller.impulse);
        }
        if (!controller.antecipation)
        {
            controller.RotateTowardsPlayer(5);
        }
        if (controller.fall)
        {
            if (controller.transform.position.y >= -2f)
            {
                CameraScript.instance.CombatCamera(60, 0.6f, 2);
                controller.Impulse(-controller.impulse);
                controller.FallTowardsPlayer(200);
            }
            else
            {
                controller.fall = false;
                pos.y = -7.5f;
                controller.agent.enabled = true;
                controller.transform.position = new Vector3(pos.x, pos.y, pos.z);
                controller.VFXjumpImpact.gameObject.SetActive(true);
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
    void tradeFuzzy()
    {
        controller.FuzzyGate(out controller.fuzzyJump, 50);
    }
}
