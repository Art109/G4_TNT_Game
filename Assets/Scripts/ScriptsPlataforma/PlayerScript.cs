using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerScript : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    private TrailRenderer tr;
    private Animator animator;
    private CinemachineImpulseSource impulseSource;
    private SpriteRenderer sr;

    [Tooltip("Animator do Caldeirão")]
    [SerializeField]
    private Animator animatorCauldron;

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
    private bool isGroundedNoWallJump;
    [SerializeField]
    private LayerMask groundNoWallJumpLayer;

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

    [Header("FruitsObjects and Count")]
    [SerializeField]
    private int fruitsCount = 0;
    [SerializeField]
    private GameObject guavaObject;
    [SerializeField]
    private GameObject mangoObject;
    [SerializeField]
    private GameObject appleObject;
    [SerializeField]
    private GameObject pineappleObject;


    [Header("Cinemachine")]
    [SerializeField]
    private CameraFollowOffset cameraFollowOffsetScript;
    private bool flipCamera = false;

    [Header("LifeController")]
    private bool isDead = false;
    private bool isDecomposing = false;

    [Header("Timer")]
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    [Tooltip("Tempo em Segundos")]
    private float time = 300f;
    private bool timeStop = false;
    private bool timerIsOver = false;

    [Header("Condition Victory")]
    private bool contactCauldron = false;
    [SerializeField]
    private Transform cauldronTransfrom;
    [SerializeField]
    private GameObject[] fruitsPrefab;
    private bool oneC = true;
    [SerializeField]
    private GameObject tntObject;
    [SerializeField]
    private GameObject smokePrefab;
    [SerializeField]
    private Transform launchSmoke;
    [SerializeField]
    private GameObject returnMenu;

    [Header("Canvas Pause and Controls")]
    [SerializeField]
    private GameObject pauseObejct;
    private bool paused = false;


    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
        animator = GetComponent<Animator>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(footTransform.position, 0.2f, layerGround);

        isGroundedNoWallJump = Physics2D.OverlapCircle(footTransform.position, 0.2f, groundNoWallJumpLayer);

        

        if (contactCauldron)
        {
            Victory();
            return;
        }

        if (isDead)
        {
            Dead();
            return;
        }

        if (timerIsOver)
        {
            TimeIsOver();
            return;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        Timer();
        Move();
        Jump();
        WallJumpVerify();
        HandleWallSlide();
        Dash();
    }

    void Victory()
    {
        cameraFollowOffsetScript.offsetX = 1f;
        cameraFollowOffsetScript.zooming = true;
        rb.gravityScale = 2f;
        rb.velocity = new Vector2(0, rb.velocity.y);

        if (returnMenu.activeInHierarchy && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(0);
        }

        if (fruitsCount >= 4)
        {
            animator.Play("idle");
            if (oneC)
            {
                oneC = false;
                StartCoroutine(FruitDrop());
            }
        }
        else
        {
            returnMenu.SetActive(true);
            animator.Play("angry");
        }
    }

    IEnumerator FruitDrop()
    {
        for (int i = 0; i < fruitsCount; i++)
        {
            GameObject fruit = Instantiate(fruitsPrefab[i], transform.position, Quaternion.identity);
            Vector3 start = transform.position;
            Vector3 end = cauldronTransfrom.position;

            impulseSource.GenerateImpulse();
            if (i == 0)
            {
                guavaObject.SetActive(false);
            }
            else if (i == 1)
            {
                mangoObject.SetActive(false);
            }
            else if (i == 2)
            {
                appleObject.SetActive(false);
            }
            else if (i == 3)
            {
                pineappleObject.SetActive(false);
            }

            float t = 0f;
            float duration = 0.5f;

            while (t < 1f)
            {
                if (fruit == null)
                    yield break;

                t += Time.deltaTime / duration;
                Vector3 pos = Vector3.Lerp(start, end, t);
                pos.y += Mathf.Sin(t * Mathf.PI) * 1.0f; 
                fruit.transform.position = pos;

                yield return null;
            }

            animatorCauldron.SetTrigger("Smoke");
            if (fruit != null)
                Destroy(fruit);

            yield return new WaitForSeconds(0.5f);
        }
        
        GameObject smoke = Instantiate(smokePrefab, launchSmoke.position, Quaternion.identity);

        Destroy(smoke, 0.9f);
        tntObject.SetActive(true);

        returnMenu.SetActive(true);
    }

    void Dead()
    {
        cameraFollowOffsetScript.zooming = true;
        cameraFollowOffsetScript.targetZoom = 5f;
        cameraFollowOffsetScript.offsetX = 0f;
        rb.velocity = Vector3.zero;
        if (!isDecomposing)
        {
            isDecomposing = true;
            StartCoroutine(Decomposing());
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Temporário
            SceneManager.LoadScene("Assets/Scenes/PlataformaPrototipo.unity");
        }
    }

    IEnumerator Decomposing()
    {
        animator.Play("dead");
        yield return new WaitForSeconds(0.5f);
        animator.Play("decomposing");
        yield return new WaitForSeconds(0.5f);
        sr.enabled = false;
    }

    void Timer()
    {
        if (time > 0 && !timeStop && !contactCauldron)
        {
            time -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);

            timerText.text = $"{minutes:00}:{seconds:00}";

            if (time <= 0)
            {
                timeStop = true;
            }
        }
        else if (timeStop)
        {
            timerText.text = "0:00";
            timerIsOver = true;
        }
    }

    void TimeIsOver()
    {
        cameraFollowOffsetScript.targetZoom = 5f;
        cameraFollowOffsetScript.zooming = true;
        cameraFollowOffsetScript.offsetX = 0f;
        if (isTouchingWall)
        {
            rb.gravityScale = 0f;
            rb.velocity = Vector3.zero;
            animator.Play("angry");
        }

        if (rb.velocity.y == 0)
        {
            animator.Play("angry");
            rb.velocity = Vector3.zero;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            // Temporário
            SceneManager.LoadScene("Assets/Scenes/PlataformaPrototipo.unity");
        }
    }

    public void Pause()
    {
        paused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        pauseObejct.SetActive(true);
    }

    public void Resume()
    {
        paused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        pauseObejct.SetActive(false);
    }

    public void ResetLevel()
    {
        // Temporário
        SceneManager.LoadScene("PlataformaPrototipo");
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene(0);
    }

    void Move()
    {
        float direction = turnRight ? 1f : -1f;
        rb.velocity = new Vector2(direction * velocity, rb.velocity.y);
    }

    void Jump()
    {
        if (isDashing || isDead)
            return;

        if ((!isGrounded || !isGroundedNoWallJump) && !isTouchingWall && !isDead)
        {
            if (rb.velocity.y == 0)
            {
                animator.Play("run");
            }
            else if (rb.velocity.y > 0.1f)
            {
                animator.Play("rising");
            }
            else if (rb.velocity.y < -0.1f)
            {
                animator.Play("falling");
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            if (isGrounded || isGroundedNoWallJump)
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
        if (isDashing || isDead)
            return;

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

        if (dashInput && canDash && !isTouchingWall && !paused)
        {
            animator.Play("roll");
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

        if (isGrounded || isGroundedNoWallJump)
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
        int layer = col.gameObject.layer;

        if (((1 << layer) & layerGround) != 0 || ((1 << layer) & groundNoWallJumpLayer) != 0)
        {
            foreach (ContactPoint2D contact in col.contacts)
            {
                // Verifica se bateu na lateral (X grande, Y pequeno)
                if (Mathf.Abs(contact.normal.x) > 0.9f)
                {
                    // Se bateu do lado esquerdo (normal.x positivo), entï¿½o vira para a direita
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
                    break; // Jï¿½ virou, nï¿½o precisa checar outros contatos
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Apple"))
        {
           
            impulseSource.GenerateImpulse();
            fruitsCount += 1;
            appleObject.SetActive(true);
            Destroy(col.gameObject);
        }

        if (col.CompareTag("Guava"))
        {
            impulseSource.GenerateImpulse();
            fruitsCount += 1;
            guavaObject.SetActive(true);
            Destroy(col.gameObject);
        }

        if (col.CompareTag("Pineapple"))
        {
            impulseSource.GenerateImpulse();
            fruitsCount += 1;
            pineappleObject.SetActive(true);
            Destroy(col.gameObject);
        }

        if (col.CompareTag("Mango"))
        {
            impulseSource.GenerateImpulse();
            fruitsCount += 1;
            mangoObject.SetActive(true);
            Destroy(col.gameObject);
        }

        if (col.CompareTag("Death"))
        {
            impulseSource.GenerateImpulse();
            isDead = true;
        }

        if (col.CompareTag("Cauldron"))
        {
            contactCauldron = true;
        }
    }
}