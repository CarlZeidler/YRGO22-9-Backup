using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private static bool paused = false;
    [SerializeField] private GameObject pauseMenuCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Play();
            }
            else
            {
                Stop();
            }
        }
    }
    public void Play()
    {
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }

    private void Stop()
    {
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }
}
