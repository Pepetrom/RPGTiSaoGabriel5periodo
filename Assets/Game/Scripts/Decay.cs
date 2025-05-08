using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decay : MonoBehaviour
{
    private void Start()
    {
        Invoke("DecayFun",3);
    }
    void DecayFun()
    {
        Destroy(gameObject);
    }
}
