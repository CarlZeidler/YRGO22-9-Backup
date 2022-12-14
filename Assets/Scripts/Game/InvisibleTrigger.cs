using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InvisibleTrigger : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprt;
    [SerializeField] private typeOfTrigger triggerType;
    [SerializeField] private EventTrigger eventScript;
    [SerializeField] private bool resetOnReassemble;
    [SerializeField] private bool oneTimeUse;

    [Header("Settings for repeating triggers")]
    [SerializeField] private float startTime;
    [SerializeField] private float repeatInterval;
    [SerializeField] private bool limitNumberOfRepeats;
    [SerializeField] private int numberOfRepeats;

    [SerializeField] private enum typeOfTrigger { Single, Repeating };

    private int counter;

    private void Start()
    {
        sprt.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && resetOnReassemble)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerType == typeOfTrigger.Repeating && other.CompareTag("Player"))
        {
            InvokeRepeating(nameof(triggerEvent), startTime, repeatInterval);
            if (oneTimeUse)
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (other.CompareTag("Player") && oneTimeUse)
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    private void triggerEvent()
    {
        if (!limitNumberOfRepeats)
            eventScript.linkedEvent.Invoke();
        else if (limitNumberOfRepeats && counter <= numberOfRepeats)
        {
            eventScript.linkedEvent.Invoke();
            counter++;
        }
        else
        {
            CancelInvoke(nameof(triggerEvent));
            counter = 0;
        }
    } 
}
