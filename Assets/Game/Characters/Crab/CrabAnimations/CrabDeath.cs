using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabDeath : ICrabInterface
{
    CrabFSM controller;
    public CrabDeath(CrabFSM controller) {  this.controller = controller; }

    public void OnEnter()
    {
        UIItems.instance.ShowBOSSHUD(false);
        controller.ownCollider.enabled = false;
        controller.ownFire.Stop();
    }

    public void OnExit()
    {
        controller.end = false;
        GameManager.instance.Score(3000);
    }

    public void OnUpdate()
    {
        if (controller.end)
        {
            controller.Die(controller.gameObject);
        }
    }
}
