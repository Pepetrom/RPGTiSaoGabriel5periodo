using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissingLever : MonoBehaviour
{
    public WaterLevelLever lever;
    private void OnDisable()
    {
        lever.completeLever = true;
        lever.lever.SetActive(true);
    }
}
