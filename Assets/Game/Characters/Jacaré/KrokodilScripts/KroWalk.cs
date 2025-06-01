using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroWalk : IKrokodil
{
    KrokodilFSM controller;
    public KroWalk(KrokodilFSM controller) {  this.controller = controller; }

    public void OnEnter()
    {
        controller.agent.speed = 10f;
    }

    public void OnExit()
    {
       
    }

    public void OnUpdate()
    {
        controller.agent.SetDestination(controller.player.transform.position);
        controller.RotateTowardsPlayer(6);
        if(controller.TargetDir().magnitude < controller.meleeRange + 5)
        {
            controller.agent.speed = 0f;
            if (controller.randomValue < 0.7f)
            { 
                controller.animator.SetBool("kick", true);
                controller.SetState(new KroKick(controller));
            }
            else
            {
                controller.animator.SetBool("heavy", true);
                controller.SetState(new KroHeavyAtt(controller));
            }
        }
    }
}
