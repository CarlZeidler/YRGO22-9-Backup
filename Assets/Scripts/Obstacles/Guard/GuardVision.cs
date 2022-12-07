using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardVision : MonoBehaviour
{
    public GuardBehaviour behaviourScript;
    public bool canSee = true;
    private PlayerRespawn respawnScript;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] PolygonCollider2D thisCollider;

    public void Update()
    {
        VisionConeVisibility();
    }

    private void VisionConeVisibility()
    {
        if (!canSee)
        {
            spriteRenderer.enabled = false;
            thisCollider.enabled = false;
        }
        else
        {
            spriteRenderer.enabled = true;
            thisCollider.enabled = true;
        }
    }


    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        respawnScript = other.GetComponent<PlayerRespawn>();
    //        Invoke(nameof(PlayerSpotted), killtime);
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if(collision.CompareTag("Player"))
    //        CancelInvoke(nameof(PlayerSpotted));
    //}

    //private void PlayerSpotted()
    //{
    //    respawnScript.Die();
    //}

}
