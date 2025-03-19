using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public Valve valve;
    private void OnDisable()
    {
        valve.leverActivated = true;
        //valve.lever.SetActive(true);
    }
}
