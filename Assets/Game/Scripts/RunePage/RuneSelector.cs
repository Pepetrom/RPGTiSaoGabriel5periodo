using System;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class RuneSelector : MonoBehaviour
{
    [SerializeField] GameObject runes, skills;
    [SerializeField] GameObject[] runesBanner;
    [SerializeField] GameObject[] buttons;
    [SerializeField] Slider[] sliders;
    [SerializeField] bool[] runePurchased;
    [SerializeField] int[] runeValue;
    [SerializeField] int[] runeValuePerLevel;
    [SerializeField] TextMeshProUGUI[] priceTexts;
    float totalUpgrades = 3;
    int atributesPerSkill;
    int liberados1, liberados2, liberados3;
    int upperlimit, lowerlimit, selected;
    public void OpenSkills()
    {
        runes.SetActive(!runes.activeSelf);
        skills.SetActive(!skills.activeSelf);
    }
    private void Start()
    {
        PlayerController.instance.equipedPrimaryRune = SaveLoad.instance.saveData.player.primaryRune;
        PlayerController.instance.equipedSecondaryRune = SaveLoad.instance.saveData.player.secondaryRune;
        PlayerController.instance.equipedTerciaryRune = SaveLoad.instance.saveData.player.TerciaryRune;
        PlayerController.instance.strength =SaveLoad.instance.saveData.player.strenght;
        PlayerController.instance.agility = SaveLoad.instance.saveData.player.agility; 
        PlayerController.instance.constitution = SaveLoad.instance.saveData.player.constitution;
        if (SaveLoad.instance.saveData.player.runePurchased.Length > 0) runePurchased = SaveLoad.instance.saveData.player.runePurchased ;
        if (SaveLoad.instance.saveData.player.runeValue.Length > 0) runeValue = SaveLoad.instance.saveData.player.runeValue;
      
        atributesPerSkill = runePurchased.Length / 3;
        for(int i = 0; i < priceTexts.Length; i++)
        {
            if(runeValue[i] == 0) runeValue[i] = runeValuePerLevel[0];
            priceTexts[i].text = $"{runeValue[i]}";
        }
        UpdateRuneSelector();
        UpdatePrice();
        EquipRune();
    }
    void UpdatePrice()
    {
        runeValue[0] = runeValuePerLevel[PlayerController.instance.strength];
        runeValue[1] = runeValuePerLevel[PlayerController.instance.agility];
        runeValue[2] = runeValuePerLevel[PlayerController.instance.constitution];
        for (int i = 0; i < priceTexts.Length; i++)
        {
            priceTexts[i].text = $"{runeValue[i]}";
        }
        if (PlayerController.instance.strength >= totalUpgrades)
        {
            priceTexts[0].text = "MAX";
        }
        if (PlayerController.instance.agility >= totalUpgrades)
        {
            priceTexts[1].text = "MAX";
        }
        if (PlayerController.instance.constitution >= totalUpgrades)
        {
            priceTexts[2].text = "MAX";
        }

    }
    void UpdateRuneSelector()
    {
        liberados1 = 0;
        liberados2 = 0;
        liberados3 = 0;
        for (int i = 0; i < atributesPerSkill; i++)
        {
            if (runePurchased[i]) liberados1++;
        }
        for (int i = atributesPerSkill; i < atributesPerSkill * 2; i++)
        {
            if (runePurchased[i]) liberados2++;
        }
        for (int i = atributesPerSkill * 2; i < atributesPerSkill * 3; i++)
        {
            if (runePurchased[i]) liberados3++;
        }

        if (liberados1 > 1)
        {
            buttons[0].SetActive(true);
            buttons[1].SetActive(true);
        }
        if (liberados2 > 1)
        {
            buttons[2].SetActive(true);
            buttons[3].SetActive(true);
        }
        if (liberados3 > 1)
        {
            buttons[4].SetActive(true);
            buttons[5].SetActive(true);
        }
        sliders[0].value = (float)PlayerController.instance.strength / totalUpgrades;
        sliders[1].value = (float)PlayerController.instance.agility / totalUpgrades;
        sliders[2].value = (float)PlayerController.instance.constitution / totalUpgrades;
    }
    int whichMudavel = 0;
    public void PurchaseRune(int which)
    {
        if (GameManager.instance.skillPoints < runeValue[which - 1]) return;
        whichMudavel = 0;
        switch (which)
        {
            case 1:
                if (PlayerController.instance.strength >= totalUpgrades) return;
                whichMudavel = (atributesPerSkill * PlayerController.instance.strength) + which;
                PlayerController.instance.AddAtribute(Atribute.strength);
                break;
            case 2:
                if (PlayerController.instance.agility >= totalUpgrades) return;
                whichMudavel = (atributesPerSkill * PlayerController.instance.agility) + which;
                PlayerController.instance.AddAtribute(Atribute.agility);
                break;
            case 3:
                if (PlayerController.instance.constitution >= totalUpgrades) return;
                whichMudavel = (atributesPerSkill * PlayerController.instance.constitution) + which;
                PlayerController.instance.AddAtribute(Atribute.constitution);
                break;
        }
        Debug.Log("tentei comprar a runa: " + whichMudavel);
        if (whichMudavel > runePurchased.Length) return;
        if (runePurchased[whichMudavel]) return;
        GameManager.instance.Score(-runeValue[which - 1]);
        runePurchased[whichMudavel] = true;
        UpdateRuneSelector();
        UpdatePrice();
        EquipRune();
    }
    void EquipRune()
    {
        foreach (var rune in runesBanner)
        {
            rune.SetActive(false);
        }
        runesBanner[PlayerController.instance.equipedPrimaryRune].SetActive(true);
        runesBanner[PlayerController.instance.equipedSecondaryRune + atributesPerSkill].SetActive(true);
        runesBanner[PlayerController.instance.equipedTerciaryRune + (atributesPerSkill*2)].SetActive(true);
        SaveLoad.instance.saveData.player.primaryRune = PlayerController.instance.equipedPrimaryRune;
        SaveLoad.instance.saveData.player.secondaryRune = PlayerController.instance.equipedSecondaryRune;
        SaveLoad.instance.saveData.player.TerciaryRune = PlayerController.instance.equipedTerciaryRune;

        SaveLoad.instance.saveData.player.runePurchased = runePurchased;
        SaveLoad.instance.saveData.player.runeValue = runeValue;

        SaveLoad.instance.saveData.player.strenght = PlayerController.instance.strength;
        SaveLoad.instance.saveData.player.agility = PlayerController.instance.agility;
        SaveLoad.instance.saveData.player.constitution = PlayerController.instance.constitution;
        SaveLoad.instance.Save();
    }
    public void SelectPrimaryRune(int which)
    {
        upperlimit = atributesPerSkill -1;
        lowerlimit = 0;
        selected = PlayerController.instance.equipedPrimaryRune + which;
        if (selected < lowerlimit) selected = upperlimit;
        if (selected > upperlimit) selected = lowerlimit;
        if (!runePurchased[selected])
        {
            for (int i = selected; i <= upperlimit && i >= lowerlimit; i += which)
            {
                Debug.Log($"the rune is trying to equip is {i} and {(runePurchased[i] ? "you have it" : "you dont have it")}");
                if (runePurchased[i])
                {
                    selected = i;
                    break;
                }
                else
                {
                    if (i == upperlimit) selected = lowerlimit;
                }
            }
        }
        PlayerController.instance.equipedPrimaryRune = selected % atributesPerSkill;
        EquipRune();
    }
    public void SelectSecondaryRune(int which)
    {
        upperlimit = (atributesPerSkill * 2) - 1;
        lowerlimit = atributesPerSkill ;
        selected = PlayerController.instance.equipedSecondaryRune + atributesPerSkill + which;
        if (selected < lowerlimit) selected = upperlimit;
        if (selected > upperlimit) selected = lowerlimit;
        if (!runePurchased[selected])
        {
            for (int i = selected; i <= upperlimit && i >= lowerlimit; i += which)
            {
                Debug.Log($"the rune is trying to equip is {i} and {(runePurchased[i] ? "you have it" : "you dont have it")}");
                if (runePurchased[i])
                {
                    selected = i;
                    break;
                }
                else
                {
                    if (i == upperlimit) selected = lowerlimit;
                }
            }
        }
        PlayerController.instance.equipedSecondaryRune = selected % atributesPerSkill;
        EquipRune();
    }
    public void SelectTerciaryRune(int which)
    {
        upperlimit = (atributesPerSkill * 3) - 1;
        lowerlimit = atributesPerSkill*2;
        selected = PlayerController.instance.equipedTerciaryRune + (atributesPerSkill * 2) + which;

        if (selected < lowerlimit) selected = upperlimit;
        if (selected > upperlimit) selected = lowerlimit;
        if (!runePurchased[selected])
        {
            for (int i = selected; i <= upperlimit && i >= lowerlimit; i += which)
            {
                Debug.Log($"the rune is trying to equip is {i} and {(runePurchased[i] ? "you have it" : "you dont have it")}");
                if (runePurchased[i])
                {
                    selected = i;
                    break;
                }
                else
                {
                    if (i == upperlimit) selected = lowerlimit;
                }
            }
        }
        PlayerController.instance.equipedTerciaryRune = selected % atributesPerSkill;
        EquipRune();
    }
}
