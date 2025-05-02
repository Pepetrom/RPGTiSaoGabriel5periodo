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
            if(controller.randomValue < 70)
            {
                if (controller.fuzzyJump <= controller.minJump)
                {
                    controller.animator.SetBool("isFurnace", true);
                    controller.SetState(new CrabFurnaceState(controller));
                }
                else if(controller.fuzzyJump >= controller.maxJump)
                {
                    controller.animator.SetBool("isJumping", true);
                    controller.SetState(new CrabJumpState(controller));
                }
                else
                {
                    a = Random.Range(0f, 1f);
                    fuzzificado = controller.FuzzyLogic(controller.fuzzyJump, controller.minJump, controller.maxJump);
                    if(a > fuzzificado - controller.jumpCount)
                    {
                        controller.animator.SetBool("isFurnace", true);
                        controller.SetState(new CrabFurnaceState(controller));
                    }
                    else
                    {
                        controller.animator.SetBool("isJumping", true);
                        controller.SetState(new CrabJumpState(controller));
                    }
                }
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
