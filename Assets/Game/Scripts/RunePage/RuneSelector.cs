using System;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.UI;

public class RuneSelector : MonoBehaviour
{
    [SerializeField] GameObject runes, skills;
    [SerializeField] GameObject[] runesBanner;
    [SerializeField] GameObject[] buttons;
    [SerializeField] Slider[] sliders;
    [SerializeField] bool[] runePurchased;
    [SerializeField] int runeValue;
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
        atributesPerSkill = runePurchased.Length / 3;
        foreach (var t in priceTexts)
        {
            t.text = $"{runeValue}";
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
        if (GameManager.instance.skillPoints < runeValue) return;
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
        GameManager.instance.Score(-runeValue);
        runeValue += 50;
        runePurchased[whichMudavel] = true;
        foreach ( var t in priceTexts)
        {
            t.text = $"{runeValue}";
        }

        EquipRune();
        UpdateRuneSelector();
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
           for( int i = selected; i <= upperlimit && i >= lowerlimit; i += which)
            {
                if (runePurchased[i])
                {
                    selected = i;
                    break;
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
                if (runePurchased[i])
                {
                    selected = i;
                    break;
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
                if (runePurchased[i])
                {
                    selected = i;
                    break;
                }
            }
        }
        PlayerController.instance.equipedTerciaryRune = selected % atributesPerSkill;
        EquipRune();
    }
}
