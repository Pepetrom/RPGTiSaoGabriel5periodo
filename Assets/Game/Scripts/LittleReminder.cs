using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LittleReminder : MonoBehaviour
{
    public static LittleReminder instance;
    public TextMeshProUGUI text;
    public GameObject textObj;
    private void Awake()
    {
        instance = this;
    }
    public void littleReminder(string reminder)
    {
        textObj.SetActive(true);
        text.text = reminder;
        Invoke("Dismiss", 3);
    }
    void Dismiss()
    {
        textObj.SetActive(false);
        text.text = "Little Reminder";
    }
}
