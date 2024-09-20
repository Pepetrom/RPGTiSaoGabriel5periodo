using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleHands : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Colidiu");
        }
    }
}
