using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftPlatform : MonoBehaviour
{
    [HideInInspector]public float speed = 5f;
    [HideInInspector]public Vector3 target;


    //moves towards target
    private void Update()
    {
        if((transform.position - target).magnitude > 0.05f)
        {
            transform.position += (target-transform.position).normalized*speed*Time.deltaTime;
        }
        else
        {
            GetComponentInChildren<PlatformPlayerFollow>().OnDespawn();
            //depsawn anim
            Destroy(gameObject);
        }
    }
}
