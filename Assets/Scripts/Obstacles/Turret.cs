using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : HackableObjects
{
    [SerializeField] private float killTime = 1f;
    [SerializeField] private bool pInRange = false;

    [SerializeField]private Animator animator;
    [SerializeField] private AudioSource audS;
    [SerializeField] private AudioClip shoot;
    [SerializeField] private SpriteRenderer detectionAreaSprite;
    [SerializeField] private Color detectionColor,activeColor,inactiveColor;

    public void Active(bool active)
    {
        if (active)
            detectionAreaSprite.color = activeColor;
        else
            detectionAreaSprite.color = inactiveColor;
    }
    public void OnPlayerEnter()
    {
        pInRange = true;
        RaycastHit2D ray = Physics2D.Raycast(transform.position, (GameManager.instance.player.transform.position-transform.position), Mathf.Infinity, ~gameObject.layer);
        if(ray.collider.gameObject.layer-1 == GameManager.instance.player.layer&&pInRange)
        {
            detectionAreaSprite.color = detectionColor;
            Invoke(nameof(Death), killTime);
            Invoke(nameof(Shoot), killTime - killTime / 8);

        }
    }
    //when obstacles are moved out of area, check for player
    public void TriggerExit()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, (GameManager.instance.player.transform.position - transform.position), Mathf.Infinity, ~gameObject.layer);
        Debug.Log(GameManager.instance.player.layer);
        if (ray.collider.gameObject.layer - 1 == GameManager.instance.player.layer && pInRange)
        {
            detectionAreaSprite.color = detectionColor;
            Invoke(nameof(Death), killTime);
            Invoke(nameof(Shoot), killTime - killTime / 8);

        }
    }
    public void OnPlayerExit()
    {
        pInRange = false;
        detectionAreaSprite.color = activeColor;
        CancelInvoke(nameof(Death));
        CancelInvoke(nameof(Shoot));
    }
    private void Shoot()
    {
        animator.speed = 2  ;
        animator.SetTrigger("Shoot");
    }
    private void Death()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, (GameManager.instance.player.transform.position - transform.position), Mathf.Infinity, ~gameObject.layer);
        if (ray.collider.gameObject.layer - 1 == GameManager.instance.player.layer && pInRange)
        {
            GameManager.instance.player.GetComponent<PlayerRespawn>().Respawn();

        }
    }
    public void SetEnable(bool enable)
    {
        animator.speed = 1;
        if (enable)
        {
            animator.SetTrigger("Bootup");
        }
        else
        {
            animator.SetTrigger("Shutdown");
        }
    }
}
