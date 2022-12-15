using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    [SerializeField] float delayUntilSceneTransition;
    [SerializeField] SpriteRenderer sprt;
    [SerializeField] private Animator transitionAnimator;

    private int currentScene;
    private int sceneToLoad;
    private float counter = 0;

    void Start()
    {
        sprt.enabled = false;
        currentScene = SceneManager.GetActiveScene().buildIndex;
        ResetTimeScale();
    }

    public void NextLevel()
    {
        Invoke(nameof(OpenScene), delayUntilSceneTransition);
        Invoke(nameof(FadeOutAnimation), delayUntilSceneTransition - 1f);
    }

    private void OpenScene()
    {
        SceneManager.LoadScene(currentScene + 1);
    }

    public void FadeOutAnimation()
    {
        transitionAnimator.SetTrigger("TriggerTransition");
    }

    public void StartSpecificSceneWithDelay(int scene)
    {
        sceneToLoad = scene;
        FadeOutAnimation();
        Invoke(nameof(StartSpecificScene), 1f);
    }

    public void StartSpecificScene()
    {

        SceneManager.LoadScene(sceneToLoad);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResetTimeScale()
    {
        Time.timeScale = 1f;
    }
}
