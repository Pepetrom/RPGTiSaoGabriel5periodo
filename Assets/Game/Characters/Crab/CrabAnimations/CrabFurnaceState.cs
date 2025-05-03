using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabFurnaceState : ICrabInterface
{
    CrabFSM controller;
    public CrabFurnaceState(CrabFSM controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.damage = 20;
        controller.jumpCount -= 20;
    }

    public void OnExit()
    {
        controller.end = false;
        controller.antecipation = false;
        controller.canDoFireDamage = false;
        controller.hashitted = false;
    }

    public void OnUpdate()
    {
        if (!controller.antecipation)
        {
            controller.RotateTowardsPlayer(8);
        }
        if (controller.end)
        {
            controller.fireCircle.SetActive(false);
            controller.animator.SetBool("isFurnace", false);
            controller.SetState(new CrabIdleState(controller));
        }
        if (controller.activate)
        {
            controller.fire.SetActive(true);
            controller.fireCircle.SetActive(true);
        }
        else
        {
            controller.fire.SetActive(false);
        }
    }
}
