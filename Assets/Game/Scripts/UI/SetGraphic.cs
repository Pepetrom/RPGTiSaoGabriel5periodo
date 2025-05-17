using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

public class SetGraphic : MonoBehaviour
{
    [Header("Dropdowns")]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private TMP_Dropdown screenModeDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    [Header("Post Process")]
    [SerializeField] private Volume postProcessVolume;
    [SerializeField] private Slider postExposureSlider;

    private ColorAdjustments colorAdjustments;

    private List<Resolution> customResolutions = new List<Resolution>
    {
        new Resolution { width = 1280, height = 720 },
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 2560, height = 1440 }
    };

    private void Start()
    {
        SetupQualityDropdown();
        SetupScreenModeDropdown();
        SetupResolutionDropdown();
        SetupPostProcessSlider();
    }

    #region Quality
    private void SetupQualityDropdown()
    {
        qualityDropdown.ClearOptions();
        List<string> options = new List<string>(QualitySettings.names);
        qualityDropdown.AddOptions(options);
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();
        qualityDropdown.onValueChanged.AddListener(SetQualityLevelDropdown);
    }

    public void SetQualityLevelDropdown(int index)
    {
        QualitySettings.SetQualityLevel(index, false);
    }
    #endregion

    #region Screen Mode
    private void SetupScreenModeDropdown()
    {
        screenModeDropdown.ClearOptions();
        screenModeDropdown.AddOptions(new List<string> { "Janela", "Tela Cheia", "Sem Bordas" });

        FullScreenMode mode = Screen.fullScreenMode;
        screenModeDropdown.value = mode switch
        {
            FullScreenMode.Windowed => 0,
            FullScreenMode.ExclusiveFullScreen => 1,
            FullScreenMode.FullScreenWindow => 2,
            _ => 0
        };

        screenModeDropdown.RefreshShownValue();
        screenModeDropdown.onValueChanged.AddListener(SetScreenMode);
    }

    private void SetScreenMode(int index)
    {
        FullScreenMode selectedMode = index switch
        {
            0 => FullScreenMode.Windowed,
            1 => FullScreenMode.ExclusiveFullScreen,
            2 => FullScreenMode.FullScreenWindow,
            _ => FullScreenMode.Windowed
        };

        Screen.fullScreenMode = selectedMode;
    }
    #endregion

    #region Resolution
    private void SetupResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>();
        foreach (var res in customResolutions)
        {
            resolutionOptions.Add($"{res.width}x{res.height}");
        }

        resolutionDropdown.AddOptions(resolutionOptions);

        // seleciona resolução atual como a inicial (se tiver na lista)
        int currentIndex = customResolutions.FindIndex(r => r.width == Screen.width && r.height == Screen.height);
        resolutionDropdown.value = currentIndex != -1 ? currentIndex : 1;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    private void SetResolution(int index)
    {
        Resolution selected = customResolutions[index];
        Screen.SetResolution(selected.width, selected.height, Screen.fullScreenMode);
    }
    #endregion

    #region Post Exposure
    private void SetupPostProcessSlider()
    {
        if (postProcessVolume != null && postProcessVolume.profile.TryGet(out colorAdjustments))
        {
            postExposureSlider.minValue = -5f;
            postExposureSlider.maxValue = 5f;
            postExposureSlider.value = colorAdjustments.postExposure.value;

            postExposureSlider.onValueChanged.AddListener(SetPostExposure);
        }
    }

    private void SetPostExposure(float value)
    {
        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.Override(value);
        }
    }
    #endregion
}
