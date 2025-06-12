using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KroAttController : IKrokodil
{
    KrokodilFSM controller;
    public KroAttController(KrokodilFSM controller) { this.controller = controller; }

    public void OnEnter()
    {
        controller.animator.SetBool("att2", false);
        controller.animator.SetBool("swing", false);
        controller.animator.SetBool("att2comb2", false);
        controller.animator.SetBool("heavy", false);
        controller.SortNumber();
        Debug.Log("Entrei no attController");
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if(controller.TargetDir().magnitude < controller.meleeRange)
        {
            if (controller.randomValue < 50)
            {
                controller.SetState(new KroJump(controller));
            }
            else
            {
                // ao inv�s do ataque, colocar o dash para tr�s
                controller.animator.SetBool("att2", true);
                controller.SetState(new KroAtt2(controller));
            }
        }
        else
        {
            controller.animator.SetBool("isAttack", false);
            controller.SetState(new KroIdle(controller));
        }
        /*if (controller.TargetDir().magnitude < controller.meleeRange + 4)
        {
            if (controller.randomValue > controller.basicAtt)
            {
                controller.animator.SetBool("att2", true);
                controller.SetState(new KroAtt2(controller));
            }
            else
            {
                controller.animator.SetBool("att1", true);
                controller.SetState(new KroAtt1(controller));
            }
            return;
        }
        else
        {
            // colocar o dash para frente
            controller.animator.SetBool("isAttack", false);
            controller.SetState(new KroIdle(controller));
        }*/
    }
}
