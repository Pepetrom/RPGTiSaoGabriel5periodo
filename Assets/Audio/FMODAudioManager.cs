using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMODAudioManager : MonoBehaviour
{
    public static FMODAudioManager instance;
    public EventReference paper;
    private void Awake()
    {
        instance = this;
    }
    public void PlayOneShot(EventReference sound, Vector3 position)
    {
        RuntimeManager.PlayOneShot(sound, position);
    }
    public void PlayFootsteps(string path)
    {
        EventInstance footsteps = RuntimeManager.CreateInstance(path);
        Debug.Log("Ei");
        footsteps.start();
        footsteps.release();
    }
}