using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RunePage : MonoBehaviour
{
    public static RunePage instance;
    public GameObject[] RuneBorders;
    public Image[] RuneImages;
    public Image[] Lines;
    public Button[] RuneButtons;
    public Rune[] Runes;
    public bool[] purchased;
    public int[] specialRunes;
    //RuneMove
    private RectTransform panelRectTransform;
    Vector2 lastMousePosition;
    Vector2 newRunePagePosition;
    public float moveSpeed;
    public RectTransform canvas;
    float limitX, limitY;
    public AudioManager audioMan;
    void Start()
    {
        instance = this;
        purchased = new bool[Runes.Length];
        StartRunePage();
        panelRectTransform = GetComponent<RectTransform>();
        limitX = (canvas.rect.width / panelRectTransform.rect.width) * (canvas.rect.width);
        limitY = (canvas.rect.height / panelRectTransform.rect.height) * (canvas.rect.height);
        moveSpeed = moveSpeed * ((canvas.rect.width * canvas.rect.height) / (1920 * 1080));
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition.x = Input.mousePosition.x;
            lastMousePosition.y = Input.mousePosition.y;
        }
        if (Input.GetMouseButton(0))
        {
            newRunePagePosition.x = (Input.mousePosition.x - lastMousePosition.x) * Time.deltaTime * moveSpeed;
            newRunePagePosition.y = (Input.mousePosition.y - lastMousePosition.y) * Time.deltaTime * moveSpeed;
            newRunePagePosition= panelRectTransform.anchoredPosition + newRunePagePosition;

            newRunePagePosition.x = Mathf.Clamp(newRunePagePosition.x, -limitX,limitX);
            newRunePagePosition.y = Mathf.Clamp(newRunePagePosition.y, -limitY, limitY);
            
            panelRectTransform.anchoredPosition = newRunePagePosition;
        }

    }
    public void PurchaseSkill(int which)
    {
        if (GameManager.instance.skillPoints <= 300 || purchased[which])
        {
            audioMan.PlayAudio(1);
            return;
        }
        audioMan.PlayAudio(0);
        GameManager.instance.skillPoints -= 300;
        purchased[which] = true;
        if (Runes[which].nextRunes.Length > 0)
        {
            for(int i = 0; i < Runes[which].nextRunes.Length;i++)
            {
                RuneButtons[Runes[which].nextRunes[i]].interactable = true;
            }
        }
        if (Runes[which].nextLines.Length > 0)
        {
            for (int j = 0; j < Runes[which].nextLines.Length; j++)
            {
                Lines[Runes[which].nextLines[j]].color = Color.white;
            }
        }
        RuneEffect(Runes[which].effect);
        RuneBorders[which].SetActive(true);
        RuneImages[which].color = Color.white;
        UIItems.instance.UpdateSkillPoints();
    }
    public void RuneEffect(int which)
    {
        switch (which)
        {
            case 0:
                break;
            case 1:
                PlayerController.instance.AddAtribute("resistance");
                break;
            case 2:
                PlayerController.instance.AddAtribute("inteligence");
                break;
            case 3:
                PlayerController.instance.AddAtribute("agility");
                break;
            case 4:
                UnlockRune(1);
                break;
            case 5:
                UnlockRune(2);
                break;
            case 6:
                UnlockRune(3);
                break;
        }
    }
    public void UnlockRune(int which)
    {
        audioMan.PlayAudio(2);
        GameManager.instance.unlockedRunes[which] = true;
    } 
    public void StartRunePage()
    {
        for (int i = 1;i< RuneBorders.Length; i++)
        {
            RuneBorders[i].SetActive(false);
            RuneImages[i].color = Color.red;
            RuneButtons[i].interactable = false;
            Lines[i].color = Color.red;
        }
        RuneButtons[0].interactable = true;
        Lines[0].color = Color.white;
        RuneButtons[4].interactable = true;
        Lines[4].color = Color.white;
        RuneButtons[8].interactable = true;
        Lines[8].color = Color.white;
    }
}
