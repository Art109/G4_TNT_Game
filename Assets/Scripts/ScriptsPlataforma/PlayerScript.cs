using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    private TrailRenderer tr;
    private Animator animator;

    [Header("Move and Jump")]
    [SerializeField]
    private float velocity = 5f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private Transform footTransform;
    [SerializeField]
    private LayerMask layerGround;
    private bool isGrounded;
    private bool turnRight = true;

    [Header("WallJump")]
    [SerializeField]
    private Transform wallCheckTransform;
    [SerializeField]
    private float wallCheckRadius = 0.78f;
    [SerializeField]
    private float wallJumpXForce = 6f;
    [SerializeField]
    private float wallJumpYForce = 6F;
    private bool isTouchingWall;

    [Header("Dashing")]
    [SerializeField]
    private float dashingVelocity = 8f;
    [SerializeField]
    private float dashingTime = 0.66f;
    private Vector2 dashingDirection;
    private bool isDashing;
    private bool canDash = true;

    [Header("WallSlide")]
    [SerializeField]
    private float wallSlideSpeed = 1f;
    private bool isWallSliding;

    [Header("Fruits")]
    private int countApples = 0;
    private int countGuava = 0;
    private int countPineapple = 0;
    private int countMango = 0;

    [Header("Cinemachine")]
    private bool flipCamera = false;

    [Header("LifeController")]
    private bool isDead = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(footTransform.position, 0.2f, layerGround);

        if (isDead)
        {
            Dead();
            return;
        }
        Move();
        Jump();
        WallJumpVerify();
        HandleWallSlide();
        Dash();
    }

    void Dead()
    {
        animator.Play("dead");

        if (Input.GetKeyDown(KeyCode.R))
        {
            // Temporario
            SceneManager.LoadScene("Assets/Scenes/PlataformaPrototipo.unity");
        }
    }

    void Move()
    {
        float direction = turnRight ? 1f : -1f;
        rb.velocity = new Vector2(direction * velocity, rb.velocity.y);
    }

    void Jump()
    {
        if (!isGrounded && !isTouchingWall && !isDead)
        {
            if (rb.velocity.y > 0.1f)
            {
                animator.Play("rising");
            }
            else if (rb.velocity.y < -0.1f)
            {
                animator.Play("falling");
            }
        }
        else if(isGrounded && !isTouchingWall && !isDead)
        {
            animator.Play("run");
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            else if (isTouchingWall)
            {
                float wallDirection = turnRight ? -1f : 1f;
                rb.velocity = new Vector2(wallJumpXForce * wallDirection, wallJumpYForce);
                Flip();
            }
        }
    }

    void WallJumpVerify()
    {
        Vector2 direction = turnRight ? Vector2.right : Vector2.left;
        RaycastHit2D wallHit = Physics2D.Raycast(transform.position, direction, wallCheckRadius, layerGround);
        isTouchingWall = wallHit.collider != null;
    }

    void HandleWallSlide()
    {
        if (isTouchingWall && !isGrounded && !isDead)
        {
            animator.Play("crouch");
            isWallSliding = true;
            rb.velocity = new Vector2(0, -wallSlideSpeed);

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                wallSlideSpeed = 3.00f;
            }
            else
            {
                wallSlideSpeed = 0.20f;
            }

            if (Physics2D.Raycast(transform.position, Vector2.right, wallCheckRadius, layerGround))
            {
                flipCamera = false;
            }
            else if (Physics2D.Raycast(transform.position, Vector2.left, wallCheckRadius, layerGround))
            {
                flipCamera = true;
            }
        }
        else
        {
            isWallSliding = false;
            flipCamera = turnRight;
        }
    }


    void Flip()
    {
        turnRight = !turnRight;
        Vector3 escalaLocal = transform.localScale;
        escalaLocal.x *= -1;
        transform.localScale = escalaLocal;
    }

    public bool IsLookingRight()
    {
        return turnRight;
    }

    public bool isLookingCamera()
    {
        return flipCamera;
    }

    private void Dash()
    {
        bool dashInput = Input.GetKeyDown(KeyCode.LeftShift);

        if (dashInput && canDash)
        {
            isDashing = true;
            canDash = false;
            tr.emitting = true;

            float direction = Mathf.Sign(transform.localScale.x);
            dashingDirection = new Vector2(direction, 0);
            StartCoroutine(StoppingDash());
        }

        if (isDashing)
        {
            rb.velocity = dashingDirection.normalized * dashingVelocity;
            return;
        }

        if (isGrounded)
        {
            canDash = true;
        }
    }

    IEnumerator StoppingDash()
    {
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        isDashing = false;
    }

    void OnDrawGizmosSelected()
    {
        Vector2 direction = turnRight ? Vector2.right : Vector2.left;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(direction * wallCheckRadius));
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (((1 << col.gameObject.layer) & layerGround) != 0)
        {
            foreach (ContactPoint2D contact in col.contacts)
            {
                // Verifica se bateu na lateral (X grande, Y pequeno)
                if (Mathf.Abs(contact.normal.x) > 0.9f)
                {
                    // Se bateu do lado esquerdo (normal.x positivo), então vira para a direita
                    if (!isWallSliding)
                    {
                        if (contact.normal.x > 0 && !turnRight)
                        {
                            Flip();
                        }
                        // Se bateu do lado direito (normal.x negativo), vira para a esquerda
                        else if (contact.normal.x < 0 && turnRight)
                        {
                            Flip();
                        }
                    }
                    break; // Já virou, não precisa checar outros contatos
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Apple"))
        {
            countApples += 1;
            Destroy(col.gameObject);
        }

        if (col.CompareTag("Guava"))
        {
            countGuava += 1;
            Destroy(col.gameObject);
        }

        if (col.CompareTag("Pineapple"))
        {
            countPineapple += 1;
            Destroy(col.gameObject);
        }

        if (col.CompareTag("Mango"))
        {
            countMango += 1;
            Destroy(col.gameObject);
        }

        if (col.CompareTag("Death"))
        {
            isDead = true;
        }
    }
}