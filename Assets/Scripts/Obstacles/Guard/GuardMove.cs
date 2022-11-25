using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMove : MonoBehaviour
{
    public float moveSpeed;
    public float acceleration;
    public float deceleration;
    public float patrolTime;
    public float initialPatrolTime;
    public float maxSpeed;

    float rayCastBuffer = 0f;

    public bool startDirectionRight;
    public bool rayCastLeft;
    public bool rayCastRight;
    private bool facingRight;
    public bool canMove = true;

    public GuardBehaviour behaviourScript;
    public GameObject visuals;

    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        facingRight = startDirectionRight;
    }

    void Update()
    {
        CheckPatrolTime();
        Move();
        FlipSprite();
        EdgeCheck();
    }

    private void CheckPatrolTime()
    {
        if (patrolTime <= 0)
        {
            TurnAround();
        }
        else
        {
            patrolTime -= 1 * Time.deltaTime;
        }   
    }
    private void TurnAround()
    {
            facingRight = !facingRight;
            patrolTime = initialPatrolTime;
    }
    private void Move()
    {
        if (facingRight && canMove)
        {
            rb2d.velocity = new Vector2(Mathf.Clamp(moveSpeed + acceleration * Time.deltaTime, 0, maxSpeed), rb2d.velocity.y);
        }
        else if (!facingRight && canMove)
        {
            rb2d.velocity = new Vector2(-Mathf.Clamp(moveSpeed + acceleration * Time.deltaTime, 0, maxSpeed), rb2d.velocity.y);
        }

    }

    private void FlipSprite()
    {
        //Flips sprite depending on facing direction.
        if (facingRight)
        {   
            visuals.transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, base.transform.localScale.z);
        }
        if (!facingRight)
        {
            visuals.transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void EdgeCheck()
    {
        //Raycasts down to the sides to check if the floor is gone.
        rayCastLeft = Physics2D.Raycast(transform.position, new Vector2(-0.6f, -1f), 2f);
        rayCastRight = Physics2D.Raycast(transform.position, new Vector2(0.6f, -1f), 2f);

        if (!rayCastLeft || !rayCastRight && rayCastBuffer <= 0)
        {
            TurnAround();
            rayCastBuffer = 3f;
        }
        else if (rayCastBuffer > 0)
        {
            rayCastBuffer -= 1 * Time.deltaTime;
        }
    }
}
