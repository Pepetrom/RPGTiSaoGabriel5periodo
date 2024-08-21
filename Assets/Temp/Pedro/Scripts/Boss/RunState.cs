using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : IBossFSM
{
    BossStateMachine controller;
    public RunState(BossStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.boss.speed = 15f;
        controller.animator.SetBool("Run", true);
    }

    public void OnExit()
    {
        controller.animator.SetBool("Run", false);
    }

    public void OnUpdate()
    {
        controller.boss.SetDestination(controller.player.transform.position);
        if (controller.TargetDir().magnitude <= 25)
        {
            controller.SetState(new WalkState(controller));
        }
    }
}
