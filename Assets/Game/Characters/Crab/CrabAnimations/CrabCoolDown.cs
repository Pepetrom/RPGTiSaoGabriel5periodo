using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabCoolDown : ICrabInterface
{
    CrabFSM controller;
    public CrabCoolDown(CrabFSM controller) {  this.controller = controller; }
    public void OnEnter()
    {
        controller.animator.SetBool("isFurnace", false);
        controller.ownFire.Stop();
    }

    public void OnExit()
    {
        controller.ownFire.Play();
        controller.end = false;
    }

    public void OnUpdate()
    {
        if (controller.end)
        {
            controller.animator.SetBool("cooling", false);
            controller.SetState(new CrabAttController(controller));
        }
    }
}
