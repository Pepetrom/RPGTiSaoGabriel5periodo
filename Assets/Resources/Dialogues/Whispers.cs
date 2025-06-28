using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Whispers : MonoBehaviour
{
    public string[] dialoguePaths;
    public Text hoff;
    public int index;
    bool canTalk;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hoff.text = "HOFF";
            DialogueManager.instance.LoadDialogue(dialoguePaths[index]);
            DialogueManager.instance.printLine(DialogueManager.instance.text);
            canTalk = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIItems.instance.dialoguePanel.SetActive(false);
            Destroy(gameObject);
            canTalk = false;
        }
    }
    private void Update()
    {
        if (canTalk)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                DialogueManager.instance.LoadDialogue(dialoguePaths[0]);
                DialogueManager.instance.printLine(DialogueManager.instance.text);
            }
        }
    }
}
