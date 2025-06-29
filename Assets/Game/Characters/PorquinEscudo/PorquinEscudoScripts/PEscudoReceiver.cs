using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEscudoReceiver : MonoBehaviour
{
    public PorquinEscudoFSM porquin;
    public void Antecipation()
    {
        porquin.Antecipation();
    }
    public void Activate()
    {
        porquin.Activate();
    }
    public void Deactivate()
    {
        porquin.Deactivate();
    }
    public void End()
    {
        porquin.End();
    }
    public void Action()
    {
        porquin.Action();
    }
    public void StopAction()
    {
        porquin.StopAction();
    }
    public void PlaySoundAttached(string path)
    {
        FMODAudioManager.instance.PlaySoundAttached(path);
    }
}
