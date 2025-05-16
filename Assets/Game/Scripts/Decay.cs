using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decay : MonoBehaviour
{
    [SerializeField] float time = 3;
    private void Start()
    {
        Invoke("DecayFun",time);
    }
    void DecayFun()
    {
        Destroy(gameObject);
    }
}
