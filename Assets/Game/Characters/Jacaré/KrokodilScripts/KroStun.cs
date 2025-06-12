using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroStun : IKrokodil
{
    KrokodilFSM controller;
    public KroStun(KrokodilFSM controller) {  this.controller = controller; }

    public void OnEnter()
    {
        controller.animator.SetBool("isAttack", false);
        controller.stun.GetComponent<ParticleSystem>().Play();
        controller.posture = controller.maxPosture;
    }

    public void OnExit()
    {
        controller.end = false;
    }

    public void OnUpdate()
    {
        if (controller.end)
        {
            controller.SetState(new KroIdle(controller));
        }
    }
}
