using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMenuSong : MonoBehaviour
{
    void Start()
    {
        FMODAudioManager.instance.PlayMenuMusic();
    }
}
