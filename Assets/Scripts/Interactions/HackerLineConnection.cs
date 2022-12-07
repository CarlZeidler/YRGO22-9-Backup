using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerLineConnection : MonoBehaviour
{
    private LineRenderer line;
    [SerializeField] float animSpeed;
    public bool diagonal = false;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    public void UpdateLine(Vector3[] positions)
    {
        GetComponent<LineRenderer>().positionCount = positions.Length;
        if (diagonal)
        {
            //line.positionCount = 2;
            GetComponent<LineRenderer>().positionCount = 2;
            positions = new Vector3[] { positions[positions.Length-1], positions[0] };
        }
        GetComponent<LineRenderer>().SetPositions(positions);
    }
    private void Update()
    {
        GetComponent<Renderer>().material.mainTextureOffset += new Vector2(animSpeed*Time.deltaTime, 0);

        if (GetComponent<Renderer>().material.mainTextureOffset.x > 1)
            GetComponent<Renderer>().material.mainTextureOffset = Vector2.zero;
    }
}
