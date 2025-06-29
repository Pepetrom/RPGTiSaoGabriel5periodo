using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEscudoRunAtt : IPorquinEscudo
{
    PorquinEscudoFSM controller;
    public PEscudoRunAtt(PorquinEscudoFSM controller) { this.controller = controller; }

    public void OnEnter()
    {
        controller.animator.SetBool("runAtt",true);
        controller.damage = 30;
        controller.agent.speed = 8;
        controller.agent.angularSpeed = 80;
        controller.isShieldIsActive = false;
        controller.end = false;
    }

    public void OnExit()
    {
        controller.animator.SetBool("runAtt", false);
        controller.antecipation = false;
        controller.activate = false;
        controller.end = false;
        controller.action = false;
        controller.isShieldIsActive = true;
        controller.hashitted = false;
    }

    public void OnUpdate()
    {
        if (!controller.antecipation)
            controller.agent.SetDestination(controller.player.transform.position);
        else
        {
            controller.agent.speed = 0;
            controller.agent.angularSpeed = 0;
        }
        if (controller.activate)
            controller.shield.enabled = true;
        else
            controller.shield.enabled = false;
        if (controller.action) 
            controller.RotateTowardsPlayer(6);
        if (controller.end)
            controller.SetState(new PEscudoIdle(controller));

    }
}
