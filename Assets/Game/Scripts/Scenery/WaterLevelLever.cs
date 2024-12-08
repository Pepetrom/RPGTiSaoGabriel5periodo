using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLevelLever : MonoBehaviour
{
    bool activated = true;
    public Animator animator;
    public int[] heightIds;
    public bool completeLever = false;
    public GameObject lever;
    public ObjectThatMove objectThatMove;
    public void Activate()
    {
        if (!completeLever)
        {
            return;
        }
        if (activated)
        {
            PlayerController.instance.audioMan.PlayAudio(5);
            activated = false;
            animator.SetTrigger("Deactivate");
            objectThatMove.ChangeLocation(heightIds[0]);
        }
        else
        {
            PlayerController.instance.audioMan.PlayAudio(5);
            activated = true;
            animator.SetTrigger("Activate");
            objectThatMove.ChangeLocation(heightIds[1]);
        }
    }
}
