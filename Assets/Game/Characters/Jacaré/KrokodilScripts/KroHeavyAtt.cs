using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroHeavyAtt : IKrokodil
{
    KrokodilFSM controller;
    public KroHeavyAtt(KrokodilFSM controller) { this.controller = controller; }

    public void OnEnter()
    {
        controller.damage = 40;
        controller.SortNumber();
        controller.moveAtt -= 20;
    }

    public void OnExit()
    {
        controller.antecipation = false;
        controller.end = false;
        controller.combo = false;
        controller.activate = false;
    }

    public void OnUpdate()
    {
        if (!controller.antecipation)
        {
            controller.RotateTowardsPlayer(10);
            CameraScript.instance.CombatCamera(90, 0.6f, 0.8f);
        }
        if (controller.activate)
        {
            controller.twoHandedCollider.enabled = true;
            CameraScript.instance.CombatCamera(60, 0.6f, 1.8f);
        }
        else
            controller.twoHandedCollider.enabled = false;

        if (!controller.isSecondStage)
        {
            if (controller.combo)
            {
                if (controller.randomValue > 40)
                {
                    controller.animator.SetTrigger("spin");
                    controller.SetState(new KroSpin(controller));
                }
            }
        }
        else
        {
            if (controller.combo)
            {
                if (controller.randomValue < 200)
                {
                    controller.animator.SetTrigger("spin");
                    controller.SetState(new KroSpin(controller));
                }
                else
                {
                    controller.animator.SetBool("heavyCombo", true);
                    controller.SetState(new KroHeavyCombo(controller));
                }
            }
        }
        if (controller.end)
        {
            controller.SetState(new KroAttController(controller));
        }
    }
}
