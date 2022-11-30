using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{
    public void OpenScene(int index)
    {
        //Placeholder scene assigned
        SceneManager.LoadScene(index);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartTime()
    {
        Time.timeScale = 1f;
    }
}
