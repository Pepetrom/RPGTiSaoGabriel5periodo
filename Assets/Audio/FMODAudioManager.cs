using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMODAudioManager : MonoBehaviour
{
    public static FMODAudioManager instance;
    [Header("Ambiente")]
    public EventReference bonfireInteract;
    public EventReference boxCrash;
    public EventReference notes;
    public EventReference lever;
    public EventReference valve;
    public EventReference fillingWater;

    [Header("player")]
    public EventReference[] sword;
    public EventReference estus;
    public EventReference dash;

    [Header("Porquinho")]
    public EventReference porquinDeath;
    public EventReference porquin;

    private void Awake()
    {
        instance = this;
    }
    public void PlayOneShot(EventReference sound, Vector3 position)
    {
        RuntimeManager.PlayOneShot(sound, position);
    }
    public void PlayOneShotAttached(EventReference sound, GameObject ob)
    {
        RuntimeManager.PlayOneShotAttached(sound, ob);
    }
    public void PlayFootsteps(string path)
    {
        EventInstance footsteps = RuntimeManager.CreateInstance(path);
        footsteps.start();
        footsteps.release();
    }
    public void PlaySoundAttached(string path)
    {
        RuntimeManager.PlayOneShotAttached(path, gameObject);
    }
}