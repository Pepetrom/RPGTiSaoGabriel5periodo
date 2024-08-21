using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : IBossFSM
{
    BossStateMachine controller;
    public WalkState(BossStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.boss.speed = 6f;
        controller.animator.SetBool("Walk", true);
    }

    public void OnExit()
    {
        controller.animator.SetBool("Walk", false);
    }

    public void OnUpdate()
    {
        controller.boss.SetDestination(controller.player.transform.position);
        if (controller.TargetDir().magnitude > 25)
        {
            controller.SetState(new RunState(controller));
        }
        if(controller.TargetDir().magnitude < 15)
        {
            controller.SetState(new StateIdle(controller));
        }
    }
}
