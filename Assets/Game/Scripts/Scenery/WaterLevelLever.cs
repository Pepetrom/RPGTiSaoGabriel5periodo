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
            PlayerController.instance.audioMan.PlayAudio(6);
            return;
        };
        lever.SetActive(true);
        if (activated)
        {
            activated = false;
            PlayerController.instance.audioMan.PlayAudio(5);
            animator.SetTrigger("Activate");
            ObjectThatMove.instance.ChangeLocation(heightIds[0]);
        }
        else
        {
            activated = true;
            PlayerController.instance.audioMan.PlayAudio(5);
            animator.SetTrigger("Deactivate");
            ObjectThatMove.instance.ChangeLocation(heightIds[1]);
        }
    }
}
