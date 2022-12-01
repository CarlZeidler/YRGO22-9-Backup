using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : HackableObjects
{
    public GameObject StartPointRef;
    public GameObject EndPointRef;
    private Animator thisAnimator;
    private EdgeCollider2D laserCollider;

    private LineRenderer lineRenderer;

    private Vector2 startPoint;
    private Vector2 endPoint;

    [SerializeField] private LayerMask ignorLayers;
    private RaycastHit2D ray;
    private Vector2 rayPointRef;

    void Start()
    {
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

        List<Vector2> edges = new List<Vector2>();
        edges.Add(startPoint);
        edges.Add(rayPointRef);
        laserCollider.SetPoints(edges);
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
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerRespawn>().Respawn();
        }
    }

    public void DisableBeam()
    {
        //isHacked = true;
        thisAnimator.SetTrigger("Deactive");
    }

    public void Reactivated()
    {
        //isHacked = false;
        thisAnimator.SetTrigger("Active");
    }
}
