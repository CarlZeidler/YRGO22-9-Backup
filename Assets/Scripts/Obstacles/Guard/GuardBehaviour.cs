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
    [SerializeField] private Color detectionColor, activeColor, inactiveColor;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] private Light2D visionCone;
    [SerializeField] private PolygonCollider2D visionCollider;

    public GuardMove moveScript;
    public GuardVision visionScript;

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
                Invoke(nameof(Shoot), killTime - killTime / 4);
            }
        }
        else
        {
            CancelInvoke(nameof(Death));
            CancelInvoke(nameof(Shoot));
        }

        PlayerProximityCheck();
        VisionConeColor();
    }

    private void VisionConeColor()
    {
        if (moveScript.shutDown)
            visionCone.color = inactiveColor;
        else if (moveScript.playerSeen)
            visionCone.color = detectionColor;
        else
            visionCone.color = activeColor;
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
        }
        else if (moveScript.shutDown)
            CancelInvoke(nameof(KillPlayer));
    }

    public void Shutdown()
    {
        moveScript.canMove = false;
        moveScript.shutDown = true;
        moveScript.playerSeen = false;
        pInRange = false;
        foreach(var animator in animators)
        {
            animator.SetTrigger("Shutdown");
        }
    }

    public void ReActivated()
    {
        //Restore functionality when the hacking time is over
       
        foreach (var animator in animators)
        {
            animator.SetTrigger("Startup");
        }

        //The animator will trigger the ResumeBehavior function
    }

    public void ResumeBehavior()
    {
        moveScript.canMove = true;
        moveScript.shutDown = false;
    }

    public void OnPlayerEnter()
    {
        pInRange = true;
        RaycastHit2D ray = Physics2D.Raycast(transform.position, (GameManager.instance.player.transform.position - transform.position), Mathf.Infinity, ~ignoreLayer);
        if (ray.collider.gameObject.layer == GameManager.instance.player.layer && pInRange)
        {
            Invoke(nameof(Death), killTime);
            Invoke(nameof(Shoot), killTime - killTime / 8);
            moveScript.playerSeen = true;
        }
    }

    public void OnPlayerExit()
    {
        pInRange = false;
        CancelInvoke(nameof(Death));
        CancelInvoke(nameof(Shoot));
        moveScript.playerSeen = false;
        moveScript.canMove = true;
    }

    public void TriggerExit()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, (GameManager.instance.player.transform.position - transform.position), Mathf.Infinity, ~ignoreLayer);
        if (ray.collider.gameObject.layer == GameManager.instance.player.layer && pInRange)
        {
            Invoke(nameof(Death), killTime);
            Invoke(nameof(Shoot), killTime - killTime / 8);
        }
    }

    private void Death()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, (GameManager.instance.player.transform.position - transform.position), Mathf.Infinity, ~ignoreLayer);
        if (ray.collider.gameObject.layer == GameManager.instance.player.layer && pInRange)
        {
            GameManager.instance.player.GetComponent<PlayerRespawn>().Die();

        }
    }

    private void KillPlayer()
    {
        GameManager.instance.player.GetComponent<PlayerRespawn>().Die();
    }

    private void Shoot()
    {
        foreach (var animator in animators)
        {
            animator.SetTrigger("Shoot");
        }
    }
}
