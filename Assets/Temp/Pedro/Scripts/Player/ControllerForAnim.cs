using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerForAnim : MonoBehaviour
{
    public int slot;
    public void EventOne()
    {
        PlayerController.instance.atacks[slot].EventOne();
    }
    public void EventTwo()
    {
        PlayerController.instance.atacks[slot].EventTwo();
    }
    public void EventThree()
    {
        PlayerController.instance.atacks[slot].EventThree();
    }
    public void AtackEnd()
    {
        PlayerController.instance.atacks[slot].AtackEnd();
    }
}
