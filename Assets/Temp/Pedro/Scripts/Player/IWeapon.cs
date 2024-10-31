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
    public void AtackStartAction();
    public void AtackHit(Collider other);
    public void AtackCriticalHit(Collider other);
    public void AtackHeavyHit(Collider other);
    public void AtackHeavyEffect();
}
