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
        pos = controller.transform.position;
    }

    public void OnExit()
    {
        controller.action = false;
        controller.action2 = false;
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
            controller.CombatCamera(120, 0.6f, 4f);
        }
        if (controller.antecipation)
        {
            controller.RotateTowardsPlayer(5);
            controller.CombatCamera(120, 0.6f, 4f);
        }
        if (controller.eventS)
        {
            controller.CombatCamera(60, 0.6f, 4f);
            controller.Impulse(-controller.jumpForce);
            if(controller.transform.position.y <= pos.y)
            {
                controller.transform.position = new Vector3(pos.x,pos.y, pos.z);
                controller.agent.enabled = true;
                controller.eventS = false;
            }
        }
        if (controller.end)
        {
            controller.CombatCamera(60, 0.6f, 4f);
            controller.SetState(new KroAttController(controller));
        }
    }
}
