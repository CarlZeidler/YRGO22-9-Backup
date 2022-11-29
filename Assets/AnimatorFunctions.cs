using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip clip1;

    public void PlaySound()
    {
        source.Play();
    }
}
