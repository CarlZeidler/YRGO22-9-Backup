using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardVision : MonoBehaviour
{
    public GuardBehaviour behaviourScript;
    public bool canSee = true;
    private PlayerRespawn respawnScript;
    public float killtime;

    public void Update()
    {
        VisionConeVisibility();
    }

    private void VisionConeVisibility()
    {
        if (!canSee)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<PolygonCollider2D>().enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            respawnScript = other.GetComponent<PlayerRespawn>();
            Invoke(nameof(PlayerSpotted), killtime);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            CancelInvoke(nameof(PlayerSpotted));
    }

    private void PlayerSpotted()
    {
        respawnScript.Die();
    }

}
