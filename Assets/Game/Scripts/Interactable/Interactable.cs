using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool isInRange = false;
    public abstract void Interact();
    public Renderer rendererInteractable;
    Material oldMaterial;
    private void Start()
    {
        oldMaterial = rendererInteractable.material;
    }
    public void Enter()
    {
        isInRange = true;
        if (rendererInteractable) rendererInteractable.material = GameManager.instance.glowingMaterial;
    }
    public void Exit()
    {
        isInRange = false;
        if (rendererInteractable) rendererInteractable.material = oldMaterial;
    }
    public bool IsInRange()
    {
        return isInRange;
    }
}
