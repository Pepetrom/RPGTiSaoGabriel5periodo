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
                controller.animator.SetBool("swing", true);
                controller.SetState(new PEscudoSwing(controller));
            }
            else
            {
                controller.swingRate -= 10;
                if(controller.randomValue > controller.basicAtt)
                {
                    controller.SetState(new PEscudoAtt2(controller));
                }
                else
                {
                    controller.SetState(new PEscudoAtt1(controller));
                }
            }
        }
        else if(controller.TargetDir().magnitude < controller.runRange)
        {
            controller.SetState(new PEscudoWalk(controller));
        }
        else if(controller.TargetDir().magnitude < controller.patrolRange)
        {
            controller.SetState(new PEscudoRunAtt(controller));
        }
        else
        {
            controller.SetState(new PEscudoPatrol(controller));
        }
    }
}
