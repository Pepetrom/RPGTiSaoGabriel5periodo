using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabWalkFState : ICrabInterface
{
    CrabFSM controller;
    public CrabWalkFState(CrabFSM controller)
    {
        this.controller = controller;
    }
    public void OnEnter()
    {
        controller.animator.SetBool("isWalking", true);
    }

    public void OnExit()
    {
        controller.animator.SetBool("isWalking", false);
    }

    public void OnUpdate()
    {
        controller.agent.SetDestination(controller.player.transform.position);
        controller.RotateTowardsPlayer(6);
    }
}
