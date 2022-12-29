using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = value;
        }
    }
    private float _speed = 2;
    public float maxSpeed = 20;
    public float Acceleration = 5;
    public float Deceleration = 5;

    private float overWriteSpeed;

    private bool animspeedLocked = false;
    [HideInInspector] public bool canMove = true;
    public bool bonusJump;
    public bool disableGroundcheck;

    [Space]

    public float jumpforce = 10;
    [SerializeField] private float jumpDurationLeft;
    private float _jumpDurationLeft;

    [Space]

    private Rigidbody2D rb;

    //running fx
    [SerializeField]public Animator anim;
    [SerializeField] private Transform playerModel;
    [SerializeField]private SpriteRenderer sr;
    [SerializeField] private GameObject IdleAnim, moveAnim;

    //for running noises
    [SerializeField] public AudioSource audJump,audRun;


    [Space]

    [SerializeField] private Transform feet;
    public LayerMask ground;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Move();
        Jump();
    }
    private void Move()
    {
        //for turning faster
        float accelerationScale = 1;
        if (IdleAnim.GetComponent<Renderer>().enabled && ((Mathf.Abs(speed) > 1f)||Mathf.Abs(rb.velocity.y)>10f))
        {
            ToggleIdleOff();
        }
        if (canMove)
        {
            if ((Input.GetAxisRaw("Horizontal") > 0) && (speed < maxSpeed))
            {
                if (speed < 0)
                    accelerationScale = 3;
                if (!Grounded())
                {
                    accelerationScale /= 2;
                }

                speed = Mathf.Clamp(speed + Acceleration * Time.deltaTime * accelerationScale, -maxSpeed , maxSpeed);
                if(sr.flipX == true)
                    Flipsprite(true);
            }
            else if ((Input.GetAxisRaw("Horizontal") < 0) && (speed > -maxSpeed))
            {
                if (speed > 0)
                    accelerationScale = 3;
                if (!Grounded())
                {
                    accelerationScale /= 2;
                }   

                speed = Mathf.Clamp(speed - Acceleration * Time.deltaTime*accelerationScale,-maxSpeed, maxSpeed);
                if(sr.flipX == false)
                    Flipsprite(false);
            }
            else
            {
                //deccelerate on no input
                if (speed > Deceleration * Time.deltaTime)
                    speed = speed - Deceleration * Time.deltaTime;
                else if (speed < -Deceleration * Time.deltaTime)
                    speed = speed + Deceleration * Time.deltaTime;
                else
                {
                    speed = 0;
                    //sepaerate anim model on idle
                }
            }

        }
        //else
        //{
        //    //deccelerate on no input
        //    if (speed > Deceleration * Time.deltaTime)
        //        speed = speed - Deceleration * Time.deltaTime;
        //    else if (speed < -Deceleration * Time.deltaTime)
        //        speed = speed + Deceleration * Time.deltaTime;
        //    else
        //    {
        //        speed = 0;
        //        //sepaerate anim model on idle

        //    }

        //}
        if (canMove)
        {
            if(overWriteSpeed ==0)
                rb.velocity = new Vector2(speed, rb.velocity.y);
            else
            {
                speed = overWriteSpeed;
                overWriteSpeed = 0;
            }

        }
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        //anim.speed = rb.velocity.magnitude;
        anim.SetFloat("VerticalSpeed", Mathf.Abs(rb.velocity.y));

        //when run, play particles and increase animation speed
        if(rb.velocity.magnitude > 0.5f)
        {
            //if (!ps.isPlaying && Grounded())
            //    ps.Play();
            //else if (!Grounded())
            //    ps.Stop();
            if (!animspeedLocked&&Grounded())
                anim.speed = Mathf.Clamp(rb.velocity.magnitude, 0.25f, 2.5f);
        }
        else
        {
            //if(ps.isPlaying)
            //    ps.Stop();
            anim.speed = 1;
        }
    }
    private void Jump()
    {
        if (Grounded() & !Input.GetButton("Jump"))
        {
            _jumpDurationLeft = jumpDurationLeft;
        }
        if (canMove)
        {
            if (Input.GetButtonDown("Jump") && (Grounded()))
            {
                //play sound on jump btn
                audJump.Play();
                anim.SetTrigger("Jump");

            }

            if (Input.GetButton("Jump") && _jumpDurationLeft > 0)
            {
                //increase height on hold
                rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            }

            if (Input.GetButton("Jump"))
            {
                _jumpDurationLeft -= Time.deltaTime;
            }
        }
        else if (Input.GetButtonDown("Jump")&& bonusJump)
        {
            overWriteSpeed = rb.velocity.x;
            bonusJump = false;
            canMove = true;
        }

        if (Input.GetButtonUp("Jump"))
        {
            _jumpDurationLeft = 0;
        }
    }
    public void RestJumpDuration()
    {
        _jumpDurationLeft = jumpDurationLeft;
    }
    public void ToggleIdleOn()
    {   
        if(IdleAnim.GetComponent<Renderer>().enabled == false)
        {
            IdleAnim.GetComponent<Renderer>().enabled = true;
            moveAnim.GetComponent<Renderer>().enabled = false;
        }
    }
    public void ToggleIdleOff()
    {
        IdleAnim.GetComponent<Renderer>().enabled = false;
        moveAnim.GetComponent<Renderer>().enabled = true;
    }
    private void Flipsprite(bool flip)
    {
        //anim.SetBool("Flipped", flip);
        if (flip)
        {
            //sprite flipX is still used for checking flipstate
            //var pshape = ps.shape;
            //pshape.rotation = new Vector3(pshape.rotation.x, -45, pshape.rotation.z);
            sr.flipX = false;
            playerModel.eulerAngles = Vector3.zero;
            IdleAnim.transform.eulerAngles = Vector3.zero;
        }
        else
        {
            //var pshape = ps.shape;
            //pshape.rotation = new Vector3(pshape.rotation.x, 135, pshape.rotation.z);
            sr.flipX = true;
            playerModel.eulerAngles = new Vector3(0, 180, 0);
            IdleAnim.transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
    private bool Grounded()
    {
        if (!disableGroundcheck)
        {
            if (Physics2D.OverlapCircle(feet.position, 0.2f, ground))
            {
                if (bonusJump & !canMove)
                {
                    bonusJump = false;
                    canMove = true;
                }
                return true;

            }
            else
                return false;
        }
        else
            return false;
    }
    public void LockAnimSpeed()
    {
        animspeedLocked = true;
        anim.speed = 1;
    }
    public void UnlockAnimSpeed()
    {
        animspeedLocked = false;
    }
    public void PlayWalkSound()
    {
        if (Grounded())
        {
            audRun.Play();
        }
    }
}
