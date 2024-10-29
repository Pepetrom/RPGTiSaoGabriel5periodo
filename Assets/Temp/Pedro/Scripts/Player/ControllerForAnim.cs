using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerForAnim : MonoBehaviour
{
    public int slot;
    public void StartRegisterHit()
    {
        PlayerController.instance.atacks[slot].StartRegisterHit();
    }
    public void StopRegisterHit()
    {
        PlayerController.instance.atacks[slot].StopRegisterHit();
    }
    public void OpenComboWindow()
    {
        PlayerController.instance.atacks[slot].OpenComboWindow();
    }
    public void CloseComboWindow()
    {
        PlayerController.instance.atacks[slot].CloseComboWindow();
    }
    public void AtackEnd()
    {
        PlayerController.instance.atacks[slot].AtackEnd();
    }
    public void Atack1Effect()
    {

    }
    public void Rune()
    {
        //PlayerController.instance.RuneEffect();
    }
}
