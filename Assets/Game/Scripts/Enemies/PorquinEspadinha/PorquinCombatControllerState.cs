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
        //Debug.Log("Controle");
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
            if (controller.sortedNumber <= 0.6f)
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
