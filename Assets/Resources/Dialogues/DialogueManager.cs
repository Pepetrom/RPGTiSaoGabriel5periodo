using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
public class DialogueManager : MonoBehaviour
{
    public Text text;
    public bool dialogueStatus = true;
    private JsonData dialogue;
    private int i;
    private string speaker;
    public static DialogueManager instance;
    bool inDialogue;
    public bool dialogueEnded;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        UIItems.instance.dialoguePanel.SetActive(false);
    }
    public bool LoadDialogue(string path)
    {
        if (!inDialogue)
        {
            i = 0;
            dialogueEnded = false;
            UIItems.instance.dialoguePanel.SetActive(true);
            var jsonTextFile = Resources.Load<TextAsset>(path);
            if (jsonTextFile == null) Debug.LogError("Arquivo JSON n�o encontrado!");
            else Debug.Log("JSON carregado com sucesso!");
            dialogue = JsonMapper.ToObject(jsonTextFile.text);
            inDialogue = true;
            return true;
        }
        return false;
    }
    public bool printLine(Text text)
    {
        if (inDialogue)
        {
            JsonData line = dialogue[i];

            if (line[0].ToString() == "EOD")
            {
                dialogueEnded = true;
                dialogueStatus = false;
                inDialogue = false;
                text.text = "";
                UIItems.instance.dialoguePanel.SetActive(false);
                PlayerController.instance.ResetAllActions();
                return false;
            }
            foreach (JsonData key in line.Keys)
                speaker = key.ToString();
            StopAllCoroutines();
            StartCoroutine(WriteText(line[0].ToString()));
            i++;
        }
        return true;
    }
    IEnumerator WriteText(string fullTex)
    {
        text.text = "";
        foreach(char c in fullTex)
        {
            text.text += c;
            yield return new WaitForSeconds(0.02f);
        }
    }
}
