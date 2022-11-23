using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HackableObjects : MonoBehaviour
{
    public UnityEvent onHackEvent, onHackRevealEvent, onHackHideEvent;
    private GameObject originalState;
    public enum ObjectState
    {
        bluePersistent, redUnPersistent
    }
    public ObjectState objectState;

    public int hackingStrength = 0;


    private void Start()
    {
        //save own state if red spawn this on player respawn
        if(objectState == ObjectState.redUnPersistent)
        {
            originalState = gameObject;
        }
        //add to manager list
        GameManager.instance.hackableObjects.Add(this);
    }


    public void OnHackingModeReveal()
    {
        onHackRevealEvent.Invoke();
    }
    public void OnHackingModeHide()
    {
        onHackHideEvent.Invoke();
    }
    public void CommitHack()
    {
        onHackEvent.Invoke();
    }
    public void AddHackingPower(int amount)
    {
        hackingStrength += amount;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("HackingModeOverlay"))
    //    {
    //        OnHackingModeReveal();
    //    }
    //}
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("HackingModeOverlay"))
    //    {
    //        OnHackingModeHide();
    //    }
    //}
}
