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
    [SerializeField] private bool stopRepeatingOnReassemble;

    [SerializeField] private enum typeOfTrigger { Single, Repeating };

    private int counter;
    private float respawnHold;
    private PlayerRespawn respawnScript;

    private void Start()
    {
        sprt.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab) && GameManager.instance.player.GetComponent<PlayerRespawn>().isDead && resetOnReassemble)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }

        if (Input.GetKey(KeyCode.Tab) && GameManager.instance.player.GetComponent<PlayerRespawn>().isDead && stopRepeatingOnReassemble)
        {
            CancelInvoke(nameof(triggerEvent));
        }

        if (Input.GetKey(KeyCode.Tab) && respawnHold >= 0.5f)
        {
            respawnHold = 0f;
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        else if (Input.GetKey(KeyCode.Tab))
        {
            respawnHold += 1 * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Tab) && stopRepeatingOnReassemble && respawnHold >= 0.5f)
        {
            respawnHold = 0f;
            CancelInvoke(nameof(triggerEvent));
        }
        else if (Input.GetKey(KeyCode.Tab))
        {
            respawnHold += 1 * Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Tab))
            respawnHold = 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerType == typeOfTrigger.Repeating && other.CompareTag("Player"))
        {
            InvokeRepeating(nameof(triggerEvent), startTime, repeatInterval);
        }
        if (other.CompareTag("Player") && oneTimeUse)
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    { 

    }

    private void triggerEvent()
    {
        if (!limitNumberOfRepeats)
        {
            eventScript.linkedEvent.Invoke();
        }
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

    public void RemoteTrigger()
    {
        //This function is for triggering several other hackables by using one hackable.
        //Ensure this trigger is not placed in a place where the player can collide with it, or turn the collider off.

        eventScript.linkedEvent.Invoke();
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
