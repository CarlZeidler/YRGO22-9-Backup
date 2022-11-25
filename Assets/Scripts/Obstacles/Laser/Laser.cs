using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject beamVisuals;
    public GameObject StartPointRef;
    public GameObject EndPointRef;

    private Vector2 startPoint;
    private Vector2 endPoint;

    void Start()
    {
        startPoint = StartPointRef.transform.position;
        endPoint = EndPointRef.transform.position;
    }

    void Update()
    {
        
    }

    public void disableBeam()
    {

    }

}
