using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleFSMReceiver : MonoBehaviour
{
    public TurtleStateMachine turtle;
    public void AttackIdle()
    {
        turtle.AttackIdle();
    }
    public void Combo()
    {
        turtle.Combo();
    }
    public void Antecipation()
    {
        turtle.Antecipation();
    }
    public void ImpulseEvent()
    {
        turtle.ImpulseEvent();
    }
}
