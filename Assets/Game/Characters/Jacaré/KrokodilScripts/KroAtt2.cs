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
            controller.RotateTowardsPlayer(8);
        if (controller.combo)
        {
            if(controller.randomValue < 100)
            {
                controller.animator.SetBool("att2comb2", true);
                controller.SetState(new KroComboGun(controller));
            }
        }
        if (controller.end)
        {
            controller.animator.SetBool("att2", false);
            controller.SetState(new KroAttController(controller));
        }
    }
}
