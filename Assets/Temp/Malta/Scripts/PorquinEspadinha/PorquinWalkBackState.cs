using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorquinWalkBackState : IPorquinStateMachine
{
    PorquinStateMachine controller;

    public PorquinWalkBackState(PorquinStateMachine controller)
    {
        //Debug.Log("To aqui");
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.agent.speed = 1.5f;
    }

    public void OnExit()
    {
        //Debug.Log("Sai mane");
    }

    public void OnUpdate()
    {
        controller.RotateTowardsPlayer();
        controller.agent.SetDestination(controller.transform.position - controller.player.transform.position);
        if(controller.TargetDir().magnitude > controller.safeRange)
        {
            controller.animator.SetBool("isWalking", true);
            controller.SetState(new PorquinWalkState(controller));
        }
    }
}
