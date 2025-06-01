using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroStartState : IKrokodil
{
    KrokodilFSM controller;
    public KroStartState(KrokodilFSM controller) {  this.controller = controller; }
    public void OnEnter()
    {
        controller.ownCollider.enabled = false;
        Debug.Log("To no start");
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if (controller.end)
        {
            controller.ownCollider.enabled = true;
            controller.animator.SetBool("isStart", false);
            controller.SetState(new KroIdle(controller));
        }
    }
}
