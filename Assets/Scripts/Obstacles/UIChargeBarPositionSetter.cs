using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChargeBarPositionSetter : MonoBehaviour
{
    private Vector2 ogPos;
    public float xOffset, yOffset;

    private void Start()
    {
        ogPos = transform.position;
    }

    private void Update()
    {
        Vector2 newPosition = transform.position;
        float x, y;

        x = Camera.main.WorldToScreenPoint(ogPos).x;
        y = Camera.main.WorldToScreenPoint(ogPos).y;


        if (Camera.main.WorldToScreenPoint(ogPos).x > Screen.width-xOffset)
        {
             x = Screen.width - xOffset;
        }
        else if (Camera.main.WorldToScreenPoint(ogPos).x < xOffset)
        {
             x = xOffset;
        }

        if (Camera.main.WorldToScreenPoint(ogPos).y > Screen.height-yOffset) 
        {
             y = Screen.height - yOffset;
        }
        else if(Camera.main.WorldToScreenPoint(ogPos).y < yOffset)
        {
            y = yOffset;
        }


        newPosition = Camera.main.ScreenToWorldPoint(new Vector2(x, y));
            
        transform.position = newPosition;
    }
}
