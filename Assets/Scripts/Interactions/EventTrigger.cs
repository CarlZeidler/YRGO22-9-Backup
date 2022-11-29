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
    public LayerMask colliderLayer, colliderLayer2;
    public LayerMask ignoreLayer;

    public Collider2D TriggerCollider;
    [Space]
    public bool disableColliderOnTrigger;
    public bool disableColliderOnTriggerExit;

    public bool triggerOnExit;
    public bool separateEventOnTriggerExit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(colliderLayer.value);
        if (CheckIgnore(other))
        {
            if (other.tag == colliderTag)
            {
                linkedEvent.Invoke();
                if(disableColliderOnTrigger)
                    TriggerCollider.enabled = false;
            }
            else if (other.tag == colliderTag2)
            {
                linkedEvent.Invoke();
                if (disableColliderOnTrigger)
                    TriggerCollider.enabled = false;
            }
            else if(other.gameObject.layer == colliderLayer.value)
            {
                
                linkedEvent.Invoke();
                if (disableColliderOnTrigger)
                    TriggerCollider.enabled = false;
            }
            else if (other.gameObject.layer == colliderLayer2.value)
            {
                linkedEvent.Invoke();
                if (disableColliderOnTrigger)
                    TriggerCollider.enabled = false;
            }
            else if (colliderTag == null)
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
                        if (other.tag == colliderTag)
                        {
                            linkedEventOnExit.Invoke();
                            if (disableColliderOnTriggerExit)
                                TriggerCollider.enabled = false;
                        }
                        else if (other.tag == colliderTag2)
                        {
                            linkedEvent.Invoke();
                            if (disableColliderOnTrigger)
                                TriggerCollider.enabled = false;
                        }
                        else if (other.gameObject.layer == colliderLayer.value)
                        {
                            linkedEventOnExit.Invoke();
                            if (disableColliderOnTriggerExit)
                                TriggerCollider.enabled = false;
                        }
                        else if (other.gameObject.layer == colliderLayer2.value)
                        {
                            linkedEventOnExit.Invoke();
                            if (disableColliderOnTriggerExit)
                                TriggerCollider.enabled = false;
                        }
                        else if (colliderTag == null)
                        {
                            linkedEventOnExit.Invoke();
                            if (disableColliderOnTrigger)
                                TriggerCollider.enabled = false;
                        }
                    }
                }
                else
                {
                    if (other.tag == colliderTag)
                    {
                        linkedEventOnExit.Invoke();
                        if (disableColliderOnTriggerExit)
                            TriggerCollider.enabled = false;
                    }
                    else if (other.tag == colliderTag2)
                    {
                        linkedEvent.Invoke();
                        if (disableColliderOnTrigger)
                            TriggerCollider.enabled = false;
                    }
                    else if (other.gameObject.layer == colliderLayer.value)
                    {
                        linkedEventOnExit.Invoke();
                        if (disableColliderOnTriggerExit)
                            TriggerCollider.enabled = false;
                    }
                    else if (other.gameObject.layer == colliderLayer2.value)
                    {
                        linkedEventOnExit.Invoke();
                        if (disableColliderOnTriggerExit)
                            TriggerCollider.enabled = false;
                    }
                    else if (colliderTag == null)
                    {
                        linkedEventOnExit.Invoke();
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
