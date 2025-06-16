using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleDeathState : ITurtleStateMachine
{
    TurtleStateMachine controller;
    public TurtleDeathState(TurtleStateMachine controller)
    {
        this.controller = controller;
        GameManager.instance.RemoveEnemy(controller.gameObject);
        Vector3 dirToPlayer = PlayerController.instance.transform.position - controller.transform.position;
        Vector3 newDir = dirToPlayer.normalized;
        Quaternion bodyRotation = Quaternion.LookRotation(newDir);
        controller.transform.rotation = bodyRotation;
        controller.DestroyTurtle(controller.hpCanvas);
    }
    public void OnEnter()
    {
        controller.GetComponent<Collider>().enabled = false;
        GameManager.instance.Score(200);
    }

    public void OnExit()
    {
        controller.end = false;
        controller.active = false;
    }

    public void OnUpdate()
    {
        if (controller.active)
        {
            controller.agent.enabled = false;
            controller.rb.isKinematic = false;
            controller.KB(-20);
        }
        else
        {
            controller.agent.enabled = true;
            controller.rb.isKinematic = true;
        }
    }
}
