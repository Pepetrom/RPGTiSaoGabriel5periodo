using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotaoContinuar : MonoBehaviour
{
    void Start()
    {
        SaveLoad.instance.ShowContinuar(this.gameObject);
    }
}
