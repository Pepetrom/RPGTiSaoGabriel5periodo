using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Canvas lockSprite;
    public bool dead = false;
    private void Start()
    {
        lockSprite.enabled = false;
    }
    public void ShowSprite(bool state)
    {
        lockSprite.enabled = state;
    }
}
