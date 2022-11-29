using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMove : MonoBehaviour
{
    [Header("Guard patrol settings")]
    public float moveSpeed;
    public float maxSpeed;
    public float patrolTime;
    public float HoldTimeLeft;
    public float HoldTimeRight;

    [Header("Vision settings")]
    public float eyesPivotUpRight;
    public float eyesPivotDownRight;
    public float eyesPivotUpLeft;
    public float eyesPivotDownLeft;

    [Header("Patrol start direction")]
    public bool startDirectionRight;
    
    public GuardBehaviour behaviourScript;
    public GameObject visuals;
    public GameObject eyes;

    [HideInInspector]
    public bool canMove = true;

    private bool rayCastLeft;
    private bool rayCastRight;

    private bool facingRight;
    private float realPatrolTime;
    private float realHoldTimeLeft;
    private float realHoldTimeRight;
    private float rayCastBuffer = 0f;
    private float acceleration = 1.5f;
    private float pivotTimeRight;
    private float pivotTimeLeft;
    private float counter;

    private Vector3 targetAngle;
    private Vector3 currentAngle;

    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        facingRight = startDirectionRight;
        realPatrolTime = patrolTime;
        realHoldTimeLeft = HoldTimeLeft;
        realHoldTimeRight = HoldTimeRight;
        pivotTimeLeft = HoldTimeLeft;
        pivotTimeRight = HoldTimeRight;
    }

    void Update()
    {
        CheckPatrolTime();
        Move();
        FlipSprite();
        Holding();
        EdgeCheck();
    }

    private void CheckPatrolTime()
    {
        if (realPatrolTime <= 0 && facingRight && realHoldTimeRight <= 0)
        {
            TurnAround();
        }
        else if (realPatrolTime <= 0 && !facingRight && realHoldTimeLeft <= 0)
        {
            TurnAround();
        }
        else if (canMove)
        {
            realPatrolTime -= 1 * Time.deltaTime;
        }
    }
    private void TurnAround()
    {
        facingRight = !facingRight;
        realPatrolTime = patrolTime;
        realHoldTimeLeft = HoldTimeLeft;
        realHoldTimeRight = HoldTimeRight;
        canMove = true;
    }

    private void Holding()
    {
        if (facingRight && realPatrolTime <= 0 && realHoldTimeRight > 0)
        {
            canMove = false;
            
            //Steps through the selected angles to pivot the vision cone when the guard is at the right patrol apex.
            if (counter <= pivotTimeRight*0.25)
            {
                currentAngle = eyes.transform.eulerAngles;
                targetAngle = new Vector3(0f, 0f, eyesPivotUpRight);

                currentAngle = new Vector3(0f, 0f, Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));
                eyes.transform.eulerAngles = currentAngle;
                
            }
            else if (counter > pivotTimeRight*0.25 && counter <= pivotTimeRight*0.5)
            {
                currentAngle = eyes.transform.eulerAngles;
                targetAngle = Vector3.zero;

                currentAngle = new Vector3(0f, 0f, Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));
                eyes.transform.eulerAngles = currentAngle;
            }
            else if (counter <= pivotTimeRight*0.75)
            {
                currentAngle = eyes.transform.eulerAngles;
                targetAngle = new Vector3(0f, 0f, -eyesPivotDownRight);

                currentAngle = new Vector3(0f, 0f, Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));
                eyes.transform.eulerAngles = currentAngle;
            }
            else if (counter < pivotTimeRight)
            {
                currentAngle = eyes.transform.eulerAngles;
                targetAngle = Vector3.zero;

                currentAngle = new Vector3(0f, 0f, Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));
                eyes.transform.eulerAngles = currentAngle;
            }
            else
            {
                counter = 0f;
            }

            counter += 1 * Time.deltaTime;
            realHoldTimeRight -= 1 * Time.deltaTime;
        }
        else if (!facingRight && realPatrolTime <= 0 && realHoldTimeLeft > 0)
        {
            canMove = false;

            //Steps through the selected angles to pivot the vision cone when the guard is at the left patrol apex.
            if (counter <= pivotTimeLeft * 0.25)
            {
                currentAngle = eyes.transform.eulerAngles;
                targetAngle = new Vector3(0f, 0f, -eyesPivotUpLeft);

                currentAngle = new Vector3(0f, 0f, Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));
                eyes.transform.eulerAngles = currentAngle;
            }
            else if (counter > pivotTimeLeft * 0.25 && counter <= pivotTimeLeft * 0.5)
            {
                currentAngle = eyes.transform.eulerAngles;
                targetAngle = Vector3.zero;

                currentAngle = new Vector3(0f, 0f, Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));
                eyes.transform.eulerAngles = currentAngle;
            }
            else if (counter <= pivotTimeLeft * 0.75)
            {
                currentAngle = eyes.transform.eulerAngles;
                targetAngle = new Vector3(0f, 0f, eyesPivotDownLeft);

                currentAngle = new Vector3(0f, 0f, Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));
                eyes.transform.eulerAngles = currentAngle;
            }
            else if (counter < pivotTimeLeft)
            {
                currentAngle = eyes.transform.eulerAngles;
                targetAngle = Vector3.zero;

                currentAngle = new Vector3(0f, 0f, Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));
                eyes.transform.eulerAngles = currentAngle;
            }
            else
            {
                counter = 0f;
            }

            counter += 1 * Time.deltaTime;
            realHoldTimeLeft -= 1 * Time.deltaTime;
        }
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

        //Resets vision cone if it didn't have time to reset during the hold period.
        currentAngle = eyes.transform.eulerAngles;
        targetAngle = Vector3.zero;

        currentAngle = new Vector3(0f, 0f, Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));
        eyes.transform.eulerAngles = currentAngle;
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
