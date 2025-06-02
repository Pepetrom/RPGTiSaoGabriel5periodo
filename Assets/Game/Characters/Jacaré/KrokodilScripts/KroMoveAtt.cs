using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroMoveAtt : IKrokodil
{
    KrokodilFSM controller;
    public KroMoveAtt(KrokodilFSM controller) { this.controller = controller; }

    public void OnEnter()
    {
        controller.end = false;
        controller.SortNumber();
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if (controller.randomValue < 60)
        {
            controller.animator.SetBool("kick", true);
            controller.SetState(new KroKick(controller));
        }
        else
        {
            controller.animator.SetBool("heavy", true);
            controller.SetState(new KroHeavyAtt(controller));
        }
    }
}