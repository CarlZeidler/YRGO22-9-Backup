using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackablePlatform : HackableObjects
{
    private Renderer setColor;
    public bool solid;

    private void Start()
    {
        setColor = GetComponent<Renderer>();
    }

    public void Toggle()
    {
        solid = !solid;

        if (solid)
        {
            GetComponent<Collider2D>().enabled = true;
            setColor.material.color = new Color(0, 255, 0);
        }
        else
        {
            GetComponent<Collider2D>().enabled = false;
            setColor.material.color = new Color(255, 0, 0);
        }
    }

}
