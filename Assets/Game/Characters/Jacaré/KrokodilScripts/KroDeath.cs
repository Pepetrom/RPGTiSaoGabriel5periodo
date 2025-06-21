using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroDeath : IKrokodil
{
    KrokodilFSM controller;
    public KroDeath(KrokodilFSM controller) {  this.controller = controller; }

    public void OnEnter()
    {
        GameManager.instance.Score(5000);
        controller.ownCollider.enabled = false;
    }

    public void OnExit()
    {
        controller.end = false;   
    }

    public void OnUpdate()
    {
        if (controller.end)
            controller.Destroy(controller.gameObject);
    }
}
