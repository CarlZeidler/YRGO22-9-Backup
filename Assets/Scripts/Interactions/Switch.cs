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
    [SerializeField] private Material lineBlue, lineRed;

    private bool toggled;
    [SerializeField] private bool useDiagonalLines;
    [SerializeField] private Transform[] customConnectionMiddleLinks;
    [SerializeField] private SpriteRenderer normal, highlight;

    private void Start()
    {
        UpdateHackableLinks();
        ToggleVisualConnections(false);


        //hackable Start();
        //save own state if red spawn this on player respawn
        if (objectState == ObjectState.redUnPersistent)
        {
            originalState = gameObject;
        }
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
                Vector3[] positions = new Vector3[] { transform.position, new Vector3(transform.position.x, hackable.transform.position.y, 0), hackable.transform.position };

                hackerLines[linkedHackables.IndexOf(hackable)].diagonal = useDiagonalLines;
                hackerLines[linkedHackables.IndexOf(hackable)].UpdateLine(positions);
            }

            if (hackable.objectState == ObjectState.bluePersistent)
                hackerLines[linkedHackables.IndexOf(hackable)].GetComponent<Renderer>().material = lineBlue;
            else if(hackable.objectState == ObjectState.redUnPersistent)
                hackerLines[linkedHackables.IndexOf(hackable)].GetComponent<Renderer>().material = lineRed;
            //TODO else green
        }
    }
    public void Toggle()
    {
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
            }
        }
    }
    public void ToggleVisualConnections(bool enable)
    {
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
