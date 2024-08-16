using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TartarugaPatrolState : ITartarugaInterface
{
    TartarugaStateMachine controller;
    float time;
    public TartarugaPatrolState(TartarugaStateMachine controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        time = Time.time + controller.patrollingCooldown;
        controller.Patrolling(controller.patrolCenter);
        Debug.Log("Comecei a patrulha");
    }

    public void OnExit()
    {
        Debug.Log("Parei de patrulhar");
    }

    public void OnUpdate()
    {
        if (Time.time > time)
        {
            controller.Patrolling(controller.patrolCenter);
            time = Time.time + controller.patrollingCooldown;
        }
        if (controller.IsNearTarget())
        {
            controller.SetState(new TartarugaFollowState(controller));
        }
        Debug.Log("Patrulhando");
    }
}
