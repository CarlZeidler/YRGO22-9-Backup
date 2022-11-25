using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : HackableObjects
{
    public GameObject StartPointRef;
    public GameObject EndPointRef;
    public GameObject LaserBeamPrefab;
    private Animator thisAnimator;
    private EdgeCollider2D laserCollider;

    private LineRenderer lineRenderer;

    private Vector2 startPoint;
    private Vector2 endPoint;

    public bool laserEnabled = true;

    void Start()
    {
        startPoint = StartPointRef.transform.localPosition;
        endPoint = EndPointRef.transform.localPosition;

        lineRenderer = GetComponent<LineRenderer>();
        thisAnimator = GetComponentInChildren<Animator>();
        laserCollider = GetComponent<EdgeCollider2D>();
        
        StartLaser();
    }

    void Update()
    {
        LaserStatus();
    }

    private void StartLaser()
    {
        Vector2 laserPos = new Vector2(startPoint.x + (endPoint.x - startPoint.x) / 2, startPoint.y + (endPoint.y - startPoint.y) / 2);

        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);

        List<Vector2> edges = new List<Vector2>();
        edges.Add(startPoint);
        edges.Add(endPoint);

        laserCollider.SetPoints(edges);

    }

    private void LaserStatus()
    {
        if (laserEnabled)
        {
            thisAnimator.SetBool("Active", true);
        }
        else if (!laserEnabled)
        {
            thisAnimator.SetBool("Active", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Do something when triggered.
    }

    public void DisableBeam()
    {
        laserEnabled = false;
        Invoke(nameof(Reactivated), hackingStrength);
    }

    private void Reactivated()
    {
        laserEnabled = true;
    }
}
