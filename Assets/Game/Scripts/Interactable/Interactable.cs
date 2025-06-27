using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool isInRange = false;
    public abstract void Interact();
    public Outline outline;
    private void Start()
    {
        if (outline) outline.enabled = false;
    }
    public void Enter()
    {
        isInRange = true;
        if (outline) outline.enabled = true;
    }
    public void Exit()
    {
        isInRange = false;
        if (outline) outline.enabled = false;
    }
    public bool IsInRange()
    {
        return isInRange;
    }
}
