using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEscudoAtt1 : IPorquinEscudo
{
    PorquinEscudoFSM controller;
    public PEscudoAtt1(PorquinEscudoFSM controller) {  this.controller = controller; }

    public void OnEnter()
    {
        controller.animator.SetTrigger("att1");
        controller.basicAtt += 10;
    }

    public void OnExit()
    {
        controller.antecipation = false;
        controller.activate = false;
        controller.end = false;
    }

    public void OnUpdate()
    {
        if (!controller.antecipation)
            controller.RotateTowardsPlayer(4);
        if(controller.activate)
            controller.shield.enabled = true;
        else
            controller.shield.enabled = false;
        if (controller.end)
            controller.SetState(new PEscudoIdle(controller));

    }
}
