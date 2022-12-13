using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : HackableObjects
{
    public GameObject StartPointRef;
    public GameObject EndPointRef;
    public GameObject laserSpark;
    private Animator thisAnimator;
    private EdgeCollider2D laserCollider;

    private LineRenderer lineRenderer;

    private Vector2 startPoint;
    private Vector2 endPoint;

    [SerializeField] private LayerMask ignorLayers;
    private RaycastHit2D ray;
    private Vector2 rayPointRef;

    [SerializeField] bool isActive;

    void Start()
    {
        if (!isActive)
        {
            Invoke(nameof(ToggleHackState), 0.1f);
        }
        //save own state if red spawn this on player respawn
        if (objectState == ObjectState.redUnPersistent)
        {
            originalState = gameObject;
        }
        //add to manager list
        GameManager.instance.hackableObjects.Add(this);


        lineRenderer = GetComponent<LineRenderer>();
        thisAnimator = GetComponentInChildren<Animator>();
        laserCollider = GetComponent<EdgeCollider2D>();

    }
    private void Update()
    {
        startPoint = StartPointRef.transform.position;
        endPoint = EndPointRef.transform.position;
        LaserCheck();
    }
    private void LaserCheck()
    {
        //ray from start to direction of end
        ray = Physics2D.Raycast(startPoint, endPoint - startPoint, 100, ~ignorLayers);

        //check if point moved
        if(ray && ray.point != rayPointRef)
        {
            rayPointRef = ray.point;
            DrawLaserLine();
        }
        else if(!ray)
        {
            Vector2 pos = ((endPoint - startPoint)) * 100;
            if (rayPointRef != pos)
            {
                rayPointRef = pos;
                DrawLaserLine();
            }
        }
    }
    private void DrawLaserLine()
    {
        //ray origin is in worldspace, linerenderer positions are not
        //line pos0 != startpoint worldpos
        lineRenderer.SetPosition(0, StartPointRef.transform.localPosition);
        //ray point - endpoint worldposition+worldpoint refrence,magic numbers
        lineRenderer.SetPosition(1, rayPointRef-endPoint+ (Vector2)EndPointRef.transform.localPosition);
        //Places laserSpark on raypoint position
        laserSpark.transform.position = new Vector2(rayPointRef.x, rayPointRef.y);

        //List<Vector2> edges = new List<Vector2>();
        //edges.Add(startPoint);
        //edges.Add(rayPointRef);
        //laserCollider.SetPoints(edges);
        if (ray && isActive)
            if (ray.collider.CompareTag("Player"))
                ray.collider.GetComponent<PlayerRespawn>().Die();
            else if (ray.collider.CompareTag("Enemy"))
            {
                if(ray.collider.GetComponent<GuardBehaviour>().objectState != ObjectState.bluePersistent)
                ray.collider.GetComponent<GuardRespawn>().Respawn();
            }
    }

    private void LaserStatus()
    {
        if (!isHacked)
        {
            thisAnimator.SetBool("Active", true);
        }
        else if (isHacked)
        {
            thisAnimator.SetBool("Active", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&isActive)
        {
            collision.GetComponent<PlayerRespawn>().Die();
        }
    }

    public void DisableBeam()
    {
        //isHacked = true;
        thisAnimator.SetTrigger("Deactive");
        isActive = false;
    }

    public void Reactivated()
    {
        //isHacked = false;
        thisAnimator.SetTrigger("Active");
        isActive = true;
    }
}
