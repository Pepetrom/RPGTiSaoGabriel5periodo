using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    public GameObject pressF,DeathPanel, canTalk;
    public Button respawnButton;
    private List<Button> locations = new List<Button>();
    [HideInInspector] public bool deathAnimationIsOver, gearEnd;
    public Animator gearAnimator;
    public GameObject medicine, essence;
    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        DeathPanel.gameObject.SetActive(false);
        respawnButton.gameObject.SetActive(false);
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
    public void CanTalk(bool state)
    {
        canTalk.SetActive(state);
    }
    public void AddLocation(Button location)
    {
        if (!locations.Contains(location))
        {
            locations.Add(location);
            location.gameObject.SetActive(true);
        }
    }
    public void PlayerIsDead()
    {
        DeathPanel.gameObject.SetActive(true);
    }
    public void GearEnd()
    {
        gearEnd = true;
    }
    public void DeathAnim()
    {
        deathAnimationIsOver = true;
        Debug.Log(deathAnimationIsOver);
    }
    public void RespawnButton()
    {
        respawnButton.gameObject.SetActive(true);
    }
    public void Respawn()
    {
        GameManager.instance.Rest();
        GameManager.instance.ResetPositionPlayer();
    }
    public void GearLoopAnimation(bool state)
    {
        gearAnimator.SetBool("isLooping", state);
    }
    public void ShowMedicine(bool state)
    {
        medicine.SetActive(state);
    }
    public void ShowEssence(bool state)
    {
        essence.SetActive(state);
    }
}
