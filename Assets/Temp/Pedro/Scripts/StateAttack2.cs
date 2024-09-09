using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class StateAttack2 : IBossFSM
{
    BossStateMachine controller;
    public StateAttack2( BossStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.damage = 20;
        controller.animator.SetBool("Attack2", true);
    }

    public void OnExit()
    {
        controller.rightHand.enabled = false;
        controller.animationIsEnded = false;
        controller.animator.SetBool("Att3Att2", false);
        controller.animator.SetBool("Attack2", false);
        controller.animationIsEnded = false;
        controller.attack1toIdle = false;
        controller.att3att2 = false;
    }

    public void OnUpdate()
    {
        if(controller.attHit) 
        {
            controller.rightHand.enabled = true;
        }
        if(controller.attack1toIdle)
        {
            controller.SetState(new StateIdle(controller));
        }
    }
}
