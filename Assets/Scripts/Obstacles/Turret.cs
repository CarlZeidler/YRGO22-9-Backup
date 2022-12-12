using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Turret : HackableObjects
{
    [SerializeField] private float killTime = 1f;
    [SerializeField] private bool pInRange = false;

    [SerializeField]private Animator[] animators = new Animator[2];
    [SerializeField] private ParticleSystem disabledParticles;
    [SerializeField] private AudioSource audS;
    [SerializeField] private AudioClip shoot,detect;
    [SerializeField] private Light2D detectionAreaLight;
    [SerializeField] private Color detectionColor,activeColor,inactiveColor;
    [SerializeField] LayerMask ignoreLayer;

    private void Update()
    {
        if (pInRange)
        {
            RaycastHit2D ray = Physics2D.Raycast(transform.position, (GameManager.instance.player.transform.position - transform.position), Mathf.Infinity, ~ignoreLayer);
            if (ray.collider.gameObject.layer == GameManager.instance.player.layer && pInRange)
            {
                detectionAreaLight.color = detectionColor;
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
    public void Active(bool active)
    {
        if (active)
            detectionAreaLight.color = activeColor;
        else
            detectionAreaLight.color = inactiveColor;
    }
    public void OnPlayerEnter()
    {
        pInRange = true;
        RaycastHit2D ray = Physics2D.Raycast(transform.position, (GameManager.instance.player.transform.position - transform.position), Mathf.Infinity, ~ignoreLayer);
        if(ray.collider.gameObject.layer == GameManager.instance.player.layer && pInRange)
        {
            audS.clip = detect;
            audS.Play();
            detectionAreaLight.color = detectionColor;
            Invoke(nameof(Death), killTime);
            Invoke(nameof(Shoot), killTime - killTime / 8);

        }
    }
    //when obstacles are moved out of area, check for player
    public void TriggerExit()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, (GameManager.instance.player.transform.position - transform.position), Mathf.Infinity, ~ignoreLayer);
        if (ray.collider.gameObject.layer == GameManager.instance.player.layer && pInRange)
        {
            detectionAreaLight.color = detectionColor;
            Invoke(nameof(Death), killTime);
            Invoke(nameof(Shoot), killTime - killTime / 8);

        }
    }
    public void OnPlayerExit()
    {
        pInRange = false;
        detectionAreaLight.color = activeColor;
        CancelInvoke(nameof(Death));
        CancelInvoke(nameof(Shoot));

    }
    private void Shoot()
    {
        audS.clip = shoot;
        foreach (var animator in animators)
        {
            animator.speed = 2  ;
            animator.SetTrigger("Shoot");
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
    public void SetEnable(bool enable)
    {
        foreach (var animator in animators)
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

    public void DisabledTurretParticles(bool disabled)
    {
        
        if (disabled)
        {
            disabledParticles.Play();
        }
        else
        {
            disabledParticles.Stop();
        }
    }
}
