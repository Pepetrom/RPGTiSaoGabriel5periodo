using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineReceiver : MonoBehaviour
{
    //receptor dos métodos relacionados aos event triggers
    public BossStateMachine boss;
    public void OnAnimationEvent()
    {
        boss.OnAnimationEvent();
    }
    public void Attack1toIdle()
    {
        boss.Attack1toIdle();
    }
    public void AllowColliderRightHand()
    {
        boss.AllowColliderRightHand();
    }
    public void GrabRockEvent()
    {
        boss.GrabRockEvent();
    }
    public void ThrowRockEvent()
    {
        boss.ThrowRockEvent();
    }
}
