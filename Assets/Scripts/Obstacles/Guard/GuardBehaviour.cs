using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardBehaviour : HackableObjects
{
    public GuardMove moveScript;
    public GuardVision visionScript;
    
    public void Shutdown()
    {
        moveScript.canMove = false;
        visionScript.canSee = false;
        moveScript.SetCharacterState("shutdown");
    }

    public void ReActivated()
    {
        //Restore functionality when the hacking time is over.
        visionScript.canSee = true;
        moveScript.canMove = true;
    }
}
