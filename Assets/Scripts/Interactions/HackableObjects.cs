using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class HackableObjects : MonoBehaviour
{
    public bool isHacked = false;
    public ObjectState objectState;

    public UnityEvent onHackEvent,onHackStoppedEvent, onHackRevealEvent, onHackHideEvent;
    private UnityEvent holder;

    [SerializeField] private TextMeshProUGUI hackingPowerText;
    [SerializeField] private Slider hackingChargeSlider;
    [SerializeField] private GameObject selectionCircle;
    [SerializeField] private ParticleSystem hackedParticles;
    protected GameObject originalState;
    public enum ObjectState
    {
        bluePersistent, redUnPersistent, greenSemiPersistent
    }

    private int _hackingStrength = 0;
    public int hackingStrength 
    { 
        get { return _hackingStrength; } 
        set 
        { 
            _hackingStrength = value;
            if (value > 0)
            {
                hackingPowerText.text = _hackingStrength.ToString();
                try
                {
                    hackingChargeSlider.value = value;
                }
                catch { }
            }
            else
            {
                //TODO write 0 and invoke setactive
                hackingPowerText.text = _hackingStrength.ToString();
                try
                {
                    hackingChargeSlider.value = value;
                    if(!isHacked)
                        DrainHackingStrength();
                }
                catch { }
            }
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
        ToggleHackSelection(false);
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
            ToggleHackSelection(false);

            try
            {
                GameManager.instance.composer.HackInProgress();
            }
            catch { }
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
            if (isHacked)
            {
                isHacked = false;
                onHackStoppedEvent.Invoke();

                //try
                //{
                //    GameManager.instance.composer.hackInProgress = false;
                //}
                //catch { }

            }
            //function called on rightclick powerdrain
            hackingPowerText.transform.parent.gameObject.SetActive(false);
        }
    }
    public void ToggleHackState()
    {
        //create reference for greenobject
        if(this.holder == null)
            this.holder = onHackEvent;

        if (isHacked)
        {
            onHackStoppedEvent.Invoke();
        }
        else
        {
            onHackEvent.Invoke();
        }

        UnityEvent holder = onHackEvent;
        onHackEvent = onHackStoppedEvent;
        onHackStoppedEvent = holder;
    }
    public void ResetOnRespawn()
    {
        if(objectState == ObjectState.redUnPersistent)
        {
            if (isHacked)
            {
                hackingStrength = 0;
                //onHackStoppedEvent.Invoke();
            }
        } //toggle state if different from start
        else if(objectState == ObjectState.greenSemiPersistent)
        {
            if(holder != onHackEvent&&holder!=null)
            {
                ToggleHackState();
            }
        }
    }

    public void ToggleHackSelection(bool enabled)
    {
        try
        {
            if (enabled && GameManager.instance.selectedHackable != this)
                GameManager.instance.ChangeSelectedHackable(this);
            if (hackingStrength > 0)
                selectionCircle.SetActive(enabled);
            else
                selectionCircle.SetActive(false);
        }
        catch { }
    }

    public void HackedParticles(bool hacked)
    {

        if (hacked)
        {
            hackedParticles.Play();
        }
        else
        {
            hackedParticles.Stop();
        }
    }
}
