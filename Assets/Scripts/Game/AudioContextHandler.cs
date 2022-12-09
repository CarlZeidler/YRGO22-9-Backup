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
        Invoke(nameof(EnableCollider), .5f);
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
        float distance = composer.maxDangerDistance;

        foreach (var hackable in dangerHackables)
        {
            float newDist = (transform.position - hackable.transform.position).magnitude;
            if (newDist < distance)
                distance = newDist;
        }

        if (distance < composer.maxDangerDistance)
        {
            // Debug.Log(distance * -1 + 25);
            composer.dangerDistance = distance * -1 + 25;               
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (safeHackables.Contains(collision.transform.root.GetComponentInChildren<HackableObjects>()))
        {
            composer.nearbyHackablesAmount++;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (safeHackables.Contains(collision.transform.root.GetComponentInChildren<HackableObjects>()))
        {
            composer.nearbyHackablesAmount--;
        }
    }
    //make sure all items have been added to list before checking collisions
    private void EnableCollider()
    {
        GetComponent<Collider2D>().enabled = true;
    }
}
