using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorFunctions : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip clip1;

    [SerializeField] UnityEvent linkedEvent;

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
    public void InvokeEvent()
    {
        linkedEvent.Invoke();
    }
}
