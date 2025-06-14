using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroStartState : IKrokodil
{
    KrokodilFSM controller;
    public KroStartState(KrokodilFSM controller) {  this.controller = controller; }
    public void OnEnter()
    {

    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        CameraScript.instance.CombatCamera(120, 0.6f, 1.2f);
        controller.CameraShakeKro();
        if (controller.end)
        {
            CameraScript.instance.CombatCamera(60, 0.6f, 1.2f);
            controller.animator.SetBool("isStart", false);
            controller.SetState(new KroIdle(controller));
        }
    }
}
