using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GuardBehaviour : HackableObjects
{
    [SerializeField] private float killTime = 1f;
    [SerializeField] private bool pInRange = false;
    [SerializeField] private Animator[] animators = new Animator[2];
    [SerializeField] private SpriteRenderer detectionAreaSprite;
    [SerializeField] private Color detectionColor, activeColor, inactiveColor;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] private Light2D visionCone;
    [SerializeField] private PolygonCollider2D visionCollider;

    [SerializeField] private Transform topLeftRaycastReference;
    [SerializeField] private Transform topRightRaycastReference;
    [SerializeField] private Transform bottomLeftRaycastReference;
    [SerializeField] private Transform bottomRightRaycastReference;

    public GuardMove moveScript;
    public GuardVision visionScript;

    private bool canSee;
    private bool tooClose = false;
    private void Update()
    {
        if (pInRange)
        {
            RaycastHit2D ray = Physics2D.Raycast(transform.position, (GameManager.instance.player.transform.position - transform.position), Mathf.Infinity, ~ignoreLayer);
            if (ray.collider.gameObject.layer == GameManager.instance.player.layer && pInRange)
            {
                visionCone.color = detectionColor;
                Invoke(nameof(Death), killTime);
                Invoke(nameof(Shoot), killTime - killTime / 8);
            }
        }
        else
        {
            CancelInvoke(nameof(Death));
            CancelInvoke(nameof(Shoot));
        }

        PlayerProximityCheck();
    }

    private void PlayerProximityCheck()
    {
        float minDist = 2f;
        float dist = Vector3.Distance(GameManager.instance.player.transform.position, transform.position);
        
        if (dist < minDist && !moveScript.shutDown && !tooClose)
        {
            if (moveScript.facingRight)
            {
                if (GameManager.instance.player.transform.position.x < transform.position.x)
                {
                    Invoke(nameof(KillPlayer), 2f);
                    tooClose = true;
                }
                else if (GameManager.instance.player.transform.position.x > transform.position.x)
                {
                    Invoke(nameof(KillPlayer), 0f);
                    tooClose = true;
                }
            }
            else if (!moveScript.facingRight)
            {
                if (GameManager.instance.player.transform.position.x > transform.position.x)
                {
                    Invoke(nameof(KillPlayer), 2f);
                    tooClose = true;
                }
                else if (GameManager.instance.player.transform.position.x < transform.position.x)
                {
                    Invoke(nameof(KillPlayer), 0f);
                    tooClose = true;
                }
            }
        }
        else if (dist > minDist && tooClose)
        {
            CancelInvoke(nameof(KillPlayer));
            tooClose = false;
            Debug.Log("Got away");
        }
        else if (moveScript.shutDown)
            CancelInvoke(nameof(KillPlayer));
    }

    public void Shutdown()
    {
        moveScript.canMove = false;
        moveScript.shutDown = true;
        canSee = false;
        foreach(var animator in animators)
        {
            animator.SetTrigger("Shutdown");
        }
        visionCone.color = inactiveColor;
    }

    public void ReActivated()
    {
        //Restore functionality when the hacking time is over.
        canSee = true;
        moveScript.canMove = true;
        moveScript.shutDown = false;
        foreach (var animator in animators)
        {
            animator.SetTrigger("Startup");
        }
        visionCone.color = activeColor;
    }

    public void OnPlayerEnter()
    {
        pInRange = true;
        RaycastHit2D ray = Physics2D.Raycast(transform.position, (GameManager.instance.player.transform.position - transform.position), Mathf.Infinity, ~ignoreLayer);
        if (ray.collider.gameObject.layer == GameManager.instance.player.layer && pInRange)
        {
            visionCone.color = detectionColor;
            Invoke(nameof(Death), killTime);
            Shoot();
            moveScript.canMove = false;
        }
    }

    public void OnPlayerExit()
    {
        pInRange = false;
        visionCone.color = activeColor;
        CancelInvoke(nameof(Death));
        CancelInvoke(nameof(Shoot));
        moveScript.canMove = true;
    }

    public void TriggerExit()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, (GameManager.instance.player.transform.position - transform.position), Mathf.Infinity, ~ignoreLayer);
        if (ray.collider.gameObject.layer == GameManager.instance.player.layer && pInRange)
        {
            visionCone.color = detectionColor;
            Invoke(nameof(Death), killTime);
            Invoke(nameof(Shoot), killTime - killTime / 8);

        }
    }

    private void Death()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, (GameManager.instance.player.transform.position - transform.position), Mathf.Infinity, ~ignoreLayer);
        if (ray.collider.gameObject.layer == GameManager.instance.player.layer && pInRange)
        {
            GameManager.instance.player.GetComponent<PlayerRespawn>().Respawn();

        }
    }

    private void KillPlayer()
    {
        GameManager.instance.player.GetComponent<PlayerRespawn>().Respawn();
    }

    private void Shoot()
    {
        moveScript.canMove = false;
        foreach (var animator in animators)
        {
            animator.SetTrigger("Shoot");
        }
    }

    //private void VisionConeVisibility()
    //{
    //    if (!canSee)
    //    {
    //        visionCone.color = inactiveColor;   
    //        visionCollider.enabled = false;
    //        Debug.Log("Inactive color");
    //    }
    //    else
    //    {
    //        visionCone.color = activeColor;
    //        visionCollider.enabled = true;
    //    }
    //}
}
