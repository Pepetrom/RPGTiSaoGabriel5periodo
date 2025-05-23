using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : Interactable
{
    public ObjectThatMove objectThatMove;
    public int locationID = 0;
    public override void Interact()
    {
        Activate();
    }
    void Activate()
    {
        objectThatMove.ChangeLocation(locationID);
    }
}
