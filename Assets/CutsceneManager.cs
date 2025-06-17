using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager instance;
    public GameObject hoff, player, canvas;
    public Camera[] cameras;
    public Animator croc, porquin;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        hoff.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hoff.SetActive(true);
            player.SetActive(false);
            canvas.SetActive(false);
            ChangeCamera(0);
        }
    }
    public void ChangeCamera(int index)
    {
        switch (index)
        {
            case 0:
                cameras[0].gameObject.SetActive(false);
                cameras[1].gameObject.SetActive(true);
                break;
            case 1:
                cameras[1].gameObject.SetActive(false);
                cameras[2].gameObject.SetActive(true);
                break;
            case 2:
                cameras[2].gameObject.SetActive(false);
                cameras[3].gameObject.SetActive(true);
                break;
            case 3:
                cameras[3].gameObject.SetActive(false);
                cameras[4].gameObject.SetActive(true);
                break;
        }
    }
    public void StartCrocAnim()
    {
        croc.SetTrigger("croc");
    }
    public void StartPorquin()
    {
        porquin.SetTrigger("porquin");
    }
}
