using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JabutiDialogue : MonoBehaviour
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
                if (!QuestManager.instance.medicine)
                {
                    PlayerController.instance.StopAllActions();
                    DialogueManager.instance.LoadDialogue(dialoguePaths[0]);
                    DialogueManager.instance.printLine(DialogueManager.instance.text);
                    if (DialogueManager.instance.dialogueEnded)
                        QuestManager.instance.davidDialogueIndex = 3;
                }
                else
                {
                    PlayerController.instance.StopAllActions();
                    DialogueManager.instance.LoadDialogue(dialoguePaths[0]);
                    DialogueManager.instance.printLine(DialogueManager.instance.text);
                    UIItems.instance.ShowMedicine(false);
                    //QuestManager.instance.Poem();
                    if (DialogueManager.instance.dialogueEnded)
                        QuestManager.instance.davidDialogueIndex = 4;
                }
            }
        }
    }
}
