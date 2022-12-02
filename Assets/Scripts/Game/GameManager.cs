using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public GameObject player;
    public List<HackableObjects> hackableObjects = new List<HackableObjects>();

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
    }

    public void RevealHackables(float delay)
    {
        CancelInvoke(nameof(HideHackablesDelayed));
        CancelInvoke(nameof(RevealHackablesDelayed));
        Invoke(nameof(RevealHackablesDelayed), delay);
    }
    public void HideHackables(float delay)
    {
        CancelInvoke(nameof(HideHackablesDelayed));
        CancelInvoke(nameof(RevealHackablesDelayed));
        Invoke(nameof(HideHackablesDelayed), delay);
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


}
