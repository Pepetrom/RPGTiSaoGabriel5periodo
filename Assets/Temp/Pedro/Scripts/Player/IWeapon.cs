using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon 
{
    public bool CanBeInterupted();
    public void SetSlot(int slot);
    public void AtackStart();
    public void AtackUpdate();
    public void CloseComboWindow();
    public void StartRegisterHit();
    public void StopRegisterHit();
    public void OpenComboWindow();
    public void Hit(Collider other);
    public void InteruptAtack();
    public void AtackEnd();
}
