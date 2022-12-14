using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerHack : MonoBehaviour
{
    [SerializeField] AudioSource hackerVision, commit;
    public AudioSource select;

    public bool inHackingMode;
    public int maxBatteryCharges = 10;
    private int _batteryCharges = 10;
    public int batteryCharges 
    {
        get { return _batteryCharges; } 
        set 
        {
            //update UI slider on value change
            Mathf.Clamp(value, 0, maxBatteryCharges);
            _batteryCharges = value;
            batteryChargeSlider.value = value;

            //update music reference
            try
            {
                GameManager.instance.composer.chargesSpent = maxBatteryCharges - value;
            }
            catch { }
        } 
    }

    [SerializeField] private Animator hackingUIAnim;
    [SerializeField] private Slider batteryChargeSlider;

    private void Start()
    {
        //default values on slider
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
        {
            foreach (var hackableObject in GameManager.instance.hackableObjects)
            {
                hackableObject.CommitHack();
            }
            if (inHackingMode)
            {
                ToggleHackingMode();
                commit.Play();
            }
        }    

    }
    public void ToggleHackingMode()
    {
        if (!inHackingMode)
        {
            hackerVision.Play();
            GameManager.instance.RevealHackables(.015f);
            hackingUIAnim.SetBool("HackingMode", true);
            Time.timeScale = 0.1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;
        }
        else
        {
            GameManager.instance.HideHackables(0.015f);
            hackingUIAnim.SetBool("HackingMode", false);
            Time.timeScale = 1f;
        }
        inHackingMode = !inHackingMode;
    }
    public void DrainCharge(int amount)
    {
        batteryCharges -= amount;
    }
    public void ResetCharges()
    {
        batteryCharges = maxBatteryCharges;
    }
}
