using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] [Range(15, 100)] private float jumpForce;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            collision.gameObject.GetComponent<PlayerMove>().anim.SetTrigger("Jump");
            collision.gameObject.GetComponent<PlayerMove>().audJump.Play();
        }
    }
}
