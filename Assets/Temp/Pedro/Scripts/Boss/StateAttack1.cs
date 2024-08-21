using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttack1 : IBossFSM
{
    BossStateMachine controller;
    bool isAttackingCombo = false;
    float timer;

    public StateAttack1(BossStateMachine controller)
    {
        this.controller = controller;
    }

    public void OnEnter()
    {
        Debug.Log("To no ataque 1");
        controller.SortNumber();  // Sorteia o número
        Debug.Log(controller.sortedNumber);
        timer = Time.time + 1.2f;
        controller.boss.speed = 0;
        controller.boss.SetDestination(controller.player.transform.position);
    }

    public void OnExit()
    {
        if (!isAttackingCombo)
        {
            controller.animator.SetBool("Attack1", false);
        }
        controller.animationIsEnded = false;
    }

    public void OnUpdate()
    {
        controller.animator.SetBool("Attack1", true);
        if (controller.sortedNumber > 0.5f && controller.animationIsEnded)
        {
            Debug.Log("Combou");
            isAttackingCombo = true;
            controller.SetState(new StateIdle(controller));
        }
        else if (controller.sortedNumber <= 0.5f && controller.attack1toIdle)
        {
            controller.SetState(new StateAttack3(controller));
        }
    }
}
