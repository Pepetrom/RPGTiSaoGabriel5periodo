using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TurtleCombatIdleState : ITurtleStateMachine
{
    TurtleStateMachine controller;

    public TurtleCombatIdleState(TurtleStateMachine controller)
    {
        this.controller = controller;
    }

    public void OnEnter()
    {
        controller.animator.SetBool("Attack1", false);
        controller.animator.SetBool("Attack2", false);
        controller.animator.SetBool("att1att2", false);
        controller.animator.SetBool("att2att3", false);
        controller.combed = false;
        controller.SortNumber();
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
        if (controller.attack2Counter >= 2)
        {
            controller.attack2Counter = 0;
            controller.SetState(new TurtleAtt1State(controller));
        }
        else
        {
            if (controller.sortedNumber <= 0.4f)
            {
                controller.lastAttack = "Attack1";
                controller.attack2Counter = 0; 
                controller.SetState(new TurtleAtt1State(controller));
            }
            else if (controller.sortedNumber > 0.5f)
            {
                if (controller.lastAttack == "Attack2")
                {
                    controller.attack2Counter++;
                }
                else
                {
                    controller.attack2Counter = 1;
                }

                controller.lastAttack = "Attack2";
                controller.SetState(new TurtleAtt2State(controller));
            }
        }
    }
}
