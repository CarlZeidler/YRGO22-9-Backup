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

    public void NextLevel(float delay)
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        Invoke(nameof(OpenScene), delay);
        Invoke(nameof(Animation), delay - 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            NextLevel(levelDelay);
        }
    }

    private void OpenScene()
    {
        SceneManager.LoadScene(currentScene + 1);
    }

    private void Animation()
    {
        transitionAnimator.SetTrigger("Transition");
    }
}
