using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardVisionScript : MonoBehaviour
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
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canSee)
        {
            behaviourScript.SpottedPlayer();
        }
    }
}
