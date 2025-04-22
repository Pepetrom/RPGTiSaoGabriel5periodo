using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItems : MonoBehaviour
{
    public static UIItems instance;
    public Text runePageText;
    public Text cheeseQ;
    public Text score;
    public GameObject[] skillButtons;
    public Canvas notes;
    public Image noteImage;
    public GameObject pressF;
    private List<Button> locations = new List<Button>();
    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        pressF.SetActive(false);
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
    public void UpdateScoreQUI(float value)
    {
        score.text = value.ToString();
    }
    public void ShowNotes(Sprite note)
    {
        noteImage.sprite = note;
        notes.gameObject.SetActive(true);
    }
    public void ActivatePressF()
    {
        pressF.SetActive(true);
    }
    public void DeactivatePressF()
    {
        pressF.SetActive(false);
    }
    public void AddLocation(Button location)
    {
        if (!locations.Contains(location))
        {
            locations.Add(location);
            location.gameObject.SetActive(true);
        }
    }
}
