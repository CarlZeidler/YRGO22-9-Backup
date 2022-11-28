using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackablePlatform : HackableObjects
{
    public bool solid;

    public void Toggle()
    {
        solid = !solid;

        if (solid)
        {
            GetComponent<Collider2D>().enabled = true;
            GetComponent<SpriteRenderer>().material.color = Color.white;
        }
        else
        {
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().material.color = Color.black;
        }
    }

}
