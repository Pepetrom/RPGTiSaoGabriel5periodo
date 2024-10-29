using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon 
{
    public bool CanBeInterupted();
    public void SetSlot(int slot);
    public void AtackStart(bool heavy);
    public void AtackUpdate();
    public void FacePlayerMouse();
    public void StartRegisterHit();
    public void StopRegisterHit();
    public void Hit(Collider other);
    public void OpenComboWindow();
    public void CloseComboWindow();
    public void AtackEnd();
    public void InteruptAtack();
    public void StoreCommand(int which);
    public void Atack1Start();
    public void Atack1Hit();
    public void Atack2Start();
    public void Atack2Hit();
    public void Atack3Start();
    public void Atack3Hit();
    public void AtackHeavyStart();
    public void AtackHeavyHit();
}
