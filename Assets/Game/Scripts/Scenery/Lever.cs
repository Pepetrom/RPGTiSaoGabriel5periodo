using System;
using UnityEngine;

public class Lever : MonoBehaviour
{
    bool activated = false;
    public Animator animator;
    public Valve valve;
    public void Activate()
    {
        if (activated)
        {
            return;
        }
        valve.CanBeActivated(true);
        PlayerController.instance.audioMan.PlayAudio(5);
        activated = true;
        animator.SetTrigger("Activate");
    }
}
