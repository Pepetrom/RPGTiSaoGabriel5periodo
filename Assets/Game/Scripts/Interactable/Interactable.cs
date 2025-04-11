using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool isInRange = false;
    public abstract void Interact();
    public void Enter()
    {
        isInRange = true;
    }
    public void Exit()
    {
        isInRange = false;
    }
    public bool IsInRange()
    {
        return isInRange;
    }
}
