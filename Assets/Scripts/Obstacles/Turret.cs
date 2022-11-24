using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : HackableObjects
{
    [SerializeField] private float killTime = 1f;

    private Animator animator;
    [SerializeField] private SpriteRenderer detectionAreaSprite;
    [SerializeField] private Color detectionColor,activeColor,inactiveColor;

    public void Active(bool active)
    {
        if (active)
            detectionAreaSprite.color = activeColor;
        else
            detectionAreaSprite.color = inactiveColor;
    }
    public void OnPlayerEnter()
    {
        detectionAreaSprite.color = detectionColor;
        Invoke(nameof(Death), killTime);
    }
    public void OnPlayerExit()
    {
        detectionAreaSprite.color = activeColor;
        CancelInvoke(nameof(Death));
    }
    private void Death()
    {
        //respawn code here
        Debug.Log("turret fired");
    }
}
