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
    bool panning;

    [SerializeField] private float hackermodeZoomMultiplier = 1;
    [SerializeField] private float zoomSpeed = 1;
    [SerializeField] private float forwardView = 1;
    [SerializeField] private float verticalView = 1;

    Vector3 targetPosition;
    Vector3 mousePos;
    private Camera camRef;
    private PlayerHack pHack;
    private IEnumerator ienHolder;

    void Start()
    {
        player = GameManager.instance.player.transform;
        size = GetComponent<Camera>().orthographicSize;
        pHack = player.GetComponent<PlayerHack>();
        camRef = GetComponent<Camera>();
        ienHolder = LerpOrthSize(size * hackermodeZoomMultiplier, zoomSpeed);
    }

    void Update()
    {
       

        if (Input.GetButton("CameraPan"))
        {
            targetPosition = (Camera.main.ScreenToWorldPoint(mousePos)/2)+player.position/2;
            panning = true;
        }
        else
        {
            targetPosition = player.position+(Vector3.right * Input.GetAxis("Horizontal Camera") * forwardView) + Vector3.up * player.GetComponent<Rigidbody2D>().velocity.normalized.y * verticalView;
            targetPosition += Vector3.up * GetComponent<Camera>().orthographicSize/4;
            
            panning = false;
        }
       


        if (Input.GetButtonDown("ToggleHackingMode"))
        {
            ToggleCamZoom();
        }
    }
    private void FixedUpdate()
    {
        //some magic numbers that set position towards mouseposition
        mousePos = Input.mousePosition * maxScreenPoint + new Vector3(Screen.width, Screen.height, 0f) * ((1f - maxScreenPoint) * 0.5f);

        //position to move to
        Vector3 position = Vector2.Lerp(transform.position, targetPosition, lerpSpeed * Time.unscaledDeltaTime);
        position = new Vector3(position.x, position.y, -10);

        //smoother follow when not panning camera
        if ((transform.position - targetPosition).magnitude > 30 & !panning)
            transform.position = position;
        else
            transform.position = position;

    }
    public void ToggleCamZoom()
    {
        if (pHack.inHackingMode)
        {
            StopCoroutine(ienHolder);
            ienHolder = LerpOrthSize(size * hackermodeZoomMultiplier, zoomSpeed);
            StartCoroutine(ienHolder);
        }
        else
        {
            StopCoroutine(ienHolder);
            ienHolder = LerpOrthSize(size, zoomSpeed);
            StartCoroutine(ienHolder);
        }
    }
    public void LerpCamZoom(float orthSize)
    {
        StartCoroutine(LerpOrthSize(orthSize, zoomSpeed));
    }
    IEnumerator LerpOrthSize(float endSize, float duration)
    {
        float time = 0;
        float startSize = camRef.orthographicSize;

        while (time < duration)
        {
            camRef.orthographicSize = Mathf.Lerp(startSize, endSize, time / duration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        camRef.orthographicSize = endSize;
    }
}
