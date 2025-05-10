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
    public GameObject canBeActivatedIndication;
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
        animator.SetTrigger("Activate");
        objectThatMove.ChangeLocation(locationID);
    }
    public void CanBeActivated(bool can)
    {
        canBeActivated = can;
        canBeActivatedIndication.SetActive(can);
    }
}
