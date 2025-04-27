using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bonfire : Interactable
{
    public Button myLocation;
    public string myName;
    public Text text;
    public int fontSize;
    public Transform myPosition;
    public override void Interact()
    {
        GameManager.instance.lastBonfireRestedAt = myPosition;
        UIItems.instance.AddLocation(myLocation);
        text.text = myName;
        text.fontSize = fontSize;
        GameManager.instance.Bonfire(!GameManager.instance.bonfire.activeSelf);
        //PlayerController.instance.audioMan.PlayAudio(6);
    }
}
