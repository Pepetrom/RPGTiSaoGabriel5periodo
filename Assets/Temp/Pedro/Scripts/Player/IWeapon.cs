using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon 
{
    public bool CanBeInterupted();
    public void SetSlot(int slot);
    public void AtackStart();
    public void AtackUpdate();
    public void EventOne();
    public void EventTwo();
    public void EventThree();
    public void Hit(Collider other);
    public void InteruptAtack();
    public void AtackEnd();
}
