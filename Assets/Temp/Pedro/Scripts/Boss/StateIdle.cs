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
        controller.boss.SetDestination(controller.player.transform.position);
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
        if (controller.TargetDir().magnitude <= 25 && controller.TargetDir().magnitude > 15)
        {
            controller.SetState(new WalkState(controller));
        }
        else if (controller.TargetDir().magnitude > 25)
        {
            controller.SetState(new RunState(controller));
        }
        else if (controller.TargetDir().magnitude <= 15)
        {
            if (controller.sortedNumber > 0.2f)
            {
                controller.SetState(new StateAttack1(controller));
            }
            else
            {
                controller.SetState(new StateAttack3(controller));
            }
        }
    }
}
