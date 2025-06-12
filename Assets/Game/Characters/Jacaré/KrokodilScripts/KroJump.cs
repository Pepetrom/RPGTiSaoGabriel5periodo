using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroJump : IKrokodil
{
    KrokodilFSM controller;
    Vector3 pos;
    public KroJump(KrokodilFSM controller) {  this.controller = controller; }

    public void OnEnter()
    {
        controller.animator.SetTrigger("jump");
        pos = controller.transform.position;
        controller.damage = 10;
        controller.action2 = false;
        controller.jumpRate += 20;
    }

    public void OnExit()
    {
        controller.action = false;
        controller.action2 = false;
        controller.action3 = false; 
        controller.antecipation = false;
        controller.eventS = false;
        controller.end = false;
    }

    public void OnUpdate()
    {
        if (controller.action)
        {
            controller.agent.enabled = false;
            controller.Impulse(controller.jumpForce);
        }
        if (controller.action2)
        {
            controller.CombatCamera(120, 0.6f, 1.4f);
        }
        if (controller.antecipation)
        {
            controller.RotateTowardsPlayer(12);
            controller.CombatCamera(120, 0.6f, 4f);
        }
        if (controller.eventS)
        {
            controller.CombatCamera(60, 0.6f, 2f);
            controller.Impulse(-controller.jumpForce);
            if(controller.transform.position.y <= pos.y)
            {
                controller.CombatCamera(60, 0.6f, 1f);
                controller.transform.position = new Vector3(pos.x,pos.y, pos.z);
                controller.agent.enabled = true;
                controller.eventS = false;
            }
        }
        if (controller.action3)
        {
            controller.Shoot();
        }
        if (controller.end)
        {
            controller.CombatCamera(60, 0.6f, 4f);
            controller.SetState(new KroAttController(controller));
        }
    }
}
