using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private bool isNearBonfire = false;
    public GameObject pressF;
    private void Start()
    {
        pressF.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bonfire"))
        {
            isNearBonfire = true;
            GameManager.instance.lastBonfireRestedAt.position = transform.position;
            pressF.SetActive(true);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bonfire"))
        {
            isNearBonfire = false;
            GameManager.instance.Bonfire(false);
            pressF.SetActive(false);
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
