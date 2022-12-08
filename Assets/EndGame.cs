using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprt;
    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private GameObject transitionCanvasObj;
    
    void Start()
    {
        sprt.enabled = false;
        transitionCanvasObj.SetActive(true);
    }

private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Invoke(nameof(OpenMainMenu), 1f);
            Invoke(nameof(Animation), 0.5f);
        }
    }

    private void Animation()
    {
        transitionAnimator.SetTrigger("Transition");
    }

    private void OpenMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
