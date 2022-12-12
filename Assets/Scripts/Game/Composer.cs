using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Composer : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioMixerGroup drums, bass1, bass2, chords, synth1, synth2, mel;
    private List<AudioSource> sources = new List<AudioSource>();
    [Space(20)]
    [SerializeField] private GameObject loop1;
    [SerializeField] private GameObject loop1_5, loop2, drumFill;
    [Space(20)]
    private float _dangerDistance;
    public float maxDangerDistance;
    private float _nearbyHackablesAmount;
    public float nearbyHackablesForMaxVolume;
    private float _chargesSpent;
    public float maxCharges;
    private bool _hackervision;
    private bool _hackInProgress;
    [Space(20)]
    public float fadeDuration = 1;
    [Space(20)] int nextLoop = 2;

    private IEnumerator faderSynth1, faderMel;

    public float dangerDistance
    {
        get { return _dangerDistance; }
        set { _dangerDistance = value; DangerDistance(); }
    }
    public float nearbyHackablesAmount
    {
        get { return _nearbyHackablesAmount; }
        set { _nearbyHackablesAmount = value; NearbyHackables(); }
    }
    public float chargesSpent
    {
        get { return _chargesSpent; }
        set { _chargesSpent = value; SpentBattery(); }
    }
    public bool hackervision
    {
        get { return _hackervision; }
        set { _hackervision = value;
            float volume;

            //set different startpoints based on if enabled for fade effect, volume is endvalue for fade
            if (value)
                volume = 0;
            else
                volume = -30;

            float startVal = 0;
            mixer.GetFloat("Synth1Volume", out startVal);

            StopCoroutine(faderSynth1);
            faderSynth1 = ToggleHackingMode(startVal,volume, fadeDuration);
            StartCoroutine(faderSynth1);
            }
    }
    public void HackInProgress()
    { 
        _hackInProgress = GameManager.instance.somethingIsHacked; 
        CommitHack(GameManager.instance.somethingIsHacked);
    }

    private void Start()
    {
        faderSynth1 = ToggleHackingMode(-40,-40, fadeDuration);
        faderMel = ToggleMelody(-40, -40, fadeDuration);
        Invoke(nameof(OnStart), 0.5f);
    }
    void OnStart()
    {
        maxCharges = GameManager.instance.player.GetComponent<PlayerHack>().maxBatteryCharges;

        sources.AddRange(loop1.GetComponentsInChildren<AudioSource>());
        sources.AddRange(loop1_5.GetComponentsInChildren<AudioSource>());
        sources.AddRange(loop2.GetComponentsInChildren<AudioSource>());
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
        drumFill.SetActive(false);
        switch (nextLoop)
        {
            case 1:
                GameManager.instance.conductor.nrOfSongBeats = 32;
                loop1.SetActive(true);
                nextLoop = 2;
                break;
            case 2:
                GameManager.instance.conductor.nrOfSongBeats = 32;
                loop1_5.SetActive(true);
                nextLoop = 3;
                break;
            case 3:
                GameManager.instance.conductor.nrOfSongBeats = 64;
                loop2.SetActive(true);
                nextLoop = 1;
                break;
            default:
                break;
        }
    }
    public void Stop()
    {
        //drumFill.SetActive(true);
        loop1.SetActive(false);
        loop1_5.SetActive(false);
        loop2.SetActive(false);
        //GameManager.instance.conductor.ResetLoop();
        //GameManager.instance.conductor.nrOfSongBeats = 4;
        //GameManager.instance.conductor.inFill = true;
    }
    public void Inverse()
    {
        foreach (var audioSource in sources)
        {
            audioSource.pitch = audioSource.pitch*-1;
        }
    }

    //guard vision distance
    void DangerDistance()
    {
        float change = dangerDistance / maxDangerDistance;
        float volume = 1 * change;
        volume = Mathf.Log10(volume) * 20;
        SetVolume(bass2, volume);
    }

    //hackervision
    IEnumerator ToggleHackingMode(float startVal, float endVal, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            SetVolume(synth1, Mathf.Lerp(startVal, endVal, time / duration));
            time += Time.unscaledDeltaTime;
            duration += Time.unscaledDeltaTime/2;
            yield return null;
        }
        SetVolume(synth1, endVal);
    }

    //amount of hackables
    void NearbyHackables()
    {
        float change = nearbyHackablesAmount / nearbyHackablesForMaxVolume;
        float volume = 1 * change;
        volume = Mathf.Log10(volume) * 20;
        SetVolume(chords, volume);
    }

    //battery spent
    void SpentBattery()
    {
        float change = chargesSpent / maxCharges;
        float volume = 1 * change;
        volume = Mathf.Log10(volume) * 20;
        SetVolume(synth2, volume);
    }

    //commit
    void CommitHack(bool hacking)
    {
        float currentVolume = 0;
        mixer.GetFloat("MelVolume", out currentVolume);
        if (hacking)
        {
            if (currentVolume < 0f)
            {
                //start fade
                StopCoroutine(faderMel);
                faderMel = ToggleMelody(-40, 0, fadeDuration);
                StartCoroutine(faderMel);
            }
        }
        else
        {
            if (currentVolume > -40f)
            {
                //start fade
                StopCoroutine(faderMel);
                faderMel = ToggleMelody(0, -40, fadeDuration);
                StartCoroutine(faderMel);
            }
        }
    }
    IEnumerator ToggleMelody(float startVal, float endVal, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            SetVolume(mel, Mathf.Lerp(startVal, endVal, time / duration));
            time += Time.unscaledDeltaTime;
            duration += Time.unscaledDeltaTime / 2;
            yield return null;
        }
        SetVolume(mel, endVal);
    }

    //record scartch and fade out rewind on death & respawn
    //trigger progression on reassemble
}
