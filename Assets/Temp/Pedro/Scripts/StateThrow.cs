using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateThrow : IBossFSM
{
    BossStateMachine controller;
    public StateThrow(BossStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.boss.SetDestination(controller.player.transform.position);
    }

    public void OnExit()
    {
        controller.animator.SetBool("Throw", false);
        //controller.isThrowing = false;
    }

    public void OnUpdate()
    {
        controller.animator.SetBool("Throw", true);
        if (controller.rock)
        {
            Debug.Log("Rock");
            controller.GrabRock();
            //controller.rockLogic.FollowSpawnPosition(controller.rockSpawnPos);
        }
        if(controller.throwRock)
        {
            Debug.Log("Tacada");
            controller.isThrowing = true;
            controller.rockLogic.ThrowRock(controller.speed,controller.rockSpawnPos,controller.rb);
        }
        controller.boss.SetDestination(controller.player.transform.position);
        controller.RotateBoss();
        if (controller.attack1toIdle)
        {
            controller.SetState(new StateIdle(controller));
        }
    }
}
