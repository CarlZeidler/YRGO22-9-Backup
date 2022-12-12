using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAnimationEvent : MonoBehaviour
{
    public GuardBehaviour behaviorsScript;

    public void ResumeBehaviorTrigger()
    {
        behaviorsScript.ResumeBehavior();
    }

}
