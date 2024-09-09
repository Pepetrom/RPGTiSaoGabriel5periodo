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
        controller.damage = 5;
        controller.animator.SetBool("Attack3", true);
        controller.SortNumber();
    }

    public void OnExit()
    {
        controller.rightHand.enabled = false;
        if (!controller.att3att2)
        {
            controller.animator.SetBool("Attack3", false);
        }
        else
        {
            controller.animator.SetBool("Att3Att2", true);
        }
        controller.animator.SetBool("Att1Att3", false);
        controller.animationIsEnded = false;
        controller.attack1toIdle = false;
        controller.att1att3 = false;
    }

    public void OnUpdate()
    {
        if (controller.attHit)
        {
            controller.rightHand.enabled = true;
        }
        if(controller.sortedNumber >= 0.5f)
        {
            if(controller.animationIsEnded)
            {
                Debug.Log("Combo2");
                controller.Att3Att2();
                controller.SetState(new StateAttack2(controller));
            }
        }
        else
        {
            if (controller.attack1toIdle)
            {
                Debug.Log("Não combou2");
                controller.SetState(new StateIdle(controller));
            }
        }
    }
}
