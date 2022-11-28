using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerLineConnection : MonoBehaviour
{
    private LineRenderer line;
    [SerializeField] float animSpeed;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    public void UpdateLine(Transform point1, Transform point2)
    {
        Vector3[] positions = new Vector3[] { point1.position, point2.position };
        line.SetPositions(positions);
    }
    private void Update()
    {
        GetComponent<Renderer>().material.mainTextureOffset += new Vector2(animSpeed*Time.deltaTime, 0);

        if (GetComponent<Renderer>().material.mainTextureOffset.x > 1)
            GetComponent<Renderer>().material.mainTextureOffset = Vector2.zero;
    }
}
