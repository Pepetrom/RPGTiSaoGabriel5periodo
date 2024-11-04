using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Estus : MonoBehaviour
{
    public static Estus estus;
    public int healQuantity,flaskQuantity, maxFlaskQuantity; // quantity VAI RESUMIR NOME DE VARIAVEL NAO

    private void Awake()
    {
        estus = this;
    }
    private void Start()
    {
        flaskQuantity = maxFlaskQuantity;
        UIItems.instance.UpdateChesseQUI(flaskQuantity);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && flaskQuantity > 0 )
        {
            HPBar.instance.RecoverHPbyItem(healQuantity);
            flaskQuantity--;
            UIItems.instance.UpdateChesseQUI(flaskQuantity);
        }
    }
    
}
