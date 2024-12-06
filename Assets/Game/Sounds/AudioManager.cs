using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip[] audios;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioSource audioSource;
    public void PlayAudio(int audioId, int audioSourceId )
    {
        audioSource.clip = audios[audioId];
    }
    private void Start()
    {
        if (!audioSource) audioSource = GetComponent<AudioSource>();
    }
}
