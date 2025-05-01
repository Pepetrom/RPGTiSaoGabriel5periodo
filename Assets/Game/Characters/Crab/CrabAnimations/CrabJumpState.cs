using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabJumpState : ICrabInterface
{
    CrabFSM controller;
    public CrabJumpState(CrabFSM controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.animator.SetBool("isJumping", true);
    }

    public void OnExit()
    {
        controller.jump = false;
        controller.fall = false;
        controller.agent.enabled = true;
    }

    public void OnUpdate()
    {
        if (controller.jump)
        {
            controller.agent.enabled = false;
            controller.Impulse(controller.impulse);
        }
        if (controller.fall)
        {
            controller.agent.enabled = true;
            controller.FallTowardsPlayer(controller.impulse * 5.5f  );
        }

    }
}
