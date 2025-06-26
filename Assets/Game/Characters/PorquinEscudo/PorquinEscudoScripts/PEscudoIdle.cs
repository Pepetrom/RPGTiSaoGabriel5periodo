using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEscudoIdle : IPorquinEscudo
{
    PorquinEscudoFSM controller;
    public PEscudoIdle(PorquinEscudoFSM controller) {  this.controller = controller; }

    public void OnEnter()
    {
        controller.animator.SetBool("canAtt", false);
        controller.SortNumber();
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        if(controller.TargetDir().magnitude < controller.meleeRange)
        {
            if(controller.randomValue > controller.swingRate)
            {
                controller.SetState(new PEscudoSwing(controller));
            }
            else
            {
                controller.SetState(new PEscudoAttController(controller));
            }
        }
        else if(controller.TargetDir().magnitude < controller.runRange)
        {
            controller.SetState(new PEscudoWalk(controller));
        }
        else if(controller.TargetDir().magnitude < controller.patrolRange)
        {

        }
        else
        {
            controller.SetState(new PEscudoPatrol(controller));
        }
    }
}
