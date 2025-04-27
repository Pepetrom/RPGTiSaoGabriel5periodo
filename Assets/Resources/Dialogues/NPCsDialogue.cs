using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCsDialogue : MonoBehaviour
{
    public string[] dialoguePaths;
    bool canTalk = false;
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
                switch (QuestManager.instance.davidDialogueIndex)
                {
                    case 0:
                        PlayerController.instance.StopAllActions();
                        DialogueManager.instance.LoadDialogue(dialoguePaths[0]);
                        DialogueManager.instance.printLine(DialogueManager.instance.text);
                        if (DialogueManager.instance.dialogueEnded)
                            QuestManager.instance.davidDialogueIndex = 1;
                        break;
                    case 1:
                        PlayerController.instance.StopAllActions();
                        DialogueManager.instance.LoadDialogue(dialoguePaths[1]);
                        DialogueManager.instance.printLine(DialogueManager.instance.text);
                        if (DialogueManager.instance.dialogueEnded)
                            QuestManager.instance.davidDialogueIndex = 2;
                        break;
                    case 2:
                        PlayerController.instance.StopAllActions();
                        DialogueManager.instance.LoadDialogue(dialoguePaths[2]);
                        DialogueManager.instance.printLine(DialogueManager.instance.text);
                        break;
                    case 3:
                        PlayerController.instance.StopAllActions();
                        DialogueManager.instance.LoadDialogue(dialoguePaths[3]);
                        DialogueManager.instance.printLine(DialogueManager.instance.text);
                        QuestManager.instance.canDropMedicine = true;
                        break;
                    case 4:
                        PlayerController.instance.StopAllActions();
                        DialogueManager.instance.LoadDialogue(dialoguePaths[4]);
                        DialogueManager.instance.printLine(DialogueManager.instance.text);
                        break;

                }
            }
        }
    }
}
