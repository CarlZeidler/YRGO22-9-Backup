using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioContextHandler : MonoBehaviour
{
    public List<HackableObjects> dangerHackables = new List<HackableObjects>();
    public List<HackableObjects> safeHackables = new List<HackableObjects>();
    [SerializeField] private Composer composer;

    private void Start()
    {
        Invoke(nameof(UpdateLists),.5f);
    }
    public void UpdateLists()
    {
        dangerHackables.Clear();
        safeHackables.Clear();

        foreach (var hackable in GameManager.instance.hackableObjects)
        {
            if(hackable.GetComponent<GuardBehaviour>()|| hackable.GetComponent<Laser>() || hackable.GetComponent<Turret>())
            {
                dangerHackables.Add(hackable);
            }
        }
        foreach (var hackable in GameManager.instance.hackableObjects)
        {
            if (!hackable.GetComponent<GuardBehaviour>() && !hackable.GetComponent<Laser>() && !hackable.GetComponent<Turret>())
            {
                safeHackables.Add(hackable);
            }
        }
    }
    private void Update()
    {
        foreach (var hackable in dangerHackables)
        {
            float distance = (transform.position-hackable.transform.position).magnitude;
            if (distance < composer.maxDangerDistance)
            {
               // Debug.Log(distance * -1 + 25);
                composer.dangerDistance = distance * -1 + 25;               
            }
        }
    }
}
