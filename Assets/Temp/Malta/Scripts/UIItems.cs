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
    public Text score;
    public GameObject[] skillButtons;
    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        UpdateScoreQUI(GameManager.instance.score);
    }
    public void UpdateChesseQUI(int value)
    {
        cheeseQ.text = value.ToString();
    }
    public void UpdateSkillPoints()
    {
        runePageText.text = GameManager.instance.skillPoints.ToString();
    }
    public void UpdateScoreQUI(int value)
    {
        score.text = value.ToString();
    }
}
