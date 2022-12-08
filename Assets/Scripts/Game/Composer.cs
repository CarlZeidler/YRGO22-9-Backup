using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Composer : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioMixerGroup drums, bass1, bass2, chords, synth1, synth2, mel;
    [Space(20)]
    [SerializeField] private GameObject loop1;
    [SerializeField] private GameObject loop1_5, loop2;
    [Space(20)]
    public float _dangerDistance;
    public float maxDangerDistance;
    public float _nearbyHackablesAmount;
    public int _chargesLeft;
    public bool _hackervision;
    public bool _hackInProgress;

    public float dangerDistance
    {
        get { return _dangerDistance; }
        set { _dangerDistance = value; DangerDistance(); }
    }
    public float nearbyHackablesAmount
    {
        get { return _nearbyHackablesAmount; }
        set { _nearbyHackablesAmount = value; }
    }
    public int chargesLeft
    {
        get { return _chargesLeft; }
        set { _chargesLeft = value; }
    }
    public bool hackervision
    {
        get { return _hackervision; }
        set { _hackervision = value; }
    }
    public bool hackInProgress
    {
        get { return _hackInProgress; }
        set { _hackInProgress = value; }
    }

    public void SetVolume(AudioMixerGroup mixerGroup, float volume)
    {
        if(mixerGroup == drums)
            mixer.SetFloat("DrumsVolume", volume);

        else if(mixerGroup == bass1)
            mixer.SetFloat("Bass1Volume", volume);

        else if (mixerGroup == bass2)
            mixer.SetFloat("Bass2Volume", volume);

        else if (mixerGroup == chords)
            mixer.SetFloat("ChordsVolume", volume);

        else if (mixerGroup == synth1)
            mixer.SetFloat("Synth1Volume", volume);

        else if (mixerGroup == synth2)
            mixer.SetFloat("Synth2Volume", volume);

        else if (mixerGroup == mel)
            mixer.SetFloat("MelVolume", volume);
    }
    public void ChangeLoop()
    {
        loop1.SetActive(!loop1.activeSelf);
        loop1_5.SetActive(!loop1_5.activeSelf);
        loop1.SetActive(!loop1.activeSelf);
    }
    void DangerDistance()
    {
        //float volume = Mathf.Clamp(((dangerDistance / maxDangerDistance) * 100) - 60,-80,0);
        float change = dangerDistance / maxDangerDistance;
        float volume = 1 * change;
        volume = Mathf.Log10(volume) * 20;
        SetVolume(bass2, volume);
        //Debug.Log(volume);
    }
    //guard vision distance
    //amount of hackables
    //hackervision
    //commit
    //battery spent

    //record scartch and fade out rewind on death & respawn
    //trigger progression on reassemble
}
