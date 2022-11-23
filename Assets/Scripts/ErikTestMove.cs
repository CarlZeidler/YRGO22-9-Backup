using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErikTestMove : MonoBehaviour
{
    public float speed = 5;

    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(x, y) * speed;

        rb2d.velocity = movement;
    }
}
