using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCsDialogue : MonoBehaviour
{
    public string[] dialoguePaths;
    bool canTalk = false;
    public Text NPCname;
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
                NPCname.text = "DAVID";
                switch (QuestManager.instance.davidDialogueIndex)
                {
                    case 0:
                        PlayerController.instance.StopAllActions();
                        DialogueManager.instance.LoadDialogue(dialoguePaths[0]);
                        DialogueManager.instance.printLine(DialogueManager.instance.text);
                        if (DialogueManager.instance.dialogueEnded)
                            QuestManager.instance.davidDialogueIndex = 1;
                        SaveLoad.instance.saveData.player.davidDialogueIndex = 1;
                        SaveLoad.instance.Save();
                        break;
                    case 1:
                        PlayerController.instance.StopAllActions();
                        DialogueManager.instance.LoadDialogue(dialoguePaths[1]);
                        DialogueManager.instance.printLine(DialogueManager.instance.text);
                        if (DialogueManager.instance.dialogueEnded)
                            QuestManager.instance.davidDialogueIndex = 2;
                        SaveLoad.instance.saveData.player.davidDialogueIndex = 2;
                        SaveLoad.instance.Save();
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
