using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackablePlatform : HackableObjects
{
    public bool solid;
    
    private void Start()
    {
        if (!solid)
        {
            Toggle();
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
        if (solid)
        {
            GetComponent<Collider2D>().enabled = true;
            GetComponent<SpriteRenderer>().material.color = Color.white;
            gameObject.layer = 6;
        }
        else
        {
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().material.color = Color.black;
            gameObject.layer = 9;
        }
        solid = !solid;
    }
}
