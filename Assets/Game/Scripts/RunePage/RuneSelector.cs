using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.Unicode;

public class RuneSelector : MonoBehaviour
{
    [SerializeField] Image[] images;
    [SerializeField] GameObject[] rune1, rune2, rune3;
    [SerializeField] GameObject[] buttons;
    [SerializeField] bool[] runePurchased;
    [SerializeField] int runeValue, liberados;
    [SerializeField] Button[] buttonsButtons;
    public void UpdateRuneSelector()
    {
        if (liberados > 3)
        {
            buttons[0].SetActive(true);
            buttons[1].SetActive(true);
        }
        if (liberados > 4)
        {
            buttons[2].SetActive(true);
            buttons[3].SetActive(true);
        }
        if (liberados > 5)
        {
            buttons[4].SetActive(true);
            buttons[5].SetActive(true);
        }
    }
    public void PurchaseRune(int which)
    {
        if (runePurchased[which]) return;
        if (GameManager.instance.skillPoints < runeValue) return;
        GameManager.instance.skillPoints -= runeValue;
        runePurchased[which] = true;

        buttonsButtons[which].interactable = false;

        liberados++;
        UpdateRuneSelector();
    }
    public void SelectPrimaryRune(int which)
    {
        int temp = PlayerController.instance.equipedPrimaryRune + which;
        if (temp < 0) temp = rune1.Length - 1;
        else temp = temp % rune1.Length;
        if (!GameManager.instance.unlockedRunes[temp])
        {
            for (int i = temp; i < rune1.Length && i >= 0; i += which)
            {
                if (i == rune1.Length)
                {
                    temp = PlayerController.instance.equipedPrimaryRune;
                    break;
                }
                if (GameManager.instance.unlockedRunes[i])
                {
                    temp = i;
                    break;
                }
            }
        }
        PlayerController.instance.equipedPrimaryRune = temp;
        rune1[0].SetActive(false);
        rune1[1].SetActive(false);
        rune1[2].SetActive(false);
        rune1[3].SetActive(false);
        rune1[temp].SetActive(true);
        //images[0].sprite = rune1[temp];
    }
    public void SelectSecondaryRune(int which)
    {
        int temp = PlayerController.instance.equipedSecondaryRune + which;
        if (temp < 0) temp = rune2.Length - 1;
        else temp = temp % rune2.Length;
        if (!GameManager.instance.unlockedRunes[temp])
        {
            for (int i = temp; i < rune2.Length && i >= 0; i += which)
            {
                if (i == rune2.Length)
                {
                    temp = PlayerController.instance.equipedSecondaryRune;
                    break;
                }
                if (GameManager.instance.unlockedRunes[i])
                {
                    temp = i;
                    break;
                }
            }
        }
        PlayerController.instance.equipedSecondaryRune = temp;
        rune2[0].SetActive(false);
        rune2[1].SetActive(false);
        rune2[2].SetActive(false);
        rune2[3].SetActive(false);
        rune2[temp].SetActive(true);
        //images[1].sprite = rune2[temp];
    }
    public void SelectTerciaryRune(int which)
    {
        int temp = PlayerController.instance.equipedTerciaryRune + which;
        if (temp < 0) temp = rune3.Length - 1;
        else temp = temp % rune3.Length;
        if (!GameManager.instance.unlockedRunes[temp])
        {
            for (int i = temp; i < rune3.Length && i >= 0; i += which)
            {
                if (i == rune3.Length)
                {
                    temp = PlayerController.instance.equipedTerciaryRune;
                    break;
                }
                if (GameManager.instance.unlockedRunes[i])
                {
                    temp = i;
                    break;
                }
            }
        }
        PlayerController.instance.equipedTerciaryRune = temp;
        rune3[0].SetActive(false);
        rune3[1].SetActive(false);
        rune3[2].SetActive(false);
        rune3[3].SetActive(false);
        rune3[temp].SetActive(true);
        //images[2].sprite = rune3[temp];
    }
}
