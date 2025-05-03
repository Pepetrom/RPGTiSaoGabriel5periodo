using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabAttController : ICrabInterface
{
    CrabFSM controller;
    float a, fuzzificado;
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
        if (controller.spinCombo)
        {
            controller.animator.SetBool("isSpin", true);
            controller.SetState(new CrabSpinState(controller));
            return;
        }
        if(controller.TargetDir().magnitude <= controller.minRange)
        {
            if(controller.fuzzyDash < controller.minDash)
            {
                controller.animator.SetBool("attFurnace", true);
                controller.SetState(new CrabGroundFurnace(controller));
            }
            else if(controller.fuzzyDash > controller.maxDash)
            {
                controller.animator.SetBool("isDashing", true);
                controller.SetState(new CrabDash(controller));
            }
            else
            {
                a = Random.Range(0, 1f);
                fuzzificado = controller.FuzzyLogic(controller.fuzzyDash, controller.minDash, controller.maxDash);
                if(a > fuzzificado)
                {
                    controller.animator.SetBool("attFurnace", true);
                    controller.SetState(new CrabGroundFurnace(controller));
                }
                else
                {
                    controller.animator.SetBool("isDashing", true);
                    controller.SetState(new CrabDash(controller));
                }
            }
        }
        else if(controller.TargetDir().magnitude <= controller.meleeRange)
        {
            if(controller.randomValue < 60)
            {
                controller.animator.SetBool("att1", true);
                controller.SetState(new CrabAtt1(controller));
            }
            else
            {
                controller.animator.SetBool("att2", true);
                controller.SetState(new CrabAtt2(controller));
            }
        }
        else if (controller.TargetDir().magnitude >= controller.maxRange)
        {
            if(controller.randomValue < 50)
            {
                controller.animator.SetBool("isSpin", true);
                controller.SetState(new CrabSpinState(controller));
            }
            else if(controller.randomValue >= 50 && controller.randomValue <= 90)
            {
                controller.animator.SetBool("attRun", true);
                controller.SetState(new CrabAttRun(controller));
            }
            else
            {
                controller.animator.SetBool("isWalking", true);
                controller.SetState(new CrabWalkFState(controller));
            }
        }
        else
        {
            controller.animator.SetBool("isEnableToAttack", false);
            controller.SetState(new CrabIdleState(controller));
        }
    }
}
