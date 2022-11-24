using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class HackableObjects : MonoBehaviour
{
    public bool isHacked = false;

    public UnityEvent onHackEvent,onHackStoppedEvent, onHackRevealEvent, onHackHideEvent;

    [SerializeField] private TextMeshProUGUI hackingPowerText;
    protected GameObject originalState;
    public enum ObjectState
    {
        bluePersistent, redUnPersistent
    }
    public ObjectState objectState;

    private int _hackingStrength = 0;
    public int hackingStrength 
    { 
        get { return _hackingStrength; } 
        set 
        { 
            _hackingStrength = value;
            if (value > 0)
                hackingPowerText.text = _hackingStrength.ToString();
            else
                hackingPowerText.text = null;
        } 
    }


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
        //if (!isHacked)
          //  ResetHackPower();
    }
    public void CommitHack()
    {
        if (!isHacked&&hackingStrength>0)
        {
            onHackEvent.Invoke();
            DrainHackingStrength();
            isHacked = true;
        }
    }
    public void AddHackingPower(int amount)
    {
        if (!isHacked)
        {
            if(amount > 0)
            {
                if(GameManager.instance.player.GetComponent<PlayerHack>().batteryCharges > 0)
                {
                    hackingStrength += amount;
                    GameManager.instance.player.GetComponent<PlayerHack>().DrainCharge(amount);
                }
                else
                {
                    //show error msg
                }
            }
            else
            {
                if (hackingStrength>0 && GameManager.instance.player.GetComponent<PlayerHack>().batteryCharges < 10)
                {
                    hackingStrength += amount;
                    GameManager.instance.player.GetComponent<PlayerHack>().DrainCharge(amount);
                }
            }
        }
    }
    public void ResetHackPower()
    {
        GameManager.instance.player.GetComponent<PlayerHack>().batteryCharges += hackingStrength;
        hackingStrength = 0;
    }

    //drain strength every second until 0
    private void DrainHackingStrength()
    {
        if(hackingStrength > 0)
        {
            hackingStrength--;
            Invoke(nameof(DrainHackingStrength), 1);
        }
        else
        {
            //stop hack
            isHacked = false;
            onHackStoppedEvent.Invoke();
        }
    }


}
