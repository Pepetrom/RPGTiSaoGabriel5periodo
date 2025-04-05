using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCsDialogue : MonoBehaviour
{
    public string[] dialoguePaths;
    bool canTalk = false;
    int dialogueIndex = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canTalk = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canTalk = false;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (canTalk)
            {
                switch (dialogueIndex)
                {
                    case 0:
                        PlayerController.instance.StopAllActions();
                        DialogueManager.instance.LoadDialogue(dialoguePaths[0]);
                        DialogueManager.instance.printLine(DialogueManager.instance.text);
                        break;
                    case 1:
                        PlayerController.instance.StopAllActions();
                        DialogueManager.instance.LoadDialogue(dialoguePaths[1]);
                        DialogueManager.instance.printLine(DialogueManager.instance.text);
                        break;
                }
            }
        }
        if(canTalk && Input.GetKeyDown(KeyCode.F))
        {
            PlayerController.instance.StopAllActions();
            DialogueManager.instance.LoadDialogue(dialoguePath);
            DialogueManager.instance.printLine(DialogueManager.instance.text);
        }
    }
}
