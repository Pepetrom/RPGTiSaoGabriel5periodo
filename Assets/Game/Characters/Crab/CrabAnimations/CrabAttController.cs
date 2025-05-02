using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabAttController : ICrabInterface
{
    CrabFSM controller;
    public CrabAttController(CrabFSM controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.animator.SetBool("att1", false);
        controller.animator.SetBool("att2", false);
        controller.animator.SetBool("att3", false);
        controller.animator.SetBool("att1att2", false);
        controller.animator.SetBool("att2att3", false);
        controller.animator.SetBool("attFurnace", false);
        controller.end = false;
        controller.SortNumber();
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if(controller.TargetDir().magnitude <= controller.minRange)
        {
            controller.animator.SetBool("attFurnace", true);
            controller.SetState(new CrabGroundFurnace(controller));
        }
        else if(controller.TargetDir().magnitude <= controller.meleeRange)
        {
            if(controller.randomValue < 100)
            {
                controller.animator.SetBool("att1", true);
                controller.SetState(new CrabAtt1(controller));
            }
            else if(controller.randomValue >= 40 && controller.randomValue <= 60)
            {
                // fornalha
            }
            else
            {
                // ataque 2
            }
        }
        else if (controller.TargetDir().magnitude >= controller.maxRange)
        {
            controller.animator.SetBool("isSpin", true);
            controller.SetState(new CrabSpinState(controller));
        }
        else
        {
            controller.animator.SetBool("isEnableToAttack", false);
            controller.SetState(new CrabIdleState(controller));
        }
    }
}
