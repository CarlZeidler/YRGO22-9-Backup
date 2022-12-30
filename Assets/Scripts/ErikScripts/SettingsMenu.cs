using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;


    private void Awake()
    {
        //set value on slider
        try
        {
            GameObject.Find("MasterVolumeSlider").GetComponentInChildren<Slider>().value = PlayerPrefs.GetFloat("masterVolume");
            GameObject.Find("MusicVolumeSlider").GetComponentInChildren<Slider>().value = PlayerPrefs.GetFloat("musicVolume");
            GameObject.Find("SfxVolumeSlider").GetComponentInChildren<Slider>().value = PlayerPrefs.GetFloat("sfxVolume");

            int ison = PlayerPrefs.GetInt("ShowTimer");
            if (ison>0)
                GameObject.Find("SpeedRunTimer").GetComponent<Toggle>().isOn = true;
            else
                GameObject.Find("SpeedRunTimer").GetComponent<Toggle>().isOn = false;
        }
        catch { }
        audioMixer.SetFloat("masterVolume", (PlayerPrefs.GetFloat("masterVolume")));
        audioMixer.SetFloat("musicVolume", (PlayerPrefs.GetFloat("musicVolume")));
        audioMixer.SetFloat("sfxVolume", (PlayerPrefs.GetFloat("sfxVolume")));
    }
    public void SetMasterVolume(float sliderVal)
    {
        audioMixer.SetFloat("masterVolume", sliderVal);
        PlayerPrefs.SetFloat("masterVolume", (sliderVal));
    }
    public void SetMusicVolume(float sliderVal)
    {
        audioMixer.SetFloat("musicVolume", sliderVal);
        PlayerPrefs.SetFloat("musicVolume", (sliderVal));
    }
    public void SetSfxVolume(float sliderVal)
    {
        audioMixer.SetFloat("sfxVolume", sliderVal);
        PlayerPrefs.SetFloat("sfxVolume", (sliderVal));
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    public void ShowSpeedrunTimer(bool show)
    {
        if (show)
            PlayerPrefs.SetInt("ShowTimer", 1);
        else
            PlayerPrefs.SetInt("ShowTimer", 0);

        try
        {
            GameManager.instance.player.GetComponent<PlayerMove>().timerText.enabled = show;
        }
        catch { }
    }
}
