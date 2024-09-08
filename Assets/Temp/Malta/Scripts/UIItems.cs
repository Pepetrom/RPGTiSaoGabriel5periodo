using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItems : MonoBehaviour
{
    public static UIItems UIItemsInstance;
    public Text cheeseQ;
    void Awake()
    {
        UIItemsInstance = this;
    }
    public void UpdateChesseQUI(int value)
    {
        cheeseQ.text = value.ToString();
    }

}
