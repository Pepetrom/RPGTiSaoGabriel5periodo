using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
{
    public Image buttonImage; // Imagem de fundo do botão
    public TextMeshProUGUI buttonText; // Texto do botão

    [Header("Colors")]
    public Color normalButtonColor = Color.gray;  // Cor padrão do botão
    public Color hoverButtonColor = Color.white;  // Cor quando o mouse passa
    public Color pressedButtonColor = Color.gray; // Cor quando pressionado
    
    public Color normalTextColor = Color.white;  // Cor padrão do texto
    public Color hoverTextColor = Color.black;  // Cor do texto ao passar o mouse
    public Color pressedTextColor = Color.white; // Cor do texto ao pressionar
    public Color selectedTextColor = Color.white; // Cor do texto quando selecionado

    private bool isSelected = false; 
    void Start()
    {
        buttonImage.color = normalButtonColor;
        buttonText.color = normalTextColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.color = hoverButtonColor;
        buttonText.color = hoverTextColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.color = normalButtonColor;
        buttonText.color = normalTextColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonImage.color = normalButtonColor;
        buttonText.color = normalTextColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonImage.color = normalButtonColor;
        buttonText.color = normalTextColor;
    }

    public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;
        
        buttonText.color = selectedTextColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
        buttonImage.color = normalButtonColor;
        buttonText.color = normalTextColor;
    }
}
