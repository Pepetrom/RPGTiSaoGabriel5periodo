using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineReceiver : MonoBehaviour
{
    public BossStateMachine boss;
    public void OnAnimationEvent()
    {
        boss.OnAnimationEvent();
    }
    public void Attack1toIdle()
    {
        boss.Attack1toIdle();
    }
}
