using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerHack : MonoBehaviour
{
    [SerializeField] AudioSource hackerVision, commit;
    public AudioSource select;
    [SerializeField]private PostProcessToggler processToggler;

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
    [SerializeField] private Animator hackingControlsAnim;
    [SerializeField] private Slider batteryChargeSlider;
    [HideInInspector] public int somethingIsHacked = 0;

    private void Start()
    {
        //default values on slider
        batteryChargeSlider.maxValue = batteryCharges;
        batteryChargeSlider.value = batteryCharges;

        Invoke(nameof(OnStart), 0.1f);
    }
    void OnStart()
    {
        processToggler = Camera.main.GetComponent<PostProcessToggler>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("ToggleHackingMode"))
        {
            ToggleHackingMode();
        }
        if (Input.GetButtonDown("CommitHack"))
        {
            //reference to how much bettery will be used
            GameManager.instance.preparedCharge = 0;

            hackingControlsAnim.SetBool("HackingMode", false);
            somethingIsHacked = 0;
            foreach (var hackableObject in GameManager.instance.hackableObjects)
            {
                hackableObject.CommitHack();
            }
            if (inHackingMode)
            {
                if (GameManager.instance.somethingIsHacked)
                {
                    ToggleHackingMode();
                    commit.Play();
                }
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

            if (GameManager.instance.preparedCharge > 0)
                hackingControlsAnim.SetBool("HackingMode", true);

            Time.timeScale = 0.1f;
            Time.fixedDeltaTime = Time.timeScale * .02f;

            processToggler.ToggleHackerFX(true);
        }
        else
        {
            GameManager.instance.HideHackables(0.015f);
            hackingUIAnim.SetBool("HackingMode", false);
            foreach (var hackableObject in GameManager.instance.hackableObjects)
            {
                if (hackableObject.hackingStrength != 0)
                {
                    somethingIsHacked++;
                    break;
                }
            }
            if (somethingIsHacked == 0)
            {
                hackingControlsAnim.SetBool("HackingMode", false);
            }
            Time.timeScale = 1f;
            processToggler.ToggleHackerFX(false);
        }
        inHackingMode = !inHackingMode;
    }

    public void ToggleHackingUIRespawn()
    {
        hackingControlsAnim.SetBool("HackingMode", false);
    }

    public void DrainCharge(int amount)
    {
        batteryCharges -= amount;

        if (GameManager.instance.preparedCharge > 0)
            hackingControlsAnim.SetBool("HackingMode", true);
        else
            hackingControlsAnim.SetBool("HackingMode", false);
    }
    public void ResetCharges()
    {
        batteryCharges = maxBatteryCharges;
    }
}
