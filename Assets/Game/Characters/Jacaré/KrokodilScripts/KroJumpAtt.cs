using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroJumpAtt : IKrokodil
{
    KrokodilFSM controller;
    public KroJumpAtt( KrokodilFSM controller) {  this.controller = controller; }
    public void OnEnter()
    {
        controller.damage = 40;
        controller.end = false;
    }

    public void OnExit()
    {
        controller.antecipation = false;
        controller.activate = false;
        controller.end = false;
        controller.action = false;
    }

    public void OnUpdate()
    {
        if (!controller.antecipation)
            controller.RotateTowardsPlayer(8);
        if (controller.activate)
            controller.twoHandedCollider.enabled = true;
        else
            controller.twoHandedCollider.enabled = false;
        if(controller.action)
            CameraScript.instance.CombatCamera(80, 0.6f, 0.8f);
        else
            CameraScript.instance.CombatCamera(60, 0.6f, 0.8f);
        if (controller.end)
            controller.SetState(new KroIdle(controller));
    }
}
