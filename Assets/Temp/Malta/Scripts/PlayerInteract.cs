using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private bool isNearBonfire = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bonfire"))
        {
            isNearBonfire = true;
            Debug.Log("To na fogueira");
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bonfire"))
        {
            isNearBonfire = false;
            Debug.Log("Sai da fogueira");
            GameManager.instance.Bonfire(false);
        }
    }

    private void Update()
    {
        if (isNearBonfire && Input.GetKeyDown(KeyCode.F))
        {
            GameManager.instance.Bonfire(!GameManager.instance.bonfire.activeSelf);
        }
    }
}
