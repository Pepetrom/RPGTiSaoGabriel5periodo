using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEscudoAtt2 : IPorquinEscudo
{
    PorquinEscudoFSM controller;
    public PEscudoAtt2(PorquinEscudoFSM controller) { this.controller = controller; }

    public void OnEnter()
    {
        controller.animator.SetTrigger("att2");
        controller.basicAtt += 15;
        controller.damage = 25;
        controller.isShieldIsActive = false;
    }

    public void OnExit()
    {
        controller.antecipation = false;
        controller.activate = false;
        controller.end = false;
        controller.isShieldIsActive = true;
        controller.hashitted = false;
    }

    public void OnUpdate()
    {
        if (!controller.antecipation)
            controller.RotateTowardsPlayer(4);
        if (controller.activate)
            controller.shield.enabled = true;
        else
            controller.shield.enabled = false;
        if (controller.end)
            controller.SetState(new PEscudoIdle(controller));

    }
}
