using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBossFightBegins : IBossFSM
{
    BossStateMachine controller;
    float time;
    public StateBossFightBegins(BossStateMachine controller)
    {
        this.controller = controller;
        time = Time.time + 6;
    }
    public void OnEnter()
    {
        Debug.Log("Começou a bossfight");
    }

    public void OnExit()
    {
        Debug.Log("Meti o pé");
    }

    public void OnUpdate()
    {
        if(Time.time >= time)
        {
            controller.SetState(new StateIdle(controller));
        }
    }
}
