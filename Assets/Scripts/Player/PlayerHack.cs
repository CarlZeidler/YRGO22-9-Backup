using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerHack : MonoBehaviour
{
    public bool inHackingMode;
    public int maxBatteryCharges = 10;
    private int _batteryCharges = 10;
    public int batteryCharges 
    {
        get { return _batteryCharges; } 
        set 
        {
            Mathf.Clamp(value, 0, maxBatteryCharges);
            _batteryCharges = value;
            batteryChargeSlider.value = value;
        } 
    }

    [SerializeField] private Animator hackingUIAnim;
    [SerializeField] private Slider batteryChargeSlider;

    private void Start()
    {
        batteryChargeSlider.maxValue = batteryCharges;
        batteryChargeSlider.value = batteryCharges;
    }

    private void Update()
    {
        if (Input.GetButtonDown("ToggleHackingMode"))
        {
            ToggleHackingMode();
        }
        if (Input.GetButtonDown("CommitHack"))
            if (inHackingMode)
            {
                foreach (var hackableObject in GameManager.instance.hackableObjects)
                {
                    hackableObject.CommitHack();
                }             
            }
    }
    private void ToggleHackingMode()
    {
        if (!inHackingMode)
        {
            GameManager.instance.RevealHackables(0.5f);
            hackingUIAnim.SetBool("HackingMode",true);
        }
        else
        {
            GameManager.instance.HideHackables(0.1f);
            hackingUIAnim.SetBool("HackingMode", false);
        }
        inHackingMode = !inHackingMode;
    }
    public void DrainCharge(int amount)
    {
        batteryCharges -= amount;
    }
}
