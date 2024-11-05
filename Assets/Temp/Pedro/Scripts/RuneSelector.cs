using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.Unicode;

public class RuneSelector : MonoBehaviour
{
    [SerializeField] Image[] images;
    [SerializeField] Sprite[] rune1, rune2, rune3;
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
        images[0].sprite = rune1[temp];
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
        images[1].sprite = rune2[temp];
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
        images[2].sprite = rune3[temp];
    }
}
