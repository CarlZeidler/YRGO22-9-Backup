using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : HackableObjects
{
    [Space(30)]
    [SerializeField] private float speed;
    [SerializeField] private float platFormSpacing;
    private bool reversed = false;

    [SerializeField] public Transform startPointTransform, endPointTransform;
    private Vector3 startPoint, endPoint;
    [SerializeField] private GameObject liftPlatform;

    private List<LiftPlatform> platforms =  new List<LiftPlatform>();
    [SerializeField]private float timer = 1;

    private void Start()
    {
        //sets references from inspector so they can be changed easier
        startPoint = startPointTransform.position;
        endPoint = endPointTransform.position;
        timer = platFormSpacing;

        //HackableObject.Start();
        //add to manager list
        GameManager.instance.hackableObjects.Add(this);
    }
    private void Update()
    {
        //spawn on timer
        if (!reversed)
        {
            if(timer <= 0)
            {
                SpawnPlatform();
                timer = platFormSpacing;
            }
            timer -= Time.deltaTime;
        }
        else
        {
            if (timer >= platFormSpacing)
            {
                SpawnPlatform();
                timer = 0;
            }
            timer += Time.deltaTime;
        }
    }
    public void SpawnPlatform()
    {
        //TODO spawn anim
        LiftPlatform platform = Instantiate(liftPlatform, startPoint, startPointTransform.rotation, transform).GetComponent<LiftPlatform>();
        platforms.Add(platform);
        platform.target = endPoint;
        platform.speed = speed;
    }
    public void Reverse()
    {
        reversed = !reversed;
        Vector3 holder = endPoint;
        endPoint = startPoint;
        startPoint = holder;

        foreach (var platform in platforms)
        {
            platform.target = endPoint;
        }
    }
}
