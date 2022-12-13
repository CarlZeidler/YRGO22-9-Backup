using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    [SerializeField] float DelayUntilSceneTransition;
    [SerializeField] SpriteRenderer sprt;
    [SerializeField] private Animator transitionAnimator;
    
    private int currentScene;

    void Start()
    {
        sprt.enabled = false;
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    public void NextLevel()
    {
        Invoke(nameof(OpenScene), DelayUntilSceneTransition);
        Invoke(nameof(FadeOutAnimation), DelayUntilSceneTransition - 1f);
    }

    private void OpenScene()
    {
        SceneManager.LoadScene(currentScene + 1);
    }

    private void FadeOutAnimation()
    {
        transitionAnimator.SetTrigger("TriggerTransition");
    }

    public void StartGame()
    {
        Invoke(nameof(StartGameLoadScene), DelayUntilSceneTransition);
        FadeOutAnimation();
    }

    private void StartGameLoadScene()
    {
        SceneManager.LoadScene(4);
    }

    public void StartSpecificScene(int scene, float delay)
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        Invoke(nameof(StartSpecificScene), delay);
        Invoke(nameof(FadeOutAnimation), delay - 1f);
    }

    public void StartSpecificScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
