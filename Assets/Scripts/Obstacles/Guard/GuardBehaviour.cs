using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardBehaviour : HackableObjects
{
    public GuardMove moveScript;
    public GuardVision visionScript;

    //The below functions can be interacted with through the hacking interface.
    public void Shutdown()
    {
        moveScript.canMove = false;
        visionScript.canSee = false;
        Invoke(nameof(ReActivated), hackingStrength);
    }

    public void Rooted()
    {
        moveScript.canMove = false;
        Invoke(nameof(ReActivated), hackingStrength);
    }

    public void Blinded()
    {
        visionScript.canSee = false;
        Invoke(nameof(ReActivated), hackingStrength);
    }

    public void ReActivated()
    {
        //Restore functionality when the hacking time is over.
        visionScript.canSee = true;
        moveScript.canMove = true;
    }
}
