using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ClickableObject : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent m1Click, m2Click;
    public bool clickable = false;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickable)
        {
            //TODO ADD COLORFADES ON CLICK
            if (eventData.button == PointerEventData.InputButton.Left)
                m1Click.Invoke();
            else if (eventData.button == PointerEventData.InputButton.Right)
                m2Click.Invoke();
        }
    }
    public void SetClickable(bool clickable)
    {
        this.clickable = clickable;
    }
}
