using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : IBossFSM
{
    BossStateMachine controller;

    public StateIdle(BossStateMachine controller)
    {
        this.controller = controller;
    }

    public void OnEnter()
    {
        controller.SortNumber();  // Sorteia o número
        controller.boss.speed = 0;
        controller.att1att3 = false;
        //Lembrar de resetar todas as animações de attack quando o boss volta para o idle
        controller.animator.SetBool("Attack1", false);
        controller.animator.SetBool("Attack2", false);
        controller.animator.SetBool("Attack3", false);
        controller.boss.SetDestination(controller.player.transform.position);
    }

    public void OnExit()
    {
        controller.boss.SetDestination(controller.player.transform.position);
    }

    public void OnUpdate()
    {
        controller.boss.SetDestination(controller.player.transform.position);
        if (controller.TargetDir().magnitude <= 25 && controller.TargetDir().magnitude > 15)
        {
            controller.SetState(new WalkState(controller));
        }
        else if (controller.TargetDir().magnitude > 25)
        {
            //if(controller.sortedNumber > 0.8f)
            //controller.SetState(new RunState(controller));
            //else 
            //controller.SetState(new StateThrow(controller));
            controller.SetState(new RunState(controller));
        }
        else if (controller.TargetDir().magnitude <= 15)
        {
            if (controller.sortedNumber < 0.3f)
            {
                controller.SetState(new StateAttack1(controller));
            }
            else if(controller.sortedNumber >= 0.3f && controller.sortedNumber < 0.6f)
            {
                controller.SetState(new StateAttack2(controller));
            }
            else
            {
                controller.SetState(new StateAttack3(controller));
            }
        }
    }
}
