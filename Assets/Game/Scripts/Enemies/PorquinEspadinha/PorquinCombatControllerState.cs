using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorquinCombatControllerState : IPorquinStateMachine
{
    PorquinStateMachine controller;
    public PorquinCombatControllerState(PorquinStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.agent.enabled = true;
        controller.rb.isKinematic = true;
        controller.sword.enabled = false;

    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if (controller.attack2Counter >= 2)
        {
            controller.attack2Counter = 0;
            controller.SetState(new PorquinAtt1State(controller));
        }
        else
        {
            if (controller.sortedNumber <= 1f)
            {
                controller.fullCombatCounter++;
                controller.SetState(new PorquinAtt2State(controller));
            }
            else
            {
                controller.SetState(new PorquinAtt3State(controller));
            }
        }
    }
}
