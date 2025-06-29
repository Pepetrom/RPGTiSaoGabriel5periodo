using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndEisenfaust : MonoBehaviour
{
    public GameObject end;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            end.SetActive(true);
        }
    }
}
