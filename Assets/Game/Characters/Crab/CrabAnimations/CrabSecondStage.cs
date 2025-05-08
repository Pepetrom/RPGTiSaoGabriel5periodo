using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabSecondStage : ICrabInterface
{
    CrabFSM controller;
    Vector3 pos;
    public CrabSecondStage(CrabFSM controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        //controller.animator.SetBool("secondStage", true);
        controller.ActivateTrails(true, true);
        controller.secondStage = true;
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
            controller.Impulse(controller.impulse * 20);
        }
        if (controller.eventS)
        {
            CameraScript.instance.CombatCamera(120, 0.6f, 1.2f);
        }
        if (controller.fall)
        {
            controller.eventS = false;
            if (controller.transform.position.y >= 8)
            {
                controller.Impulse(-controller.impulse);
                controller.FallTowardsSomething(200,controller.secondStageLocation.transform);
                controller.ownCollider.enabled = false;
            }
            else
            {
                controller.fall = false;
                pos.y = 8f;
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
            controller.animator.SetBool("secondStage", false);
            controller.SetState(new CrabBigFire(controller));
        }
    }
}
