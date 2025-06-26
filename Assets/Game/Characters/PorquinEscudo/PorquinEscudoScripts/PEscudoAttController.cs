using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEscudoAttController : IPorquinEscudo
{
    PorquinEscudoFSM controller;
    public PEscudoAttController(PorquinEscudoFSM controller) { this.controller = controller; }
    public void OnEnter()
    {
        controller.animator.SetBool("swing", false);
        controller.SortNumber();
        controller.swingRate -= 20;
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if(controller.randomValue > controller.basicAtt)
            controller.SetState(new PEscudoAtt1(controller));

    }
}
