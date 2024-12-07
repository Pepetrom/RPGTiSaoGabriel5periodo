using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorquinFSMReceiver : MonoBehaviour
{
    public PorquinStateMachine porquin;
    public void AttackIdle()
    {
        porquin.AttackIdle();
    }
    public void Combo()
    {
        porquin.Combo();
    }
    public void Antecipation()
    {
        porquin.Antecipation();
    }
    public void Activate()
    {
        porquin.Activate();
    }
    public void ImpulseEvent()
    {
        porquin.ImpulseEvent();
    }
    public void Deactivate()
    {
        porquin.Deactivate();
    }
}

