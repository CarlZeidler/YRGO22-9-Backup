using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class AnimatedClickableObject : ClickableObject, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent mouseExit, mouseEnter;

    public bool mouseEnterAnim = false;
    public Animator anim;
    [HideInInspector] public bool mouseInRect = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("enter");
        if (mouseEnterAnim)
        {
            mouseInRect = true;
            mouseEnter.Invoke();
            anim.speed = 1 / Time.timeScale;
            anim.SetTrigger("WobbleUp");
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("exit");
        if (mouseEnterAnim)
        {
            mouseExit.Invoke();
            mouseInRect = false;
            anim.speed = 1 / Time.timeScale;
            anim.SetTrigger("WobbleDown");
        }
    }
    private void Update()
    {
        //if (!rectTransform.rect.Contains(Input.mousePosition))
        //{
        //    OnPointerExit();
        //}
    }
}
