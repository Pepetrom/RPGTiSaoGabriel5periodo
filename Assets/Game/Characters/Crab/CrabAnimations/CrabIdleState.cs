using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabIdleState : ICrabInterface
{
    CrabFSM controller;
    float a;
    float fuzzificado;
    public CrabIdleState(CrabFSM controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.SortNumber();
        controller.ActivateTrails(false, false);
        controller.animator.SetBool("cooling", false);
    }

    public void OnExit()
    {

    }
    public void OnUpdate()
    {
        if(controller.TargetDir().magnitude <= controller.meleeRange)
        {
            controller.animator.SetBool("isEnableToAttack", true);
            controller.SetState(new CrabAttController(controller));
        }
        else if(controller.TargetDir().magnitude >= controller.meleeRange && controller.TargetDir().magnitude <= controller.maxRange)
        {
            if(controller.randomValue < 60)
            {
                if(controller.randomValue < (controller.fuzzyJump - controller.jumpCount))
                {
                    Debug.Log(controller.fuzzyJump - controller.jumpCount);
                    controller.animator.SetBool("isJumping", true);
                    controller.SetState(new CrabJumpState(controller));
                }
                else
                {
                    controller.animator.SetBool("isFurnace", true);
                    controller.SetState(new CrabFurnaceState(controller));
                }
            }
            else if(controller.randomValue <= 60 && controller.randomValue >= 80)
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
            controller.animator.SetBool("isEnableToAttack", true);
            controller.SetState(new CrabAttController(controller));
        }
    }
}
