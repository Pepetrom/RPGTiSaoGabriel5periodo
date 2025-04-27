using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public int davidDialogueIndex, tortoiseDialogueIndex;
    public bool medicine = false, canDropMedicine = false;
    private void Awake()
    {
        instance = this;
    }
    public void Poem()
    {

    }
    public void DropMedicine()
    {
        UIItems.instance.ShowMedicine(true);
        medicine = true;    
    }
}
