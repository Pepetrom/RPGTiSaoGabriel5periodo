using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItems : MonoBehaviour
{
    public static UIItems instance;
    public TextMeshProUGUI runePageText;
    public Text cheeseQ;
    public GameObject[] skillButtons;
    void Awake()
    {
        instance = this;
    }
    public void UpdateChesseQUI(int value)
    {
        cheeseQ.text = value.ToString();
    }
    public void UpdateSkillPoints()
    {
        runePageText.text = GameManager.instance.skillPoints.ToString();
    }
    public void UnlockSkill(int which)
    {
        skillButtons[which].SetActive(true);
    }
}
