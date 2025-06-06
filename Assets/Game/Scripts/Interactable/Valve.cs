using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valve : Interactable
{
    bool activated = false;
    public bool canBeActivated = false;
    public Animator animator;
    public ObjectThatMove objectThatMove;
    public int locationID = 0;
    public GameObject greenLight, redLight;
    public string reminderText = "";
    public override void Interact()
    {
        Activate();
    }
    public void Activate()
    {
        if (activated) return;
        if (!canBeActivated) return;
        FMODAudioManager.instance.PlayOneShot(FMODAudioManager.instance.valve, transform.position);
        FMODAudioManager.instance.PlayOneShot(FMODAudioManager.instance.fillingWater, PlayerController.instance.transform.position);
        activated = true;
        if(animator)animator.SetTrigger("Activate");
        objectThatMove.ChangeLocation(locationID);
        if(reminderText.Length > 0) LittleReminder.instance.littleReminder(reminderText);
    }
    public void CanBeActivated(bool can)
    {
        canBeActivated = can;
        if (redLight) redLight.SetActive(!can);
        if (greenLight) greenLight.SetActive(can);
    }
}
