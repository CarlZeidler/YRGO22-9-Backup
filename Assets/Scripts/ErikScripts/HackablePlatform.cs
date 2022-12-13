using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HackablePlatform : HackableObjects
{
    public bool solid;

    [SerializeField] private float speed = 1;
    [SerializeField] private Animator animator;

    private void Start()
    {
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
        //save own state if red spawn this on player respawn
        if (objectState == ObjectState.redUnPersistent)
        {
            originalState = gameObject;
        }
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
}
