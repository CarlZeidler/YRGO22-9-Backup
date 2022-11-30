using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPlayerFollow : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GameManager.instance.player.transform.parent = this.transform;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GameManager.instance.player.transform.parent = null;
        }
    }
    public void OnDespawn()
    {
        if(GameManager.instance.player.transform.parent == transform)
            GameManager.instance.player.transform.parent = null;

    }
}
