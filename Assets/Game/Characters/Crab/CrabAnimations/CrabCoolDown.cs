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
        controller.ownFire.SetActive(false);
    }

    public void OnExit()
    {
        controller.ownFire.SetActive(true);
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
