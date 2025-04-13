using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabFSMReceiver : MonoBehaviour
{
    public CrabFSM crab;

    public void Antecipation()
    {
        crab.Antecipation();
    }
    public void Deactivate()
    {
        crab.Deactivate();
    }
    public void End()
    {
        crab.End();
    }
    public void Combo()
    {
        crab.Combo();
    }
}
