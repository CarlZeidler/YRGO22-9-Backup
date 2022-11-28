using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent linkedEvent;
    public bool isInteractable;

    void Update()
    {
        if (Input.GetButtonDown("Interact") && isInteractable)
        {
            linkedEvent.Invoke();
            //isInteractable = false;

            //replace with something else later
           // GetComponent<Collider2D>().enabled = false;
        }
    }
}
