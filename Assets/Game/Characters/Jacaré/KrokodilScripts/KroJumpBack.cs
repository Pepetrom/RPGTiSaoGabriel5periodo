using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroJumpBack : IKrokodil
{
    KrokodilFSM controller;
    Vector3 pos;

    public KroJumpBack(KrokodilFSM controller) {  this.controller = controller; }

    public void OnEnter()
    {
        controller.RotateTowardsPlayer(100);
        pos = controller.jumpLocation.transform.position;
        controller.agent.speed = 0;
        controller.agent.acceleration = 8;
        controller.agent.angularSpeed = 0;
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
        if(controller.action)
        {
            controller.Impulse(controller.jumpForce);
        }
        if (controller.action2)
        {
            controller.CombatCamera(100, 0.6f, 2f);
        }
        if (controller.eventS)
        {
            controller.CombatCamera(60, 0.6f, 2f);
            controller.Impulse(-controller.jumpForce);
            controller.FallTowardsSomething(300, controller.jumpLocation.transform);
            if (controller.transform.position.y <= pos.y)
            {
                controller.CombatCamera(60, 0.6f, 1f);
                controller.transform.position = new Vector3(pos.x, pos.y, pos.z);
                controller.agent.enabled = true;
                controller.eventS = false;
            }
        }
        if (controller.end)
        {
            controller.SetState(new KroIdle(controller));
        }
    }
}
