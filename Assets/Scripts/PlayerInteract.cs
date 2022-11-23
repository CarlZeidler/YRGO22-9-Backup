using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highlightText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Interactable>())
        {
            highlightText.enabled = true;
            //TODO add som fx to interactable object to highlight which one it is
            collision.GetComponent<Interactable>().isInteractable = true;
        }
    }

    //THIS MIGHT BE BUGGED IF NEAR SEVERAL INTERACTABLES AT ONCE
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Interactable>())
        {
            highlightText.enabled = false;
            collision.GetComponent<Interactable>().isInteractable = false;
        }
    }
}
