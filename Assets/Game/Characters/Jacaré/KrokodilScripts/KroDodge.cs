using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroDodge : IKrokodil
{
    KrokodilFSM controller;
    public KroDodge(KrokodilFSM controller) {  this.controller = controller; }

    public void OnEnter()
    {
        controller.animator.SetTrigger("dodge");
    }

    public void OnExit()
    {
        controller.end = false;
    }

    public void OnUpdate()
    {
        if (controller.end)
            controller.SetState(new KroAttController(controller));
    }
}
