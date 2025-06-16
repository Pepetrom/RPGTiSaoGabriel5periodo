using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleAttController : ITurtleStateMachine
{
    TurtleStateMachine controller;
    public TurtleAttController( TurtleStateMachine controller) {  this.controller = controller; }
    public void OnEnter()
    {
        controller.SortNumber();
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if(controller.sortedNumber > controller.attRate)
        {
            controller.animator.SetBool("att1", true);
            controller.SetState(new TurtleAtt1State(controller));
        }
        else
        {
            controller.animator.SetBool("att2", true);
            controller.SetState(new TurtleAtt2State(controller));
        }
    }
}
