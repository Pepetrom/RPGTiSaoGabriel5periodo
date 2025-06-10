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
        controller.clawCollider.GetComponent<SphereCollider>().radius = 0.9f;
    }

    public void OnExit()
    {
        controller.antecipation = false;
        controller.end = false;
        controller.activate = false;
        controller.action = false;
        controller.hashitted = false;
        controller.clawCollider.GetComponent<SphereCollider>().radius = 0.5f;
    }

    public void OnUpdate()
    {
        if (!controller.antecipation)
            controller.RotateTowardsPlayer(8);
        if (controller.activate)
        {
            controller.damage = 40;
            Debug.Log("O QUE ESTÁ ACONTECENDO, PELO AMOR DE DEUS NUMENO");
            controller.twoHandedCollider.enabled = true;
        }
        else
            controller.twoHandedCollider.enabled = false;
        if (controller.action)
        {
            controller.hashitted = true;
            controller.clawCollider.enabled = true;
        }
        else
            controller.clawCollider.enabled = false;

        if (controller.end)
        {
            controller.SetState(new KroAttController(controller));
        }
    }
}
