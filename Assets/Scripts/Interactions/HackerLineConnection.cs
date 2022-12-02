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

    public void UpdateLine(Vector3 point1, Vector3 point2, Vector3 point3)
    {
        Vector3[] positions;
        if (!diagonal)
        {
            // line.positionCount = 3;
            GetComponent<LineRenderer>().positionCount = 3;
            positions = new Vector3[] { point3,point2,point1};
        }
        else
        {
            //line.positionCount = 2;
            GetComponent<LineRenderer>().positionCount = 2;
            positions = new Vector3[] { point3, point1 };
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
