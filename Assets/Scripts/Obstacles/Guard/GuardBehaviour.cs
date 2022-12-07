using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardBehaviour : HackableObjects
{
    public GuardMove moveScript;
    public GuardVision visionScript;

    [SerializeField] private float killTime = 1f;
    [SerializeField] private bool pInRange = false;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer detectionAreaSprite;
    [SerializeField] private Color detectionColor, activeColor, inactiveColor;
    [SerializeField] LayerMask ignoreLayer;

    private void Update()
    {
        if (pInRange)
        {
            RaycastHit2D ray = Physics2D.Raycast(transform.position, (GameManager.instance.player.transform.position - transform.position), Mathf.Infinity, ~ignoreLayer);
            if (ray.collider.gameObject.layer == GameManager.instance.player.layer && pInRange)
            {
                detectionAreaSprite.color = detectionColor;
                Invoke(nameof(Death), killTime);
                Invoke(nameof(Shoot), killTime - killTime / 8);
            }
        }
        else
        {
            CancelInvoke(nameof(Death));
            CancelInvoke(nameof(Shoot));
        }
    }

    public void Shutdown()
    {
        moveScript.canMove = false;
        visionScript.canSee = false;
        animator.SetTrigger("Shutdown");
        //moveScript.SetCharacterState("shutdown");
    }

    public void ReActivated()
    {
        //Restore functionality when the hacking time is over.
        visionScript.canSee = true;
        moveScript.canMove = true;
        animator.SetTrigger("Startup");
    }

    public void OnPlayerEnter()
    {
        pInRange = true;
        RaycastHit2D ray = Physics2D.Raycast(transform.position, (GameManager.instance.player.transform.position - transform.position), Mathf.Infinity, ~ignoreLayer);
        if (ray.collider.gameObject.layer == GameManager.instance.player.layer && pInRange)
        {
            detectionAreaSprite.color = detectionColor;
            Invoke(nameof(Death), killTime);
            Shoot();
            moveScript.canMove = false;
        }
    }

    public void OnPlayerExit()
    {
        pInRange = false;
        detectionAreaSprite.color = activeColor;
        CancelInvoke(nameof(Death));
        CancelInvoke(nameof(Shoot));
        moveScript.canMove = true;
    }

    public void TriggerExit()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, (GameManager.instance.player.transform.position - transform.position), Mathf.Infinity, ~ignoreLayer);
        if (ray.collider.gameObject.layer == GameManager.instance.player.layer && pInRange)
        {
            detectionAreaSprite.color = detectionColor;
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

    private void Shoot()
    {
        moveScript.canMove = false;
        animator.SetTrigger("Shoot");
        
    }
}
