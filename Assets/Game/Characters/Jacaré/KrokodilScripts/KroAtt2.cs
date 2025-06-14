using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroAtt2 : IKrokodil
{
    KrokodilFSM controller;
    public KroAtt2(KrokodilFSM controller) { this.controller = controller; }

    public void OnEnter()
    {
        controller.SortNumber();
        controller.basicAtt += 20;
        controller.jumpRate -= 5;
        controller.damage = 30;
    }

    public void OnExit()
    {
        controller.antecipation = false;
        controller.end = false;
        controller.combo = false;
        controller.activate = false;
        controller.hashitted = false;
    }

    public void OnUpdate()
    {
        if (!controller.antecipation)
            controller.RotateTowardsPlayer(8);
        if(controller.activate)
            controller.clawCollider.enabled = true;
        else
            controller.clawCollider.enabled = false;

        if (controller.combo)
        {
            if(controller.randomValue < 50)
            {
                controller.animator.SetBool("att2comb2", true);
                controller.SetState(new KroComboGun(controller));
            }
            else
            {
                controller.animator.SetBool("att2", false);
                controller.SetState(new KroAttController(controller));
            }
        }
        if (controller.end)
        {
            controller.animator.SetBool("att2", false);
            controller.SetState(new KroAttController(controller));
        }
    }
}
