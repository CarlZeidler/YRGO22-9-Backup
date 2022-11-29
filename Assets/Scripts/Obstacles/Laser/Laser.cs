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

    void Start()
    {
        //save own state if red spawn this on player respawn
        if (objectState == ObjectState.redUnPersistent)
        {
            originalState = gameObject;
        }
        //add to manager list
        GameManager.instance.hackableObjects.Add(this);

        startPoint = StartPointRef.transform.localPosition;
        endPoint = EndPointRef.transform.localPosition;

        lineRenderer = GetComponent<LineRenderer>();
        thisAnimator = GetComponentInChildren<Animator>();
        laserCollider = GetComponent<EdgeCollider2D>();
        
        StartLaser();
    }

    private void StartLaser()
    {
        //Create laser with Line Render and adjust collider to span length of rendered line.

        Vector2 laserPos = new Vector2(startPoint.x + (endPoint.x - startPoint.x) / 2, startPoint.y + (endPoint.y - startPoint.y) / 2);

        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);

        List<Vector2> edges = new List<Vector2>();
        edges.Add(startPoint);
        edges.Add(endPoint);

        laserCollider.SetPoints(edges);

        StartPointRef.transform.up = EndPointRef.transform.position - StartPointRef.transform.position;
        EndPointRef.transform.up = StartPointRef.transform.position - EndPointRef.transform.position;

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
