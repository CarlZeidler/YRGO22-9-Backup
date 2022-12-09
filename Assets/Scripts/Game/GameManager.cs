using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public GameObject player;
    public Composer composer;
    public List<HackableObjects> hackableObjects = new List<HackableObjects>();
    public List<BatteryPickup> batteries = new List<BatteryPickup>();
    public HackableObjects selectedHackable;

    private bool _somethingIsHacked;
     [HideInInspector]public bool somethingIsHacked
    {
        get 
        {
            bool isHacking = false;
            foreach (var hackable in hackableObjects)
            {
                if (hackable.isHacked)
                    isHacking = true;
            }
            return isHacking;
        }
        private set { _somethingIsHacked = value; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        player = FindObjectOfType<PlayerMove>().gameObject;
        try
        {
            composer = FindObjectOfType<Composer>();
        }
        catch { }
    }

    public void RevealHackables(float delay)
    {
        CancelInvoke(nameof(HideHackablesDelayed));
        CancelInvoke(nameof(RevealHackablesDelayed));
        Invoke(nameof(RevealHackablesDelayed), delay);

        try
        {
            composer.hackervision = true;
        }
        catch { }
    }
    public void HideHackables(float delay)
    {
        CancelInvoke(nameof(HideHackablesDelayed));
        CancelInvoke(nameof(RevealHackablesDelayed));
        Invoke(nameof(HideHackablesDelayed), delay);

        try
        {
            composer.hackervision = false;
        }
        catch { }
    }
    private void RevealHackablesDelayed()
    {
        foreach (var hackable in hackableObjects)
        {
            hackable.OnHackingModeReveal();
        }
    }
    private void HideHackablesDelayed()
    {
        foreach (var hackable in hackableObjects)
        {
            hackable.OnHackingModeHide();
        }
    }
    public void ResetHackables()
    {
        foreach (var hackable in hackableObjects)
        {
            hackable.ResetHackPower();
            hackable.ResetOnRespawn();
            if (hackable.GetComponent<Switch>())
            {
                hackable.GetComponent<Switch>().ResetToggle();
            }
            if (hackable.GetComponent<GuardRespawn>()&&hackable.objectState == HackableObjects.ObjectState.redUnPersistent)
            {
                hackable.GetComponent<GuardRespawn>().Respawn();
            }
        }
    }
    public void ResetPickups()
    {
        foreach (var battery in batteries)
        {
            battery.Respawn();
        }
    }
    public void ChangeSelectedHackable(HackableObjects newSelection)
    {
        if(selectedHackable != null)
            selectedHackable.ToggleHackSelection(false);
        selectedHackable = newSelection;
    }

}
