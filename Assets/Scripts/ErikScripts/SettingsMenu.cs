using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetMasterVolume(float sliderVal)
    {
        audioMixer.SetFloat("masterVolume", sliderVal);
    }
    public void SetMusicVolume(float sliderVal)
    {
        audioMixer.SetFloat("musicVolume", sliderVal);
    }
    public void SetSfxVolume(float sliderVal)
    {
        audioMixer.SetFloat("sfxVolume", sliderVal);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
