using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttack3 : IBossFSM
{
    BossStateMachine controller;
    float timer;
    public StateAttack3(BossStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        timer = Time.time + 1f;
    }

    public void OnExit()
    {
        controller.animator.SetBool("Attack3", false);
        controller.animationIsEnded = false;
    }

    public void OnUpdate()
    {
        controller.animator.SetBool("Attack3", true);
        if(controller.animationIsEnded)
        {
            controller.SetState(new StateIdle(controller));
        }
    }
}
