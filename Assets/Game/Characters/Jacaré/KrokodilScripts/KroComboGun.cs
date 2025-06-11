using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroComboGun : IKrokodil
{
    KrokodilFSM controller;
    public KroComboGun(KrokodilFSM controller) { this.controller = controller; }

    public void OnEnter()
    {
        controller.damage = 30;
    }

    public void OnExit()
    {
        controller.antecipation = false;
        controller.end = false;
        controller.combo = false;
        controller.activate = false;
        controller.action = false;
        controller.hashitted = false;
        Debug.Log("sai");
    }

    public void OnUpdate()
    {
        if (!controller.antecipation)
            controller.RotateTowardsPlayer(10);
        if (controller.activate)
        {
            controller.clawCollider.enabled = true;
        }
        else
            controller.clawCollider.enabled = false;
        if (controller.action)
        {
            controller.gunCollider.enabled = true;
        }
        else
            controller.gunCollider.enabled = false;

        if(controller.TargetDir().magnitude < controller.meleeRange + 5)
        {
            if (controller.combo)
            {
                controller.animator.SetTrigger("spin");
                controller.SetState(new KroSpin(controller));
            }
        }
        if (controller.end)
        {
            controller.animator.SetBool("att2comb2", false);
            controller.SetState(new KroAttController(controller));
        }
    }
}
