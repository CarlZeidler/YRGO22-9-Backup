using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //TODO move to gamemanager
    [SerializeField] private Transform player;
    public float lerpSpeed = 1;
    public float maxScreenPoint = 0.9f;
    private float size;

    [SerializeField] private float hackermodeZoomMultiplier = 1;
    [SerializeField] private float hackermodeZoomSpeed = 1;

    private Camera camRef;
    private PlayerHack pHack;
    private IEnumerator ienHolder;

    void Start()
    {
        player = GameManager.instance.player.transform;
        size = GetComponent<Camera>().orthographicSize;
        pHack = player.GetComponent<PlayerHack>();
        camRef = GetComponent<Camera>();
        ienHolder = LerpOrthSize(size * hackermodeZoomMultiplier, hackermodeZoomSpeed);
    }

    void Update()
    {
        bool panning;
        //some magic numbers that set position towards mouseposition
        Vector3 mousePos = Input.mousePosition * maxScreenPoint + new Vector3(Screen.width, Screen.height, 0f) * ((1f - maxScreenPoint) * 0.5f);
        Vector3 targetPosition;

        if (Input.GetButton("CameraPan"))
        {
            targetPosition = (Camera.main.ScreenToWorldPoint(mousePos)/2)+player.position/2;
            panning = true;
        }
        else
        {
            targetPosition = player.position;
            targetPosition += Vector3.up * GetComponent<Camera>().orthographicSize/4;
            
            panning = false;
        }
        //position to move to
        Vector3 position = Vector2.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);
        position = new Vector3(position.x, position.y, -10);

        //smoother follow when not panning camera
        if((transform.position-targetPosition).magnitude > 30&!panning)
            transform.position = position;
        else
            transform.position = position;


        if (Input.GetButtonDown("ToggleHackingMode"))
        {
            ToggleCamZoom();
        }
    }
    public void ToggleCamZoom()
    {
        if (pHack.inHackingMode)
        {
            StopCoroutine(ienHolder);
            ienHolder = LerpOrthSize(size * hackermodeZoomMultiplier, hackermodeZoomSpeed);
            StartCoroutine(ienHolder);
        }
        else
        {
            StopCoroutine(ienHolder);
            ienHolder = LerpOrthSize(size, hackermodeZoomSpeed);
            StartCoroutine(ienHolder);
        }
    }
    IEnumerator LerpOrthSize(float endSize, float duration)
    {
        float time = 0;
        float startSize = camRef.orthographicSize;

        while (time < duration)
        {
            camRef.orthographicSize = Mathf.Lerp(startSize, endSize, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        camRef.orthographicSize = endSize;
    }
}
