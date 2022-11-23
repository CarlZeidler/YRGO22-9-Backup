using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHack : MonoBehaviour
{
    public bool inHackingMode;
    [SerializeField] private Animator hackingUIAnim;

    private void Update()
    {
        if (Input.GetButtonDown("ToggleHackingMode"))
        {
            ToggleHackingMode();
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
}
