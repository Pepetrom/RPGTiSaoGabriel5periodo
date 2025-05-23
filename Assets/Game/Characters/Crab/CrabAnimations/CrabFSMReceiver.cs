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
    public void Activate()
    {
        crab.Activate();
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
    public void Jump()
    {
        crab.Jump();
    }
    public void StopJump()
    {
        crab.StopJump();
    }
    public void Fall()
    {
        crab.Fall();
    }
    public void StopFall()
    {
        crab.StopFall();
    }
    public void SpecificEvent()
    {
        crab.SpecificEvent();
    }
    public void DeactivateSpecificEvent()
    {
        crab.DeactivateSpecificEvent();
    }
    public void ActivateBigWall()
    {
        crab.ActivateBigWall();
    }
    public void DeactivateBigWall()
    {
        crab.DeactivateBigWall();
    }
    public void StopFireWall()
    {
        crab.StopFireWall();
    }
    public void PlaySoundAttached(string path)
    {
        FMODAudioManager.instance.PlaySoundAttached(path);
    }
    public void OwnColliderActivate()
    {
        crab.OwnColliderActivate();
    }
}
