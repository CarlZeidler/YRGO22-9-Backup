using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    [SerializeField] float levelDelay;
    [SerializeField] SpriteRenderer sprt;
    //[SerializeField] private Canvas transitionCanvas;
    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private GameObject transitionCanvasObj;
    
    private int currentScene;

    void Start()
    {
        sprt.enabled = false;
        transitionCanvasObj.SetActive(true);
        //transitionCanvas.worldCamera = Camera.main;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            NextLevel(levelDelay);
        }
    }

    public void NextLevel(float delay)
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        Invoke(nameof(OpenScene), delay);
        Invoke(nameof(Animation), delay - 0.5f);
    }


    private void OpenScene()
    {
        SceneManager.LoadScene(currentScene + 1);
    }

    public void StartGame()
    {
        Invoke(nameof(StartGameLoadScene), 0.5f);
        Animation();
    }

    private void StartGameLoadScene()
    {
        SceneManager.LoadScene(4);
    }

    private void Animation()
    {
        transitionAnimator.SetTrigger("Transition");
    }

    public void StartSpecificScene(int scene, float delay)
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        Invoke(nameof(StartSpecificScene), delay);
        Invoke(nameof(Animation), delay - 0.5f);
    }

    private void StartSpecificScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
