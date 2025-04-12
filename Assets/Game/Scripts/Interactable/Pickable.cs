using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : Interactable
{
    public int value = 50;
    public override void Interact()
    {
        PlayerInteract.instance.ObjectAutoDestruction(this);
        PlayerController.instance.audioMan.PlayAudio(5);
        GameManager.instance.Score(50);
        isInRange = false;
        Destroy(this.gameObject);
    }
}
