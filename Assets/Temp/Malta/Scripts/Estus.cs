using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Estus : MonoBehaviour
{
    public int hp,q; // quantity

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && q > 0 )
        {
            RecoverHPbyItem(hp);
            q--;
            UIItems.UIItemsInstance.UpdateChesseQUI(q);
        }
    }
    public void RecoverHPbyItem(int value)
    {
        if((HPBar.hpbarInstance.currentHP + value) < HPBar.hpbarInstance.maxHP)
        {
            HPBar.hpbarInstance.currentHP += value;
        }
        else
        {
            HPBar.hpbarInstance.currentHP = HPBar.hpbarInstance.maxHP;
        }
    }
}
