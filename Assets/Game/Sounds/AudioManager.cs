using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] AudioClip[] audios;
    [SerializeField] AudioMixer audioMixer;
    public void PlayAudioHere(int audioId, Transform audioPosition)
    {
        AudioSource.PlayClipAtPoint(audios[audioId], audioPosition.position,1);
    }
    private void Awake()
    {
        instance = this;
    }
}
