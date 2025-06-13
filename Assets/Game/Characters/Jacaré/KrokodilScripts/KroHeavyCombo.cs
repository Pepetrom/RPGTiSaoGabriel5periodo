using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroHeavyCombo : IKrokodil
{
    KrokodilFSM controller;
    public KroHeavyCombo(KrokodilFSM controller) { this.controller = controller; }

    public void OnEnter()
    {
        controller.damage = 35;
    }

    public void OnExit()
    {
        controller.antecipation = false;
        controller.end = false;
        controller.combo = false;
    }

    public void OnUpdate()
    {
        if (!controller.antecipation)
            controller.RotateTowardsPlayer(12);
        if (controller.activate)
        {
            controller.damage = 35;
            controller.twoHandedCollider.enabled = true;
        }
        else
            controller.twoHandedCollider.enabled = false;
        if (controller.action)
        {
            controller.damage = 20;
            controller.twoHandedCollider.enabled = true;
        }
        else
            controller.twoHandedCollider.enabled = false;

        if (controller.end)
        {
            controller.SetState(new KroAttController(controller));
        }
    }
}
