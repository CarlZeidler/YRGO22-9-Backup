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
            //GetComponent<Renderer>().material.color = new Color(255, 0, 0);
        }
        else
        {
            GetComponent<Collider2D>().enabled = false;
            //GetComponent<Renderer>().material.color = new Color(0, 255, 0);
        }
    }

}
