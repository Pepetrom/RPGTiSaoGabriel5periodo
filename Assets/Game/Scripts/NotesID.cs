using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotesID : Interactable
{
    public Sprite note;
    public override void Interact()
    {
        UIItems.instance.ActivatePressF();
        FMODAudioManager.instance.PlayOneShot(FMODAudioManager.instance.notes, transform.position);
        if (Input.GetKeyDown(KeyCode.F))
        {
            UIItems.instance.ShowNotes(note);
        }
    }
}
