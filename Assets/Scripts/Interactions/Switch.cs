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
            if (hackable.objectState == ObjectState.bluePersistent)
                hackerLines[linkedHackables.IndexOf(hackable)].GetComponent<Renderer>().material = lineBlue;
            else
                hackerLines[linkedHackables.IndexOf(hackable)].GetComponent<Renderer>().material = lineRed;
        }
    }
    public void Toggle()
    {
        if (toggled)
        {
            GetComponent<SpriteRenderer>().sprite = active;
            GetComponentInChildren<Image>().sprite = active;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = inactive;
            GetComponentInChildren<Image>().sprite = inactive;
        }
        toggled = !toggled;
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
    public void ResetToggle()
    {
        if(objectState == ObjectState.redUnPersistent)
        {
            if (toggled)
            {
                toggled = false;
                if (GetComponent<Interactable>())
                    GetComponent<Interactable>().linkedEvent.Invoke();
            }
        }
    }
}
