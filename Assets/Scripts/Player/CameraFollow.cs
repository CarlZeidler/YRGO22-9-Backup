using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //TODO move to gamemanager
    [SerializeField] private Transform player;
    public float lerpSpeed = 1;


    void Start()
    {
        player = FindObjectOfType<PlayerMove>().transform;
    }

    void Update()
    {
        bool panning;
        float maxScreenPoint = 0.9f;
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
            panning = false;
        }
        Vector3 position = Vector2.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);
        position = new Vector3(position.x, position.y, -10);

        //smoother follow when not panning camera
        if((transform.position-targetPosition).magnitude > 30&!panning)
            transform.position = position;
        else
            transform.position = position;
    }
}
