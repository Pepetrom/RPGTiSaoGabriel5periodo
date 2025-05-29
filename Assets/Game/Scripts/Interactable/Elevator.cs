using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : Interactable
{
    public ObjectThatMove objectThatMove;
    public int locationID = 0;
    bool lastActivated = false;
    public override void Interact()
    {
        Activate();
    }
    void Activate()
    {
        FMODAudioManager.instance.PlayOneShot(FMODAudioManager.instance.lever, transform.position);
        objectThatMove.ChangeLocation(lastActivated ? 0 : 1);
        lastActivated = !lastActivated;
    }
}
