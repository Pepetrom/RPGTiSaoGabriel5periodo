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
                        dialogueIndex = 1;
                        break;
                    case 1:
                        PlayerController.instance.StopAllActions();
                        DialogueManager.instance.LoadDialogue(dialoguePaths[1]);
                        DialogueManager.instance.printLine(DialogueManager.instance.text);
                        // Tocar o solo de guitarra
                        // Quando o solo terminar o index muda para 3
                        break;
                    case 2:
                        PlayerController.instance.StopAllActions();
                        DialogueManager.instance.LoadDialogue(dialoguePaths[2]);
                        DialogueManager.instance.printLine(DialogueManager.instance.text);
                        // Desabilita o colisor e quando o player fizer a quest ele libera o diálogo
                        break;

                }
            }
        }
    }
}
