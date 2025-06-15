using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerForAnim : MonoBehaviour
{
    public int slot;
    public AudioManager audioMan;
    bool step = true;
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
    public void Dash()
    {
        PlayerController.instance.atacks[slot].AtackStartAction();
    }
    public void AtackEnd()
    {
        PlayerController.instance.atacks[slot].AtackEnd();
    }
    public void EstusEnd(){
        PlayerController.instance.ResetAllActions();
    }
    public void PlaySound(int soundId)
    {
        audioMan.PlayAudio(soundId);
    }
    public void Respawn()
    {
        GameManager.instance.Respawn();
    }
}
