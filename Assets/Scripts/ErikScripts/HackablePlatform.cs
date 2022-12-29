using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class HackablePlatform : HackableObjects
{
    public bool solid;

    [SerializeField] private float speed = 1;
    [SerializeField] private Animator animator;

    private void Start()
    {
        //GetComponent<ShadowCaster2D>().castsShadows = false;
        if (solid)
        {
            ActivatePlatform();
            if (solid)
            {
                GetComponent<Collider2D>().enabled = true;
                gameObject.layer = 6;
            }
            else
            {
                GetComponent<Collider2D>().enabled = false;
                gameObject.layer = 9;
            }

        }

        //hackable
        //add to manager list
        GameManager.instance.hackableObjects.Add(this);
    }

    public void Toggle()
    {
        solid = !solid;
        ActivatePlatform();
        if (solid)
        {
            GetComponent<Collider2D>().enabled = true;
            gameObject.layer = 6;
        }
        else
        {
            GetComponent<Collider2D>().enabled = false;
            gameObject.layer = 9;
        }
    }

    public void ActivatePlatform()
    {
        animator.speed = speed;
        if (solid)
        {
            animator.SetTrigger("Activate");
        }
        else
        {
            animator.SetTrigger("Deactivate");
        }
    }

    public void ToggleShadows()
    {

    }
}
