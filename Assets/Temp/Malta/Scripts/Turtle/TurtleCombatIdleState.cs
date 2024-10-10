using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TurtleCombatIdleState : ITurtleStateMachine
{
    TurtleStateMachine controller;

    public TurtleCombatIdleState(TurtleStateMachine controller)
    {
        this.controller = controller;
    }

    public void OnEnter()
    {
        #region bools
        controller.animator.SetBool("Attack1", false);
        controller.animator.SetBool("Attack2", false);
        controller.animator.SetBool("att1att2", false);
        controller.animator.SetBool("att2att3", false);
        controller.animator.SetBool("Attack3", false);
        controller.animator.SetBool("Cannonatt3", false);
        controller.animator.SetBool("Cannon",false);
        #endregion
        controller.leftHand.gameObject.SetActive(false);
        controller.rightHand.gameObject.SetActive(false);
        controller.combed = false;
        controller.SortNumber();
        controller.rb.isKinematic = false;
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
        if (controller.hp.playerHit)
        {
            controller.SetState(new TurtleStunState(controller));
            controller.hp.playerHit = false;
        }
        if (controller.TargetDir().magnitude >= controller.minCannonRange && controller.TargetDir().magnitude <= controller.maxCannonRange)
        {
            controller.SetState(new TurtleCannonState(controller));
        }
        else if(controller.TargetDir().magnitude >= controller.meleeRange && controller.TargetDir().magnitude < controller.minCannonRange)
        {
            controller.animator.SetBool("IsWalking", true);
            controller.SetState(new TurtleWalkState(controller));
        }
        else if (controller.TargetDir().magnitude > controller.maxCannonRange)
        {
            controller.SetState(new TurtleRunState(controller));
        }
        else
        {
            if (controller.attack2Counter >= 2)
            {
                controller.attack2Counter = 0;
                controller.SetState(new TurtleAtt1State(controller));
            }
            else
            {
                if (controller.sortedNumber <= 0.4f)
                {
                    controller.lastAttack = "Attack1";
                    controller.attack2Counter = 0; 
                    controller.SetState(new TurtleAtt1State(controller));
                }
                else if (controller.sortedNumber > 0.4f)
                {
                    if (controller.lastAttack == "Attack2")
                    {
                        controller.attack2Counter++;
                    }
                    else
                    {
                        controller.attack2Counter = 1;
                    }

                    controller.lastAttack = "Attack2";
                    controller.SetState(new TurtleAtt2State(controller));
                }
            }
        }
    }
}
