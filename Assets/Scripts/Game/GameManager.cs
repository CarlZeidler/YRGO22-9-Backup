using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public GameObject player;
    public Composer composer;
    public Conductor conductor;
    public List<HackableObjects> hackableObjects = new List<HackableObjects>();
    public List<BatteryPickup> batteries = new List<BatteryPickup>();
    public List<RespawnPoint> respawnPoints = new List<RespawnPoint>();
    public HackableObjects selectedHackable;
    public int preparedCharge;

    [SerializeField]public Camera overlayCam;
    public GameObject postFx1, postFX2;

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
    public struct Stats
    {
        public float time;
        public int batterySpent, nrOfHacks, nrOfRespawns;
    }
    public Stats stats;

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
        try
        {
            player = FindObjectOfType<PlayerMove>().gameObject;
        }
        catch { }
        try
        {
            composer = FindObjectOfType<Composer>();
            conductor = FindObjectOfType<Conductor>();
            composer.ResetVolume();
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
            if (hackable.GetComponent<GuardRespawn>())
            {
                if(hackable.GetComponent<GuardBehaviour>().objectState == HackableObjects.ObjectState.bluePersistent)
                {
                    if(hackable.GetComponent<GuardRespawn>().dead)
                        hackable.GetComponent<GuardRespawn>().Respawn();
                }
                else
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
    public void ResetStats()
    {
        stats.batterySpent = 0;
        stats.nrOfHacks = 0;
        stats.nrOfRespawns = 0;
        stats.time = 0;
    }
    public void SaveStats()
    {
        PlayerPrefs.SetFloat("time", stats.time);
        PlayerPrefs.SetInt("battery", stats.batterySpent);
        PlayerPrefs.SetInt("respawns", stats.nrOfRespawns);
        PlayerPrefs.SetInt("hacks", stats.nrOfHacks);
    }
    public void ReadStats()
    {
        stats.time=PlayerPrefs.GetFloat("time");
        stats.batterySpent = PlayerPrefs.GetInt("battery");
        stats.nrOfRespawns = PlayerPrefs.GetInt("respawns");
        stats.nrOfHacks = PlayerPrefs.GetInt("hacks");
    }

    public void TurnOffRespawnLights()
    {
        foreach (var respawn in respawnPoints)
        {
            respawn.TurnOffLight();
        }
    }
}
