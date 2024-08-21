using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    public void SetSlot(int slot);
    public void ActionStart();
    public void DoAction();
    public void ActionEnd();
}
