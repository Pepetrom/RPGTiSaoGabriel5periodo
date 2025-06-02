using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroHeavyAtt : IKrokodil
{
    KrokodilFSM controller;
    public KroHeavyAtt(KrokodilFSM controller) { this.controller = controller; }

    public void OnEnter()
    {
        controller.SortNumber();
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
            controller.RotateTowardsPlayer(10);
        if (!controller.isSecondStage)
        {
            if (controller.combo)
            {
                if (controller.randomValue > 40)
                {
                    controller.animator.SetBool("spin", true);
                    controller.SetState(new KroSpin(controller));
                }
            }
        }
        else
        {
            if (controller.combo)
            {
                if (controller.randomValue > 100)
                {
                    controller.animator.SetBool("spin", true);
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
            controller.animator.SetBool("heavy", false);
            controller.SetState(new KroIdle(controller));
        }
    }
}
