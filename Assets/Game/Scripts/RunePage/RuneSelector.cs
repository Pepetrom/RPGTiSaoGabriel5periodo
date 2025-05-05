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
    [SerializeField] Button[] buttonsButtons;
    [SerializeField] int totalUpgrades;
    int runeRowSize;
    int liberados1, liberados2, liberados3;
    int upperlimit, lowerlimit, selected;
    int tier = 0;
    public void OpenSkills()
    {
        runes.SetActive(!runes.activeSelf);
        skills.SetActive(!skills.activeSelf);
    }
    private void Start()
    {
        runeRowSize = runesBanner.Length / 3;
    }
    public void UpdateRuneSelector()
    {
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
        sliders[0].value = PlayerController.instance.strength / totalUpgrades;
        sliders[1].value = PlayerController.instance.agility / totalUpgrades;
        sliders[2].value = PlayerController.instance.resistance / totalUpgrades;
    }
    public void PurchaseRune(int which)
    {
        if (runePurchased[which]) return;
        if (GameManager.instance.skillPoints < runeValue) return;
        GameManager.instance.skillPoints -= runeValue;

        runePurchased[which] = true;
        if(buttonsButtons[which]) buttonsButtons[which].interactable = false;

        Debug.Log(runeRowSize);
        EquipRune();

        liberados1 = 0;
        liberados2 = 0;
        liberados3 = 0;
        for (int i = 0; i < runeRowSize; i++)
        {
            if(runePurchased[i]) liberados1++; 
        }
        for (int i = runeRowSize; i < runeRowSize*2; i++)
        {
            if (runePurchased[i]) liberados2++;
        }
        for (int i = runeRowSize * 2; i < runeRowSize * 3; i++)
        {
            if (runePurchased[i]) liberados3++;
        }
        UpdateRuneSelector();
    }
    void EquipRune()
    {
        foreach (var rune in runesBanner)
        {
            rune.SetActive(false);
        }
        runesBanner[PlayerController.instance.equipedPrimaryRune].SetActive(true);
        runesBanner[PlayerController.instance.equipedSecondaryRune + runeRowSize].SetActive(true);
        runesBanner[PlayerController.instance.equipedTerciaryRune + (runeRowSize*2)].SetActive(true);
    }
    public void SelectPrimaryRune(int which)
    {
        upperlimit = runeRowSize -1;
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
        PlayerController.instance.equipedPrimaryRune = selected % runeRowSize;
        EquipRune();
    }
    public void SelectSecondaryRune(int which)
    {
        upperlimit = (runeRowSize * 2) - 1;
        lowerlimit = runeRowSize ;
        selected = PlayerController.instance.equipedSecondaryRune + runeRowSize + which;
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
        PlayerController.instance.equipedSecondaryRune = selected % runeRowSize;
        EquipRune();
    }
    public void SelectTerciaryRune(int which)
    {
        upperlimit = (runeRowSize * 3) - 1;
        lowerlimit = runeRowSize*2;
        selected = PlayerController.instance.equipedTerciaryRune + (runeRowSize * 2) + which;

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
        PlayerController.instance.equipedTerciaryRune = selected % runeRowSize;
        EquipRune();
    }
}
