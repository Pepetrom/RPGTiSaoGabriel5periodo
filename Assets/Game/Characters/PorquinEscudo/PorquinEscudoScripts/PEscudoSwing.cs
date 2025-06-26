using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEscudoSwing : IPorquinEscudo
{
    PorquinEscudoFSM controller;
    Vector3 swingPos;
    public PEscudoSwing(PorquinEscudoFSM controller) {  this.controller = controller; }
    public void OnEnter()
    {
        controller.SortNumber();
        controller.agent.speed = 2.3f;
        swingPos = controller.Swing();
        controller.swingRate += 20;
    }

    public void OnExit()
    {
        controller.agent.speed = 0f;
    }

    public void OnUpdate()
    {
        controller.agent.SetDestination(swingPos);
        controller.RotateTowardsPlayer(8);
        controller.SwingMove();
        if (controller.HasReachedDestination())
        {
            if(controller.TargetDir().magnitude <= controller.runRange)
            {
                if (controller.randomValue > controller.swingRate)
                {
                    swingPos = controller.Swing();
                    controller.swingRate += 20;
                }
                else
                {
                    controller.animator.SetBool("canAtt", true);
                    controller.SetState(new PEscudoAttController(controller));
                }
            }
            else
            {
                controller.animator.SetBool("swing", false);
                controller.SetState(new PEscudoIdle(controller));
            }
        }

    }
    
}
