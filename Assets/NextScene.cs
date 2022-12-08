using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    [SerializeField] float levelDelay;
    [SerializeField] SpriteRenderer sprt;
    private int currentScene;

    void Start()
    {
        sprt.enabled = false;
    }

    public void NextLevel(float delay)
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        Invoke(nameof(OpenScene), delay);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            NextLevel(levelDelay);
    }

    public void OpenScene()
    {
        SceneManager.LoadScene(currentScene + 1);
    }
}
