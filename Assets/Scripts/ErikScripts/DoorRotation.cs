using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRotation : HackableObjects
{
    [SerializeField] private float speed = 1;
    [SerializeField] private Animator anim;
    [SerializeField] private ParticleSystem hackedParticles;


    public void OpenDoor(bool open) 
    {
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

    public void HackedDoorParticles(bool hacked)
    {

        if (hacked)
        {
            hackedParticles.Play();
        }
        else
        {
            hackedParticles.Stop();
        }
    }
}

