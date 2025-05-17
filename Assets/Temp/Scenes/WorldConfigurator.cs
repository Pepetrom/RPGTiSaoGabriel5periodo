using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldConfigurator : MonoBehaviour
{
    public static WorldConfigurator Instance { get; private set; }
    public Material transparentMaterial;
    public float ObstructionFadingSpeed = 1.0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

