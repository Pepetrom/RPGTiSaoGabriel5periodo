using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Essence : Interactable
{
    public override void Interact()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            UIItems.instance.ShowEssence(true);
            QuestManager.instance.essence = true;
            Destroy(gameObject);
        }
    }
}
