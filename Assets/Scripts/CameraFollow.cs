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
        float maxScreenPoint = 0.9f;
        Vector3 mousePos = Input.mousePosition * maxScreenPoint + new Vector3(Screen.width, Screen.height, 0f) * ((1f - maxScreenPoint) * 0.5f);
        Vector3 targetPosition;

        if (Input.GetButton("CameraPan"))
        {
            targetPosition = (Camera.main.ScreenToWorldPoint(mousePos)/2)+player.position/2;
        }
        else
        {
            targetPosition = player.position;
        }
        Vector3 position = Vector2.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);
        position = new Vector3(position.x, position.y, -10);
        transform.position = position;
    }
}
