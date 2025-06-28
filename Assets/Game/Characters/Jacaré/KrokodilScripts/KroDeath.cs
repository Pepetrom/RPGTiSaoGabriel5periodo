using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroDeath : IKrokodil
{
    KrokodilFSM controller;
    public KroDeath(KrokodilFSM controller) {  this.controller = controller; }

    public void OnEnter()
    {
        controller.ownCollider.enabled = false;
    }

    public void OnExit()
    {
        BossManager.instance.krokodil = false;
        BossManager.instance.KrokodilSetUp(false);
        controller.end = false;
    }

    public void OnUpdate()
    {
        if (controller.end)
        {
            BossManager.instance.KrokodilSetUp(false);
            GameManager.instance.Score(5000);
            controller.DestroyBoss(controller.gameObject);
        }
    }
}
