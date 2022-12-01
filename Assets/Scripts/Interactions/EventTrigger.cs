using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    public UnityEvent linkedEvent;
    public UnityEvent linkedEventOnExit;
    [Space]
    public string colliderTag, colliderTag2;
    [Space]
    public string colliderLayer, colliderLayer2;

    public Collider2D TriggerCollider;
    public bool useTag1, useTag2, useLayer1, useLayer2;
    [Space]
    public bool useTimer;
    public bool useTimerOnExit;
    public float timer;
    [Space]
    public bool disableColliderOnTrigger;
    public bool disableColliderOnTriggerExit;

    public bool triggerOnExit;
    public bool separateEventOnTriggerExit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        {
            if (other.tag == colliderTag&&useTag1)
            {
                if (useTimer)
                    StartCoroutine(InvokeTimer(linkedEvent, timer));
                else
                    linkedEvent.Invoke();

                if(disableColliderOnTrigger)
                    TriggerCollider.enabled = false;
            }
            else if (other.tag == colliderTag2 && useTag2)
            {
                if (useTimer)
                    StartCoroutine(InvokeTimer(linkedEvent, timer));
                else
                    linkedEvent.Invoke();
                if (disableColliderOnTrigger)
                    TriggerCollider.enabled = false;
            }
            else if(LayerMask.LayerToName(other.gameObject.layer) == colliderLayer && useLayer1)
            {
                if (useTimer)
                    StartCoroutine(InvokeTimer(linkedEvent, timer));
                else
                    linkedEvent.Invoke();
                if (disableColliderOnTrigger)
                    TriggerCollider.enabled = false;
            }
            else if (LayerMask.LayerToName(other.gameObject.layer) == colliderLayer2 && useLayer2)
            {
                if (useTimer)
                    StartCoroutine(InvokeTimer(linkedEvent, timer));
                else
                    linkedEvent.Invoke();
                if (disableColliderOnTrigger)
                    TriggerCollider.enabled = false;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        {
            if (triggerOnExit)
            {
                if (separateEventOnTriggerExit)
                {
                    if (linkedEventOnExit!=null)
                    {
                        if (other.tag == colliderTag && useTag1)
                        {
                            if (useTimerOnExit)
                                StartCoroutine(InvokeTimer(linkedEventOnExit, timer));
                            else
                                linkedEventOnExit.Invoke();
                            if (disableColliderOnTrigger)
                                TriggerCollider.enabled = false;
                        }
                        else if (other.tag == colliderTag2 && useTag2)
                        {
                            if (useTimerOnExit)
                                StartCoroutine(InvokeTimer(linkedEventOnExit, timer));
                            else
                                linkedEventOnExit.Invoke();
                            if (disableColliderOnTrigger)
                                TriggerCollider.enabled = false;
                        }
                        else if (LayerMask.LayerToName(other.gameObject.layer) == colliderLayer && useLayer1)
                        {
                            if (useTimerOnExit)
                                StartCoroutine(InvokeTimer(linkedEventOnExit, timer));
                            else
                                linkedEventOnExit.Invoke();
                            if (disableColliderOnTrigger)
                                TriggerCollider.enabled = false;
                        }
                        else if (LayerMask.LayerToName(other.gameObject.layer) == colliderLayer2 && useLayer2)
                        {
                            if (useTimerOnExit)
                                StartCoroutine(InvokeTimer(linkedEventOnExit, timer));
                            else
                                linkedEventOnExit.Invoke();
                            if (disableColliderOnTrigger)
                                TriggerCollider.enabled = false;
                        }
                    }
                }
                else
                {
                    if (other.tag == colliderTag && useTag1)
                    {
                        if (useTimerOnExit)
                            StartCoroutine(InvokeTimer(linkedEvent, timer));
                        else
                            linkedEvent.Invoke();
                        if (disableColliderOnTrigger)
                            TriggerCollider.enabled = false;
                    }
                    else if (other.tag == colliderTag2 && useTag2)
                    {
                        if (useTimerOnExit)
                            StartCoroutine(InvokeTimer(linkedEvent, timer));
                        else
                            linkedEvent.Invoke();
                        if (disableColliderOnTrigger)
                            TriggerCollider.enabled = false;
                    }
                    else if (LayerMask.LayerToName(other.gameObject.layer) == colliderLayer && useLayer1)
                    {
                        if (useTimerOnExit)
                            StartCoroutine(InvokeTimer(linkedEvent, timer));
                        else
                            linkedEvent.Invoke();
                        if (disableColliderOnTrigger)
                            TriggerCollider.enabled = false;
                    }
                    else if (LayerMask.LayerToName(other.gameObject.layer) == colliderLayer2 && useLayer2)
                    {
                        if (useTimerOnExit)
                            StartCoroutine(InvokeTimer(linkedEvent, timer));
                        else
                            linkedEvent.Invoke();
                        if (disableColliderOnTrigger)
                            TriggerCollider.enabled = false;
                    }
                    
                }
            }
        }
    }
    private IEnumerator InvokeTimer(UnityEvent uEvent, float timer)
    {
        yield return new WaitForSeconds(timer);
        uEvent.Invoke();
    }
}
