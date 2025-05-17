using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIReceiver : MonoBehaviour
{
    public void DeathAnim()
    {
        UIItems.instance.DeathAnim();
    }
    public void GearEnd()
    {
        UIItems.instance.GearEnd();
    }
}
