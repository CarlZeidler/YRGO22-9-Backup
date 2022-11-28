using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void StartGame()
    {
        //Placeholder scene assigned
        SceneManager.LoadScene("ErikScene");
    }

    public void ContinueGame()
    {
        //Placeholder scene assigned
        SceneManager.LoadScene("ErikScene");
    }

    public void OptionsMenu()
    {
        //Placeholder scene assigned
        SceneManager.LoadScene("ErikScene");
    }

    public void CreditsMenu()
    {
        //Placeholder scene assigned
        SceneManager.LoadScene("ErikScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
