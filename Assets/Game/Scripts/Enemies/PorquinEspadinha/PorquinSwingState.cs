using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorquinSwingState : IPorquinStateMachine
{
    PorquinStateMachine controller;
    Vector3 pos;
    Vector3 swingPos;
    public PorquinSwingState(PorquinStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.agent.speed = 2f;
        pos = controller.Swing();
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        controller.agent.SetDestination(pos);
        if (Vector3.Distance(controller.transform.position, pos) < 0.1f)
        {
            controller.SetState(new PorquinAtt2State(controller));
        }
    }
}
