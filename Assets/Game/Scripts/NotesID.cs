using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotesID : MonoBehaviour
{
    public Sprite note;
    bool isNear;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIItems.instance.ActivatePressF();
            isNear = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIItems.instance.DeactivatePressF();
            isNear = false;
        }
    }
    private void Update()
    {
        if (isNear)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                UIItems.instance.ShowNotes(note);
            }
        }
    }
}
