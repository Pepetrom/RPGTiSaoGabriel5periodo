using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMODAudioManager : MonoBehaviour
{
    public static FMODAudioManager instance;
    public GameObject cameraMenu;
    [Header("Ambiente")]
    public EventReference bonfireInteract;
    public EventReference boxCrash;
    public EventReference notes;
    public EventReference lever;
    public EventReference valve;
    public EventReference fillingWater;
    public EventReference item;

    [Header("player")]
    public EventReference[] sword;
    public EventReference estus;
    public EventReference dash;
    public EventReference hoffDeath;

    [Header("Porquinho")]
    public EventReference porquinDeath;
    public EventReference porquin;
    public EventReference porquinBlood;

    [Header("Caranguejo")]
    public EventReference takingDamage;
    public EventReference crabDeath;
    [Header("Jacaré")]
    public EventReference machineGun;
    public EventReference bubbleExplosion;

    [Header("UI")]
    public EventReference pressedButton;
    public EventReference hoverButton;
    public EventReference buyRune;
    public EventReference noMoney;

    [Header("Tartaruga")]
    public EventReference turtleDamage;

    [Header("Músicas")]
    public EventReference menuSong;
    public EventReference kroSong;
    private EventInstance menuSongInstance;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
    public void PlayMenuMusic(EventReference song)
    {
        menuSongInstance = RuntimeManager.CreateInstance(song);
        menuSongInstance.start();
    }
    public void StopMenuMusic()
    {
        menuSongInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        menuSongInstance.release();
    }
}