using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : Interactable
{
    public override void Interact()
    {
        GameManager.instance.Bonfire(!GameManager.instance.bonfire.activeSelf);
        PlayerController.instance.audioMan.PlayAudio(6);
    }
}
