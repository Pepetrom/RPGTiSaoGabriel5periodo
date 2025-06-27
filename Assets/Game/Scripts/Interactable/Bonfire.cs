using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bonfire : Interactable
{
    public Button myLocation;
    public string myName;
    public Text text;
    public int fontSize;
    public Transform myPosition;
    public Animator valveAnim;
    public GameObject smoke;
    public override void Interact()
    {
        FMODAudioManager.instance.PlayOneShot(FMODAudioManager.instance.bonfireInteract, transform.position);
        GameManager.instance.lastBonfireRestedAt = myPosition;
        UIItems.instance.AddLocation(myLocation);
        text.text = myName;
        text.fontSize = fontSize;
        GameManager.instance.Bonfire(!GameManager.instance.bonfire.activeSelf);
        Invoke("DeactivateSmoke", 3f);
        ActivateValve();
        //PlayerController.instance.audioMan.PlayAudio(6);
    }
    void ActivateValve()
    {
        valveAnim.SetTrigger("Activate");
        smoke.SetActive(true);
    }
    void DeactivateSmoke()
    {
        valveAnim.SetTrigger("Return");
        smoke.SetActive(false);
    }
}
