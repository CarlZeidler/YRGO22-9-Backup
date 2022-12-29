using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] [Range(15, 200)] private float jumpForce;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            PlayerMove pmove = collision.gameObject.GetComponent<PlayerMove>();

            pmove.anim.SetTrigger("Jump");
            pmove.audJump.Play();
            pmove.RestJumpDuration();
            pmove.bonusJump = true;
        }
    }
}
