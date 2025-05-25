using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public int davidDialogueIndex, tortoiseDialogueIndex, annelieseDialogueIndex;
    public bool medicine = false, canDropMedicine = false, essence = false;
    public Sprite poem;
    private void Awake()
    {
        instance = this;
    }
    public void Poem()
    {
        UIItems.instance.ShowNotes(poem);
        GameManager.instance.Score(400);
    }
    public void DropMedicine()
    {
        UIItems.instance.ShowMedicine(true);
        medicine = true;    
    }
    public void Essence()
    {
        essence = true;
    }
}
