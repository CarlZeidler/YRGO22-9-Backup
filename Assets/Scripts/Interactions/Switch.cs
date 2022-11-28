using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : HackableObjects
{
    [Space(20)]
    public List<HackableObjects> linkedHackables;
    public List<HackerLineConnection> hackerLines;
    [SerializeField] private GameObject hackerLinePrefab;

    private void Start()
    {
        UpdateHackableLinks();
        ToggleVisualConnections();


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
            hackerLines[linkedHackables.IndexOf(hackable)].UpdateLine(transform.position, new Vector3(transform.position.x,hackable.transform.position.y, 0) ,hackable.transform.position);
        }
    }
    public void Toggle()
    {
        foreach (var hackable in linkedHackables)
        {
            hackable.ToggleHackState();
        }
    }
    public void ToggleVisualConnections()
    {
        foreach (var line in hackerLines)
        {
            //line.enabled = !line.enabled;
            line.GetComponent<Renderer>().enabled = !line.GetComponent<Renderer>().enabled;
        }
    }
}
