using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    private bool isNearBonfire = false;
    private bool isNearItem = false;
    private bool isNearNote = false;
    private bool isNearLever = false;
    private bool isNearValve = false;
    private GameObject coll;
    private Interactable collEnter, collExit;
    public GameObject pressF;

    public static PlayerInteract instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        pressF.SetActive(false);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            if (collEnter) collEnter.Exit();
            collEnter = collision.GetComponent<Interactable>();
            collEnter.Enter();
            pressF.SetActive(!!collEnter);
            Debug.Log("entrou em "+ collision.name);
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (!collEnter) return;
        if (collision.gameObject.CompareTag("Interactable"))
        {
            collExit = collision.GetComponent<Interactable>();
            if (!collExit || collEnter == collExit)
            {
                collEnter = null;
            }
            collExit.Exit();
            collExit = null;
            pressF.SetActive(!!collEnter);
            Debug.Log("saiu de " + collision.name);
        }
    }
    public void UpdatePlayerInteract()
    {

        //CheckDistance();
        if (collEnter && collEnter.IsInRange())
        {
            collEnter.Interact();
        }
    }
    public void FixedUpdatePlayerInteract()
    {
        //CheckDistance();
    }

    void CheckDistance()
    {
        if(!collEnter || Vector3.Distance(collEnter.transform.position, PlayerController.instance.transform.position) > 10)
        {
            collEnter?.Exit();
            collEnter = null;
            pressF.SetActive(!!collEnter);
        }
    }

    public void ObjectAutoDestruction(Interactable obj)
    {
        if (obj == collEnter)
        {
            collEnter.Exit();
            collEnter = null;
            pressF.SetActive(!!collEnter);
        }
    }
}
