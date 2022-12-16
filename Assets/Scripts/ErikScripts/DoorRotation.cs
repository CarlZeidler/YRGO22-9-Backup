using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRotation : HackableObjects
{
    [SerializeField] private float speed = 1;
    [SerializeField] private Animator anim;
    [SerializeField] AudioSource open;

    public void OpenDoor(bool open) 
    {
        this.open.Play();
        anim.speed = speed;
        if (open)
        {
            anim.SetTrigger("Open");
        }
        else
        {
            anim.SetTrigger("Close");
        }
    }
}

