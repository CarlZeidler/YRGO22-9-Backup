using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardBehaviour : HackableObjects
{
    public GuardMove moveScript;
    public GuardVision visionScript;

    public void SpottedPlayer()
    {
        //This function is called from GuardVisionScript when the player is spotted (vision cone triggered)
        //TODO: Add event for when player is spotted.
        Debug.Log("spotted");
    }

    //The below functions can be interacted with through the hacking interface.
    public void Shutdown(float hackingTime)
    {
        moveScript.canMove = false;
        visionScript.canSee = false;
        Invoke(nameof(ReActivated), hackingTime);
    }

    public void Rooted(float hackingTime)
    {
        moveScript.canMove = false;
        Invoke(nameof(ReActivated), hackingTime);
    }

    public void Blinded(float hackingTime)
    {
        visionScript.canSee = false;
        Invoke(nameof(ReActivated), hackingTime);
    }

    private void ReActivated()
    {
        //Restore functionality when the hacking time is over.
        visionScript.canSee = true;
        moveScript.canMove = true;
    }
}
