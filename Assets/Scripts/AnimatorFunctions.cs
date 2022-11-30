using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip clip1;

    public void PlaySound()
    {
        if(!source.isPlaying)
            source.Play();
        else
        {
            source.Stop();
            source.Play();
        }
    }
}
