using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private float velocity = 5f;
    [SerializeField]
    private float jumpForce = 4f;
    [SerializeField]
    private Transform footTransform;
    [SerializeField]
    private LayerMask layerGround;
    private bool isGrounded;
    private Rigidbody2D rb;
    private bool turnRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        float direction = turnRight ? 1f : -1f;
        rb.velocity = new Vector2(direction * velocity, rb.velocity.y);
    }

    void Jump()
    {
        isGrounded = Physics2D.OverlapCircle
            (footTransform.position, 0.2f, layerGround);

        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void Flip()
    {
        turnRight = !turnRight;
        Vector3 escalaLocal = transform.localScale;
        escalaLocal.x *= -1;
        transform.localScale = escalaLocal;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Wall"))
        {
            Flip();
        }
    }
}