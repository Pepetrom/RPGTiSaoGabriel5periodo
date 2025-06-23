using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager instance;
    public GameObject hoff, player, canvas;
    public Camera[] cameras;
    public Animator croc, porquin;
    public float shakeIntensity = 0.1f;
    public float shakeDuration = 1.0f;
    private float shakeTime = 0.0f;
    int cameraShakeIndex;
    public VisualEffect vfx;
    public GameObject volLight;
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
    private void Update()
    {
        Shake();
    }
    public void ChangeCamera(int index)
    {
        switch (index)
        {
            case 0:
                cameraShakeIndex = 1;
                cameras[0].gameObject.SetActive(false);
                cameras[1].gameObject.SetActive(true);
                break;
            case 1:
                cameraShakeIndex = 2;
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
    public void PlayVFX()
    {
        vfx.gameObject.SetActive(true);
        vfx.Play();
    }
    public void ShakeCamera()
    {
        StartShake();
        Debug.Log("Epa");
    }
    void Shake()
    {
        if (shakeTime > 0)
        {
            cameras[cameraShakeIndex].transform.position += Random.insideUnitSphere * shakeIntensity;
            shakeTime -= Time.deltaTime;
        }
    }
    public void StartShake()
    {
        shakeTime = shakeDuration;
    }
    public void VolLight()
    {
        volLight.SetActive(true);
    }
}
