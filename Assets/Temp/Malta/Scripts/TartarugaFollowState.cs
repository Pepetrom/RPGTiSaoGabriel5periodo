using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TartarugaFollowState : ITartarugaInterface
{
    TartarugaStateMachine controller;
    public TartarugaFollowState(TartarugaStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        Debug.Log("Comecei a seguir");
    }

    public void OnExit()
    {
        Debug.Log("Parei de seguir");
    }

    public void OnUpdate()
    {
        controller.tartaruga.SetDestination(controller.player.transform.position);
        if(!controller.IsNearTarget())
        {
            controller.SetState(new TartarugaPatrolState(controller));
        }
        Debug.Log("Seguindo");
    }
}
