using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Switch : HackableObjects
{
    [Space(20)]
    public List<HackableObjects> linkedHackables;
    public List<HackerLineConnection> hackerLines;
    [SerializeField] private GameObject hackerLinePrefab;
    [SerializeField] private Sprite active, inactive;
    [SerializeField] private Material lineMaterial;

    private bool toggled;
    [SerializeField] private bool useDiagonalLines;
    [SerializeField] private Transform[] customConnectionMiddleLinks;
    [Space(20)]
    [SerializeField] private SpriteRenderer normal, highlight;
    [SerializeField] private AudioSource toggle;


    [SerializeField] public float lineOffset;
    private void Start()
    {
        UpdateHackableLinks();
        ToggleVisualConnections(false);


        //hackable Start();
        //add to manager list
        GameManager.instance.hackableObjects.Add(this);
    }
    public void UpdateHackableLinks()
    {
        //clear list
        foreach (var line in hackerLines)
        {
            Destroy(line.gameObject);
        }
        hackerLines.Clear();

        //spawn new hacker line and set connection points to linked hackable
        foreach (var hackable in linkedHackables)
        {
            hackerLines.Add(Instantiate(hackerLinePrefab, transform).GetComponent<HackerLineConnection>());
            if(customConnectionMiddleLinks.Length > 0)
            {
                //extra space for start and endpoints
                Vector3[] positions = new Vector3[customConnectionMiddleLinks.Length + 2];

                //start and end should always be connections to objects
                positions[0] = transform.position;
                positions[positions.Length-1] = hackable.transform.position;

                //set positions in line exapt for first and last    
                for (int i = 1; i < positions.Length-1; i++)
                {
                    positions[i] = customConnectionMiddleLinks[i-1].position;
                }
                hackerLines[linkedHackables.IndexOf(hackable)].UpdateLine(positions);
            }
            else
            {
                //startpoint, 90 degree midpoint, endpoint
                float X, Y;
                if (transform.position.x > hackable.transform.position.x)
                    X = -lineOffset;
                else
                    X = lineOffset;
                if (transform.position.y > hackable.transform.position.y)
                    Y = lineOffset;
                else
                    Y = -lineOffset;

                Vector3[] positions = new Vector3[] { transform.position, new Vector3(transform.position.x+X, hackable.transform.position.y+Y, 0), hackable.transform.position };

                hackerLines[linkedHackables.IndexOf(hackable)].diagonal = useDiagonalLines;
                hackerLines[linkedHackables.IndexOf(hackable)].UpdateLine(positions);
            }

            hackerLines[linkedHackables.IndexOf(hackable)].GetComponent<Renderer>().material = lineMaterial;
        }
    }
    public void Toggle()
    {
        toggle.Play();
        if (toggled)
        {
            normal.sprite = active;
            highlight.sprite = active;
            //GetComponentInChildren<Image>().sprite = active;
        }
        else
        {
            normal.sprite = inactive;
            highlight.sprite = inactive;
            //GetComponentInChildren<Image>().sprite = inactive;
        }
        toggled = !toggled;
        foreach (var hackable in linkedHackables)
        {
            try
            {
                hackable.ToggleHackState();
            }
            catch
            {
                Debug.Log("Missing hackable links");
                Debug.Log(gameObject);
            }
        }
    }
    public void ToggleVisualConnections(bool enable)
    {
        UpdateHackableLinks();
        foreach (var line in hackerLines)
        {
            //line.enabled = !line.enabled;
            line.GetComponent<Renderer>().enabled = enable;
        }
    }
    public void ResetToggle()
    {
        if(objectState == ObjectState.redUnPersistent||objectState==ObjectState.greenSemiPersistent)
        {
            if (toggled &! isHacked)
            {
                if (GetComponent<Interactable>())
                    GetComponent<Interactable>().linkedEvent.Invoke();
            }
        }
    }
}
