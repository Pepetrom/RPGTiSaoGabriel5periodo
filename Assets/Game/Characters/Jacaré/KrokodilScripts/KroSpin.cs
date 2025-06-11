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
        controller.twoHandedCollider.GetComponent<SphereCollider>().radius = 0.18f;
    }

    public void OnExit()
    {
        controller.antecipation = false;
        controller.end = false;
        controller.activate = false;
        controller.action = false;
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
            CameraScript.instance.CombatCamera(90, 0.6f, 0.6f);
        }
        if (controller.activate)
        {
            controller.damage = 40;
            controller.twoHandedCollider.enabled = true;
            Debug.Log("Ativei o giro");
        }
        else
            controller.twoHandedCollider.enabled = false;
        if (controller.action)
        {
            CameraScript.instance.CombatCamera(60, 0.6f, 1.8f);
            controller.hashitted = true;
            controller.clawCollider.enabled = true;
            Debug.Log("Ativei a garra");
        }
        else
            controller.clawCollider.enabled = false;

        if (controller.end)
        {
            controller.SetState(new KroAttController(controller));
        }
    }
}
