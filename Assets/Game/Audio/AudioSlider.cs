using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    private FMOD.Studio.VCA vcaController;
    private Slider slider;
    public string vcaName;

    private void Start()
    {
        vcaController = FMODUnity.RuntimeManager.GetVCA("vca:/" + vcaName);
        slider = GetComponent<Slider>();
    }
    public void SetVolume(float volume)
    {
        vcaController.setVolume(volume);
    }
    public void SetVolumeToDefault()
    {
        vcaController.setVolume(0.7f);
    }
}
