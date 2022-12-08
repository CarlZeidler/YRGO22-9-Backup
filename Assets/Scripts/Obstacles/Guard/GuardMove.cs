using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class GuardMove : MonoBehaviour
{
    [Header("Guard patrol settings")]
    [Range(1,5)]public float moveSpeed;
    [Range(1,5)]public float maxSpeed;
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
    
    public bool shutDown = false;

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
    private Vector3 startAngle;

    private Rigidbody2D rb2d;

    [SerializeField] private Transform raycastFeetLeftReference;
    [SerializeField] private Transform raycastFeetRightReference;
    [SerializeField] private Transform raycastLeftSide;
    [SerializeField] private Transform raycastRightSide;

    [SerializeField] private Animator[] animators = new Animator[2];
    [SerializeField] private SkeletonAnimation skeletonAnimation;

    void Start()
    {
        Setup();
    }

    void Update()
    {
        CheckPatrolTime();
        Move();
        EdgeCheck();
        FlipSprite();
        Holding();
    }

    private void Setup()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        facingRight = startDirectionRight;
        realPatrolTime = patrolTime;
        realHoldTimeLeft = HoldTimeLeft;
        realHoldTimeRight = HoldTimeRight;
        pivotTimeLeft = HoldTimeLeft;
        pivotTimeRight = HoldTimeRight;
        startAngle = eyes.transform.localRotation.eulerAngles;
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
            foreach(var animator in animators)
            {
                animator.SetBool("Walking", false);
                animator.SetBool("Idle", true);
            }

            //Steps through the selected angles to pivot the vision cone when the guard is at the right patrol apex.
            if (counter <= pivotTimeRight*0.25)
            {
                currentAngle = eyes.transform.localEulerAngles;
                targetAngle = new Vector3(0f, 0f, startAngle.z+eyesPivotUpRight);

                currentAngle = new Vector3(0f, 0f, Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));
                eyes.transform.localEulerAngles = currentAngle;
            }
            else if (counter > pivotTimeRight*0.25 && counter <= pivotTimeRight*0.5)
            {
                currentAngle = eyes.transform.localEulerAngles;
                targetAngle = startAngle;

                currentAngle = new Vector3(0f, 0f, Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));
                eyes.transform.localEulerAngles = currentAngle;
            }
            else if (counter <= pivotTimeRight*0.75)
            {
                currentAngle = eyes.transform.localEulerAngles;
                targetAngle = new Vector3(0f, 0f, startAngle.z-eyesPivotDownRight);

                currentAngle = new Vector3(0f, 0f, Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));
                eyes.transform.localEulerAngles = currentAngle;
            }
            else if (counter < pivotTimeRight)
            {
                currentAngle = eyes.transform.localEulerAngles;
                targetAngle = startAngle;

                currentAngle = new Vector3(0f, 0f, Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));
                eyes.transform.localEulerAngles = currentAngle;
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
            foreach(var animator in animators)
            {
                animator.SetBool("Walking", false);
                animator.SetBool("Idle", true);
            }

            //Steps through the selected angles to pivot the vision cone when the guard is at the left patrol apex.
            if (counter <= pivotTimeLeft * 0.25)
            {
                currentAngle = eyes.transform.localEulerAngles;
                targetAngle = new Vector3(0f, 0f, startAngle.z-eyesPivotUpLeft);

                currentAngle = new Vector3(0f, 0f, Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));
                eyes.transform.localEulerAngles = currentAngle;
            }
            else if (counter > pivotTimeLeft * 0.25 && counter <= pivotTimeLeft * 0.5)
            {
                currentAngle = eyes.transform.localEulerAngles;
                targetAngle = startAngle;

                currentAngle = new Vector3(0f, 0f, Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));
                eyes.transform.localEulerAngles = currentAngle;
            }
            else if (counter <= pivotTimeLeft * 0.75)
            {
                currentAngle = eyes.transform.localEulerAngles;
                targetAngle = new Vector3(0f, 0f, startAngle.z+eyesPivotDownLeft);

                currentAngle = new Vector3(0f, 0f, Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));
                eyes.transform.localEulerAngles = currentAngle;
            }
            else if (counter < pivotTimeLeft)
            {
                currentAngle = eyes.transform.localEulerAngles;
                targetAngle = startAngle;

                currentAngle = new Vector3(0f, 0f, Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));
                eyes.transform.localEulerAngles = currentAngle;
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
            foreach(var animator in animators)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Walking", true);
            }
            rb2d.velocity = new Vector2(Mathf.Clamp(moveSpeed + acceleration * Time.unscaledDeltaTime, 0, maxSpeed), rb2d.velocity.y);
        }
        else if (!facingRight && canMove)
        {
            foreach (var animator in animators)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Walking", true);
            }
            rb2d.velocity = new Vector2(-Mathf.Clamp(moveSpeed + acceleration * Time.unscaledDeltaTime, 0, maxSpeed), rb2d.velocity.y);
        }

        //Resets vision cone if it didn't have time to reset during the hold period.
        currentAngle = eyes.transform.localEulerAngles;
        targetAngle = startAngle;

        currentAngle = new Vector3(0f, 0f, Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));
        eyes.transform.localEulerAngles = currentAngle;
    }

    private void FlipSprite()
    {
        //Flips sprite depending on facing direction.
        if (facingRight)
        {   
            visuals.transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        if (!facingRight)
        {
            visuals.transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void EdgeCheck()
    {
        bool rayCastHit = false;

        Debug.DrawRay(raycastFeetLeftReference.position, new Vector2(0f, -1f), Color.red);

        if ((!Physics2D.Raycast(raycastFeetLeftReference.position, new Vector2(0f, -1f), 2f, LayerMask.GetMask("Ground")) && !facingRight) ||
                (!Physics2D.Raycast(raycastFeetRightReference.position, new Vector2(0f, -1f), 2f, LayerMask.GetMask("Ground")) && facingRight) ||
                    (Physics2D.Raycast(raycastRightSide.position, new Vector2(0.5f, 0f), 1f, LayerMask.GetMask("Ground", "Obstacle")) && facingRight) ||
                        (Physics2D.Raycast(raycastLeftSide.position, new Vector2(-0.5f, 0f), 1f, LayerMask.GetMask("Ground", "Obstacle")) && !facingRight))
        {
            rayCastHit = true;
        }

        if (rayCastHit && rayCastBuffer <= 0)
        {
            TurnAround();
            rayCastBuffer = 0.6f;
        }
        else if (rayCastBuffer > 0)
        {
            rayCastBuffer -= 1 * Time.deltaTime;
        }
    }
}
