using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardVision : MonoBehaviour
{
    public GuardBehaviour behaviourScript;
    public bool canSee = true;

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
            Invoke(nameof(PlayerSpotted), 1f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CancelInvoke(nameof(PlayerSpotted));
    }

    private void PlayerSpotted()
    {
        behaviourScript.SpottedPlayer();
    }

}
