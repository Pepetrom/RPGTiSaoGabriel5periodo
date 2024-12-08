using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private bool isNearBonfire = false;
    private bool isNearItem = false;
    private bool isNearNote = false;
    private bool isNearLever = false;
    private GameObject coll;
    public GameObject pressF;
    private void Start()
    {
        pressF.SetActive(false);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Bonfire"))
        {
            isNearBonfire = true;
            GameManager.instance.lastBonfireRestedAt.position = transform.position;
            pressF.SetActive(true);
        }
        if (collision.gameObject.CompareTag("basicItem"))
        {
            isNearItem = true;
            coll = collision.gameObject;
            pressF.SetActive(true);
        }
        if (collision.gameObject.CompareTag("Notes"))
        {
            isNearNote = true;
            pressF.SetActive(true);
        }
        if (collision.gameObject.CompareTag("Lever"))
        {
            isNearLever = true;
            coll = collision.gameObject;
            pressF.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Bonfire"))
        {
            isNearBonfire = false;
            GameManager.instance.Bonfire(false);
            pressF.SetActive(false);
        }
        if (collision.gameObject.CompareTag("basicItem"))
        {
            pressF.SetActive(false);
            isNearItem = false;
            coll = null;
        }
        if (collision.gameObject.CompareTag("Notes"))
        {
            isNearNote = false;
            pressF.SetActive(false);
        }
        if (collision.gameObject.CompareTag("Lever"))
        {
            isNearLever = false;
            coll = null;
            pressF.SetActive(false);
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isNearBonfire )
            {
                GameManager.instance.Bonfire(!GameManager.instance.bonfire.activeSelf);
                PlayerController.instance.audioMan.PlayAudio(6);
            }
            if (isNearItem )
            {
                GameManager.instance.Score(50);
                Destroy(coll);
                coll = null;
                isNearItem = false;
                pressF.SetActive(false);
                PlayerController.instance.audioMan.PlayAudio(5);
            }
            if (isNearNote )
            {
                UIItems.instance.ShowNotes();
                PlayerController.instance.audioMan.PlayAudio(5);
            }
            if (isNearLever)
            {
                coll.GetComponent<WaterLevelLever>().Activate();
            }
        }
        
    }
}
