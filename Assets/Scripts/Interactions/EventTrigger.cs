﻿using System.Collections;
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
    public LayerMask colliderLayer, colliderLayer2;
    public LayerMask ignoreLayer;

    public Collider2D TriggerCollider;
    public bool useTag1, useTag2, useLayer1, useLayer2;
    [Space]
    public bool disableColliderOnTrigger;
    public bool disableColliderOnTriggerExit;

    public bool triggerOnExit;
    public bool separateEventOnTriggerExit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (CheckIgnore(other))
        {
            if (other.tag == colliderTag&&useTag1)
            {
                linkedEvent.Invoke();
                if(disableColliderOnTrigger)
                    TriggerCollider.enabled = false;
            }
            else if (other.tag == colliderTag2 && useTag2)
            {
                linkedEvent.Invoke();
                if (disableColliderOnTrigger)
                    TriggerCollider.enabled = false;
            }
            else if(other.gameObject.layer == colliderLayer.value && useLayer1)
            {
                linkedEvent.Invoke();
                if (disableColliderOnTrigger)
                    TriggerCollider.enabled = false;
            }
            else if (other.gameObject.layer == colliderLayer2.value && useLayer2)
            {
                linkedEvent.Invoke();
                if (disableColliderOnTrigger)
                    TriggerCollider.enabled = false;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (CheckIgnore(other))
        {
            if (triggerOnExit)
            {
                if (separateEventOnTriggerExit)
                {
                    if (linkedEventOnExit!=null)
                    {
                        if (other.tag == colliderTag && useTag1)
                        {
                            linkedEventOnExit.Invoke();
                            if (disableColliderOnTrigger)
                                TriggerCollider.enabled = false;
                        }
                        else if (other.tag == colliderTag2 && useTag2)
                        {
                            linkedEventOnExit.Invoke();
                            if (disableColliderOnTrigger)
                                TriggerCollider.enabled = false;
                        }
                        else if (other.gameObject.layer == colliderLayer.value && useLayer1)
                        {
                            linkedEventOnExit.Invoke();
                            if (disableColliderOnTrigger)
                                TriggerCollider.enabled = false;
                        }
                        else if (other.gameObject.layer == colliderLayer2.value && useLayer2)
                        {
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
                        linkedEvent.Invoke();
                        if (disableColliderOnTrigger)
                            TriggerCollider.enabled = false;
                    }
                    else if (other.tag == colliderTag2 && useTag2)
                    {
                        linkedEvent.Invoke();
                        if (disableColliderOnTrigger)
                            TriggerCollider.enabled = false;
                    }
                    else if (other.gameObject.layer == colliderLayer.value && useLayer1)
                    {
                        linkedEvent.Invoke();
                        if (disableColliderOnTrigger)
                            TriggerCollider.enabled = false;
                    }
                    else if (other.gameObject.layer == colliderLayer2.value && useLayer2)
                    {
                        linkedEvent.Invoke();
                        if (disableColliderOnTrigger)
                            TriggerCollider.enabled = false;
                    }
                    
                }
            }
        }
    }
    private bool CheckIgnore(Collider2D other)
    {
        if (other.gameObject.layer != ignoreLayer.value)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
