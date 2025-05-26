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
    public TextMeshProUGUI scoreInStore;
    public GameObject[] skillButtons;
    public Canvas notes;
    public Image noteImage;
    public GameObject pressF,DeathPanel, canTalk;
    public Button respawnButton;
    private List<Button> locations = new List<Button>();
    [HideInInspector] public bool deathAnimationIsOver, gearEnd;
    public Animator gearAnimator;
    public GameObject medicine, essence;
    [Header("Boss")]
    public GameObject bossHUD;
    public Slider hpBar, easeBar;
    public Text bossName;
    [HideInInspector]public int bossMaxHP, bossCurrentHP;
    public float lerpSpeed;
    public GameObject fadeInFadeOut;
    public GameObject dialoguePanel;
    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        DeathPanel.gameObject.SetActive(false);
        respawnButton.gameObject.SetActive(false);
        pressF.SetActive(false);
        UpdateScoreQUI(GameManager.instance.skillPoints);
    }
    private void FixedUpdate()
    {
        UpdateBossHPBar(hpBar,easeBar,bossCurrentHP,lerpSpeed);
    }
    public void UpdateChesseQUI(int value)
    {
        cheeseQ.text = $"{value}";
    }
    public void UpdateScoreQUI(float value)
    {
        score.text = $"{value}";
        scoreInStore.text = $"{value}";
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
        PlayerController.instance.StopAllActions();
        DeathPanel.gameObject.SetActive(true);
        Invoke("GarantiaRespawnButton", 3);
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
    void GarantiaRespawnButton()
    {
        if (DeathPanel.activeSelf) RespawnButton();
    }
    public void Respawn()
    {
        GameManager.instance.Rest();
        GameManager.instance.ResetPositionPlayer();
        PlayerController.instance.ForceIddle();
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
    public void ShowBOSSHUD(bool state)
    {
        bossHUD.SetActive(state);
    }
    public void ResetBossHP(int hp, string name)
    {
        bossMaxHP = hp;
        hpBar.maxValue = bossMaxHP;
        easeBar.maxValue = bossMaxHP;
        bossCurrentHP = bossMaxHP;
        hpBar.value = bossCurrentHP;
        easeBar.value = bossCurrentHP;
        bossName.text = name;
    }
    public void UpdateBossHPBar(Slider hpBar, Slider easeBar, int bossCurrentHP, float lerpSpeed)
    {
        if (hpBar.value != bossCurrentHP)
        {
            hpBar.value = Mathf.Lerp(hpBar.value, bossCurrentHP, lerpSpeed);
        }
        if (easeBar.value != hpBar.value)
        {
            easeBar.value = Mathf.Lerp(easeBar.value, hpBar.value, lerpSpeed / 10);
        }
    }
    public void FadeInFadeOut(bool state)
    {
        fadeInFadeOut.SetActive(state);
        if(!state)
            return;
        fadeInFadeOut.GetComponent<Animator>().Play("FadeInFadeOut");
    }
}
