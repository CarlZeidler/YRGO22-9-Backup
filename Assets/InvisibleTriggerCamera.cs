using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InvisibleTriggerCamera : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprt;
    [SerializeField] private EventTrigger eventScript;
    [SerializeField] private Transform cameraLockPos;
    [SerializeField] private Canvas parallaxCanvas;
    [Space]
    [SerializeField] private typeOfTrigger triggerType;
    [SerializeField] private float endSize;
    [SerializeField] private float duration;

    private float respawnHold;
    private CameraFollow cameraScript;
    private Camera mainCamera;
    private Vector3 cameraEndPos;
    [SerializeField] enum typeOfTrigger { FixedPosition, BackToNormal };

    private void Start()
    {
        sprt.enabled = false;
        cameraScript = FindObjectOfType<CameraFollow>();
        mainCamera = Camera.main;
        cameraEndPos = cameraLockPos.position;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab) && GameManager.instance.player.GetComponent<PlayerRespawn>().isDead)
        {
            cameraScript.lockPosition = false;
        }

        if (Input.GetKey(KeyCode.Tab) && respawnHold >= 0.5f)
        {
            respawnHold = 0f;
            cameraScript.lockPosition = false;
        }
        else if (Input.GetKey(KeyCode.Tab))
        {
            respawnHold += 1 * Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Tab))
            respawnHold = 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            RepositionCamera();
    }

    public void RepositionCamera()
    {
        if (triggerType == typeOfTrigger.FixedPosition)
        {
            cameraScript.lockPosition = true;
            parallaxCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            StartCoroutine(LockCameraPos(cameraEndPos, endSize, duration));
        }

        if (triggerType == typeOfTrigger.BackToNormal)
        {
            cameraScript.lockPosition = false;
            parallaxCanvas.renderMode = RenderMode.WorldSpace;
            cameraScript.ToggleCamZoom();
        }
    }

    IEnumerator LockCameraPos(Vector3 endPos, float endSize, float duration)
    {
        float time = 0;
        float startSize = mainCamera.orthographicSize;
        Vector3 startPos = mainCamera.transform.position;

        while (time < duration)
        {
            mainCamera.orthographicSize = Mathf.Lerp(startSize, endSize, time / duration);
            mainCamera.transform.position = Vector3.Lerp(startPos, endPos, time / duration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        mainCamera.orthographicSize = endSize;
    }
}