using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnnelieseDialogue : MonoBehaviour
{
    public string[] dialoguePaths;
    bool canTalk = false;
    [SerializeField] int dialogueIndex = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canTalk = true;
            UIItems.instance.CanTalk(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canTalk = false;
            UIItems.instance.CanTalk(false);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (canTalk)
            {
                switch (QuestManager.instance.annelieseDialogueIndex)
                {
                    case 0:
                        PlayerController.instance.StopAllActions();
                        DialogueManager.instance.LoadDialogue(dialoguePaths[0]);
                        DialogueManager.instance.printLine(DialogueManager.instance.text);
                        if (DialogueManager.instance.dialogueEnded)
                            QuestManager.instance.annelieseDialogueIndex = 1;
                        break;
                    case 1:
                        PlayerController.instance.StopAllActions();
                        DialogueManager.instance.LoadDialogue(dialoguePaths[1]);
                        DialogueManager.instance.printLine(DialogueManager.instance.text);
                        if(QuestManager.instance.essence)
                            QuestManager.instance.annelieseDialogueIndex = 2;
                        break;
                    case 2:
                        PlayerController.instance.StopAllActions();
                        DialogueManager.instance.LoadDialogue(dialoguePaths[2]);
                        DialogueManager.instance.printLine(DialogueManager.instance.text);
                        UIItems.instance.ShowEssence(false);
                        break;
                }
            }
        }
    }
}
