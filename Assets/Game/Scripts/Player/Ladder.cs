using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public bool entered;
    public void StartClimb()
    {
        PlayerController.instance.StopAllActions();
        PlayerController.instance.animator.SetTrigger("StartLadder");
    }
    public void EndClimb()
    {
        PlayerController.instance.ResetAllActions();
        PlayerController.instance.animator.SetTrigger("EndLadder");
    }
}
