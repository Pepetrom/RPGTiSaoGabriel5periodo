using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RuneSelector : MonoBehaviour
{
    [SerializeField] Image[] images;
    [SerializeField] Sprite[] rune1, rune2, rune3;
    public void SelectPrimaryRune(int which)
    {
        int temp = PlayerController.instance.equipedPrimaryRune;
        temp = temp + which;
        if (temp > rune1.Length - 1) temp = 0;
        if (temp < 0) temp = rune1.Length - 1;

        if (!GameManager.instance.unlockedRunes[temp])
        {
            if (which > 0)
            {
                for (int i = temp; i < rune1.Length; i++)
                {
                    if (GameManager.instance.unlockedRunes[i])
                    {
                        temp = i;
                        break;
                    }
                }
            }
            else
            {
                for (int i = temp; i >= 0; i--)
                {
                    if (GameManager.instance.unlockedRunes[i])
                    {
                        temp = i;
                        break;
                    }
                }
            }
        }
        if (temp == PlayerController.instance.equipedPrimaryRune) temp = 0;
        if (GameManager.instance.unlockedRunes[temp])
        {
        PlayerController.instance.equipedPrimaryRune = temp;
        images[0].sprite = rune1[temp];
        }
    }
    public void SelectSecondaryRune(int which)
    {
        int temp = PlayerController.instance.equipedSecondaryRune;
        temp = temp + which;
        if (temp > rune2.Length - 1) temp = 0;
        if (temp < 0) temp = rune2.Length - 1;
        if (!GameManager.instance.unlockedRunes[temp])
        {
            if (which > 0)
            {
                for (int i = temp; i < rune2.Length; i++)
                {
                    if (GameManager.instance.unlockedRunes[i])
                    {
                        temp = i;
                        break;
                    }
                }
            }
            else
            {
                for (int i = temp; i >= 0; i--)
                {
                    if (GameManager.instance.unlockedRunes[i])
                    {
                        temp = i;
                        break;
                    }
                }
            }
        }
        if (temp == PlayerController.instance.equipedSecondaryRune) temp = 0;
        if (GameManager.instance.unlockedRunes[temp])
        {
            PlayerController.instance.equipedSecondaryRune = temp;
            images[1].sprite = rune2[temp];
        }
    }
    public void SelectTerciaryRune(int which)
    {
        int temp = PlayerController.instance.equipedTerciaryRune;
        temp = temp + which;
        if (temp > rune3.Length - 1) temp = 0;
        if (temp < 0) temp = rune3.Length - 1;
        if (!GameManager.instance.unlockedRunes[temp])
        {
            if (which > 0)
            {
                for (int i = temp; i < rune3.Length; i++)
                {
                    if (GameManager.instance.unlockedRunes[i])
                    {
                        temp = i;
                        break;
                    }
                }
            }
            else
            {
                for (int i = temp; i >= 0; i--)
                {
                    if (GameManager.instance.unlockedRunes[i])
                    {
                        temp = i;
                        break;
                    }
                }
            }
        }
        if (temp == PlayerController.instance.equipedTerciaryRune) temp = 0;
        if (GameManager.instance.unlockedRunes[temp])
        {
            PlayerController.instance.equipedTerciaryRune = temp;
            images[2].sprite = rune3[temp];
        }
    }
}
