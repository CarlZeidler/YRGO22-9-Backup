using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 2;
    public float maxSpeed = 20;
    public float Acceleration = 5;
    public float Deceleration = 5;

    private bool animspeedLocked = false;

    [Space]

    public float jumpforce = 10;
    [SerializeField] private float jumpDurationLeft;
    private float _jumpDurationLeft;

    [Space]

    private Rigidbody2D rb;

    //running fx
    private ParticleSystem ps;
    [SerializeField]private Animator anim;
    [SerializeField] private Transform playerModel;
    [SerializeField]private SpriteRenderer sr;

    //for running noises
    private AudioSource aud;

    [Space]

    public AudioClip walk, jump;

    [Space]

    [SerializeField] private Transform feet;
    public LayerMask ground;
    
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        aud = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponentInChildren<Animator>();
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
        if ((Input.GetAxisRaw("Horizontal") > 0) && (speed < maxSpeed))
        {
            if (speed < 0)
                accelerationScale = 3;
            speed = Mathf.Clamp(speed + Acceleration * Time.deltaTime * accelerationScale, -maxSpeed , maxSpeed);
            if(sr.flipX == true)
                Flipsprite(true);
        }
        else if ((Input.GetAxisRaw("Horizontal") < 0) && (speed > -maxSpeed))
        {
            if (speed > 0)
                accelerationScale = 3;
            speed = Mathf.Clamp(speed - Acceleration * Time.deltaTime*accelerationScale,-maxSpeed, maxSpeed);
            if(sr.flipX == false)
                Flipsprite(false);
        }
        else
        {
            if (speed > Deceleration * Time.deltaTime)
                speed = speed - Deceleration * Time.deltaTime;
            else if (speed < -Deceleration * Time.deltaTime)
                speed = speed + Deceleration * Time.deltaTime;
            else
                speed = 0;
        }

        rb.velocity = new Vector2(speed, rb.velocity.y);
        anim.SetFloat("Speed", rb.velocity.magnitude);
        //anim.speed = rb.velocity.magnitude;
        //anim.SetFloat("verticalSpeed", rb.velocity.y);

        //when run, play particles and increase animation speed
        if(rb.velocity.magnitude > 0.5f)
        {
            if (!ps.isPlaying && Grounded())
                ps.Play();
            else if (!Grounded())
                ps.Stop();
            if (!animspeedLocked)
                anim.speed = Mathf.Clamp(rb.velocity.magnitude, 0.25f, 2.5f);
        }
        else
        {
            if(ps.isPlaying)
                ps.Stop();
            anim.speed = 1;
        }
    }
    private void Jump()
    {
        if (Grounded() & !Input.GetButton("Jump"))
            _jumpDurationLeft = jumpDurationLeft;
        if (Input.GetButtonDown("Jump") && Grounded())
        {
            aud.Stop();
            aud.clip = jump;
            aud.Play();
        }

        if (Input.GetButton("Jump") && _jumpDurationLeft > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
        }

        if (!Grounded() && Input.GetButton("Jump"))
        {
            _jumpDurationLeft -= Time.deltaTime;
        }

        if (Input.GetButtonUp("Jump"))
        {
            _jumpDurationLeft = 0;
        }
    }
    private void Flipsprite(bool flip)
    {
        //anim.SetBool("Flipped", flip);
        if (flip)
        {
            //sprite flipX is still used for checking flipstate
            var pshape = ps.shape;
            pshape.rotation = new Vector3(pshape.rotation.x, -45, pshape.rotation.z);
            sr.flipX = false;
            playerModel.eulerAngles = Vector3.zero;
        }
        else
        {
            var pshape = ps.shape;
            pshape.rotation = new Vector3(pshape.rotation.x, 135, pshape.rotation.z);
            sr.flipX = true;
            playerModel.eulerAngles = new Vector3(0, 180, 0);
        }
    }
    private bool Grounded()
    {
        if (Physics2D.OverlapCircle(feet.position, 0.2f, ground))
            return true;
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
            aud.PlayOneShot(walk);
        }
    }
}
