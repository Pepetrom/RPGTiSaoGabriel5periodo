using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabSpinState : ICrabInterface
{
    CrabFSM controller;
    public CrabSpinState(CrabFSM controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.SortNumber();
        controller.rb.isKinematic = true;
        controller.spinCount ++;
        controller.damage = 30;
    }

    public void OnExit()
    {
        controller.end = false;
        controller.antecipation = false;
        controller.combo = false;
        controller.hashitted = false;
        controller.jump = false;
    }

    public void OnUpdate()
    {
        if (!controller.antecipation)
        {
            controller.RotateTowardsPlayer(6);
        }
        if (controller.jump)
        {
            controller.agent.enabled = false;
            controller.rb.isKinematic = false;
            controller.KB(700);
        }
        else
        {
            controller.agent.enabled = true;
            controller.rb.isKinematic = true;
        }
        if (controller.activate)
        {
            controller.furnaceCollider.enabled = true;
        }
        else
        {
            controller.furnaceCollider.enabled = false;
        }
        if (controller.combo)
        {
            if(controller.spinCount >= 5)
            {
                controller.spinCount = 0;
                controller.spinCombo = false;
                controller.comboValue = 40;
                controller.animator.SetBool("isSpin", false);
                controller.SetState(new CrabAttController(controller));
            }
            else
            {
                if(controller.randomValue < controller.comboValue)
                {
                    controller.spinCombo = true;
                    controller.comboValue = 120;
                    controller.animator.SetBool("isSpin", false);
                    controller.SetState(new CrabAttController(controller));
                }
                else
                {
                    controller.animator.SetBool("isSpin", false);
                    controller.SetState(new CrabAttController(controller));
                }
            }
        }
        if (controller.end)
        {
            controller.animator.SetBool("isJumping", true);
            controller.SetState(new CrabJumpState(controller));
        }
    }
}
