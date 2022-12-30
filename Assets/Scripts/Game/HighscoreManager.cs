using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighscoreManager : MonoBehaviour
{
    public int[] highscores;
    public string[] highscoreNames;

    public string name = "Player";

    [SerializeField] private string bestTime, highscoreStringName;

    [SerializeField] private TextMeshProUGUI[] highscoreTexts;
    [SerializeField] private TextMeshProUGUI time,battery,hacks,respawns;


    private void Start()
    {
        if(GameManager.instance.stats.updateTimer)
        {
            GameManager.instance.stats.updateTimer = false;
            GameManager.instance.timerActive = false;
            if(GetHighscore()<GameManager.instance.stats.time)
                SetHighscore(GameManager.instance.stats.time);
        }
        WriteScore();
    }
    void UpdateScores(float score)
    {
        for (int i = 0; i < highscores.Length; i++)
        {
            if (score > highscores[i])
            {
                PlayerPrefs.SetFloat(bestTime + i.ToString(), score);
                PlayerPrefs.SetString(highscoreStringName + i.ToString(), name);
            }
        }
    }
    //void GetScores()
    //{
    //    for (int i = 0; i < highscores.Length; i++)
    //    {
    //        highscores[i] = PlayerPrefs.GetInt(bestTime + i.ToString());
    //        highscoreNames[i] = PlayerPrefs.GetString(highscoreStringName + i.ToString());
    //    }
    //}
    public float GetHighscore()
    {
        return PlayerPrefs.GetFloat(bestTime);
    }
    public void SetHighscore(float score)
    {
        if (PlayerPrefs.GetFloat(bestTime) == 0)
            PlayerPrefs.SetFloat(bestTime, 99999);
        if (score < PlayerPrefs.GetFloat(bestTime))
            PlayerPrefs.SetFloat(bestTime, score);
    }
    public void WriteScore()
    {
        time.text = SecondsToTime(GetHighscore()).ToString();
        battery.text = GameManager.instance.stats.batterySpent.ToString();
        hacks.text = GameManager.instance.stats.nrOfHacks.ToString();
        respawns.text = GameManager.instance.stats.nrOfRespawns.ToString();
    }
    public void SetName(string name)
    {
        this.name = name;
    }
    public void WriteScores()
    {
        for (int i = 0; i < highscores.Length; i++)
        {
            highscoreTexts[i].text = highscoreNames + " " + highscores[i].ToString();
        }
    }
    public string SecondsToTime(float seconds)
    {
        TimeSpan t = TimeSpan.FromSeconds(seconds);

        string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                        t.Hours,
                        t.Minutes,
                        t.Seconds,
                        t.Milliseconds);

        return answer;
    }
}
