using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelTrigger : MonoBehaviour
{
    public SceneManagerScript sceneManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && sceneManager.lastLevel)
        {
            sceneManager.StartSpecificSceneWithDelay(0);
            GameManager.instance.stats.updateTimer = true;
        }
        else if (collision.CompareTag("Player"))
        {
            sceneManager.NextLevel();
        }
    }
}
