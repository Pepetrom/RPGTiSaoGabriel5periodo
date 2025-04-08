using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Estus : MonoBehaviour
{
    public static Estus instance;
    public int healQuantity,flaskQuantity, maxFlaskQuantity; // quantity VAI RESUMIR NOME DE VARIAVEL NAO

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        flaskQuantity = maxFlaskQuantity;
        UIItems.instance.UpdateChesseQUI(flaskQuantity);
    }
    public void UseEstus()
    {
        flaskQuantity--;
        HPBar.instance.RecoverHPbyItem(healQuantity);
        UIItems.instance.UpdateChesseQUI(flaskQuantity);
    }   
    public void ResetEstus()
    {
        flaskQuantity = maxFlaskQuantity;
        UIItems.instance.UpdateChesseQUI(flaskQuantity);
    }
}
