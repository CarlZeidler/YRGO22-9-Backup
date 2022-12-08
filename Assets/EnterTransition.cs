using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTransition : MonoBehaviour
{
    [SerializeField] private Animator thisAnimator;
    [SerializeField] private GameObject transitionImage;

    private void Start()
    {
        transitionImage.SetActive(true);
    }
    private void Update()
    {
        Invoke(nameof(Entered), 1f);
    }

    private void Entered()
    {
        gameObject.SetActive(false);
    }
}
