using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRotation : MonoBehaviour
{

    public GameObject rotatorKnob = GameObject.FindGameObjectWithTag("Rotator");
    public Vector3 knobPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CubeTest"))
        {
            rotatorKnob.transform.rotation = Quaternion.Euler(0, 0, -90);
            Debug.Log("collision");
        }
    }

}

