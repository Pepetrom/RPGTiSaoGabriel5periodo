using JetBrains.Annotations;
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
        controller.damage = 10;
        Debug.Log("To no ataque 1");
        controller.SortNumber();  // Sorteia o número
        Debug.Log(controller.sortedNumber);
        timer = Time.time + 1.2f;
        controller.boss.speed = 0;
        controller.boss.SetDestination(controller.player.transform.position);
        controller.animator.SetBool("Attack1", true);
    }

    public void OnExit()
    {
        controller.rightHand.enabled = false;
        //Local que decide para qual animação o ataque se dirigirá, quando o combo for true o estado mudará para o ataque subsequente sem a necessidade de passar pelo idle
        if (!controller.att1att3) controller.animator.SetBool("Attack1", false);
        else controller.animator.SetBool("Att1Att3", true);
        controller.animationIsEnded = false;
        controller.attack1toIdle = false;
    }

    public void OnUpdate()
    {
        //attHit é o evento que habilita o colisor e o player pode ser atingido
        if (controller.attHit)
        {
            controller.rightHand.enabled = true;
        }
        if (controller.sortedNumber <= 0.5f) // probabilidade
        {
            if (controller.animationIsEnded)
            {
                controller.Att1Att3();
                Debug.Log("Combou");
                controller.SetState(new StateAttack3(controller));
            }
        }
        else
        {
            if (controller.attack1toIdle)
            {
                Debug.Log("Não Combou");
                controller.SetState(new StateIdle(controller));
            }
        }
    }
}
