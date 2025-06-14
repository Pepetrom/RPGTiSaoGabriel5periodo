using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroSpin : IKrokodil
{
    KrokodilFSM controller;
    public KroSpin(KrokodilFSM controller) { this.controller = controller; }

    public void OnEnter()
    {
        controller.damage = 35;
        controller.activate = false;
        controller.clawCollider.GetComponent<SphereCollider>().radius = 0.09f;
        controller.twoHandedCollider.GetComponent<SphereCollider>().radius = 0.12f;
    }

    public void OnExit()
    {
        controller.antecipation = false;
        controller.end = false;
        controller.activate = false;
        controller.action = false;
        controller.action2 = false;
        controller.hashitted = false;
        controller.clawCollider.GetComponent<SphereCollider>().radius = 0.05f;
        controller.twoHandedCollider.GetComponent<SphereCollider>().radius = 0.09f;
    }

    public void OnUpdate()
    {
        if (!controller.antecipation)
        {
            controller.RotateTowardsPlayer(8);
        }
        if (!controller.action2)
        {
            CameraScript.instance.CombatCamera(70, 0.6f, 0.6f);
        }
        if (controller.activate)
        {
            controller.twoHandedCollider.enabled = true;
        }
        else
            controller.twoHandedCollider.enabled = false;
        if (controller.action)
        {
            CameraScript.instance.CombatCamera(60, 0.6f, 1.8f);
            controller.hashitted = true;
            controller.clawCollider.enabled = true;
        }
        else
            controller.clawCollider.enabled = false;

        if (controller.end)
        {
            controller.animator.SetBool("isAttack", true);
            controller.SetState(new KroAttController(controller));
        }
    }
}
