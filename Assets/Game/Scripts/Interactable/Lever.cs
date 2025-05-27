using System;
using UnityEngine;

public class Lever : Interactable
{
    bool activated = false;
    public Animator animator;
    public Valve valve;
    public string reminderText = "";
    public override void Interact()
    {
        Activate();
    }
    public void Activate()
    {
        if (activated) return;
        valve.CanBeActivated(true);
        FMODAudioManager.instance.PlayOneShot(FMODAudioManager.instance.lever, transform.position);
        activated = true;
        animator.SetTrigger("Activate");
        if (reminderText.Length > 0) LittleReminder.instance.littleReminder(reminderText);
    }
}
