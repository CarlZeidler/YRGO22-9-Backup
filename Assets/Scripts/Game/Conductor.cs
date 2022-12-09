using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{

    //get from soundplayer reference on maincamera
    private AudioSource musicSource;

    //change in scene view for songs
    public float songBpm;

    public float nrOfSongBeats;

    //the number of seconds for each beat
    private float secPerBeat;

    //current song pos in seconds
    private float songPosition;
    private bool midTriggered = false;

    //current song pos in beats
    [SerializeField] private float songPositionInBeats;

    private float dspSongTime;
    [HideInInspector] public float distanceFromBeat;
    private float closestBeat;

    private float nextBeat = 0;
    private float nextOffBeat = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //number of seconds in each beat
        secPerBeat = 60f / songBpm;

        //time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;
    }

    // Update is called once per frame
    void Update()
    {
        //seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime);

        //beats since the song started
        songPositionInBeats = songPosition / secPerBeat;

        closestBeat = Mathf.RoundToInt(songPositionInBeats);
        distanceFromBeat = closestBeat - songPositionInBeats;

        //pregresses beats
        if (distanceFromBeat < 0)
            distanceFromBeat *= -1;

        if (songPositionInBeats > nextBeat)
        {
            nextBeat++;
        }
        if (songPositionInBeats > nextOffBeat)
        {
            nextOffBeat++;
        }
        if (songPositionInBeats > nrOfSongBeats)
        {
            Start();
            songPosition = 0;
            songPositionInBeats = 0;
            nextBeat = 0;
            nextOffBeat = .5f;
            NewLoop();
        }
        if (songPositionInBeats > nrOfSongBeats / 2&!midTriggered)
            MidLoop();
    }
    void NewLoop()
    {
        GameManager.instance.composer.HackInProgress();
    }
    void MidLoop()
    {
        midTriggered = true;
        GameManager.instance.composer.HackInProgress();
    }
}
