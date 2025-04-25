using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;
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
    private float wallCheckRadius = 0.68f;
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
    private GameObject timerObject;
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    [Tooltip("Tempo em Segundos")]
    private float time = 300f;
    private bool timeStop = false;
    private bool timerIsOver = false;
    [SerializeField]
    private GameObject timeIsOverObject;

    [Header("UI Face")]
    [SerializeField]
    private Image imageField;
    // Normal 0; Happy 1; Angry 2; Empty 3;
    [SerializeField]
    private Sprite[] facesUI;

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

    [Header("Book End")]
    [SerializeField]
    private GameObject endMenu;
    [SerializeField]
    private TextMeshProUGUI textVictoryOrDefeat;
    [SerializeField]
    private TextMeshProUGUI timeRemaining;
    [SerializeField]
    private TextMeshProUGUI fruitsRemaining;
    [SerializeField]
    private GameObject tntUI;

    [Header("Canvas Pause, Death and Controls")]
    [SerializeField]
    private GameObject pauseObejct;
    private bool paused = false;
    [SerializeField]
    private GameObject deadObject;
    [SerializeField]
    private Image faceField;

    [Header("Audios")]
    [SerializeField]
    private AudioSource jump, dropFruit, magicExplosion, hurt, roll, whooshClothes, grabItem, background, bonfire, water;
    [SerializeField]
    private AudioSource[] footsteps;

    [Header("Footsteps Audio")]
    private float stepInterval = 0.35f; 
    private float timeSinceLastStep = 0f; 
    private int lastFootstepIndex = -1;

    private PlayerInput playerInput;


    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
        animator = GetComponent<Animator>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        playerInput = GetComponent<PlayerInput>();
        playerInput.SwitchCurrentActionMap("Player");
        Time.timeScale = 1f;
    }

    void Update()
    {
        
        isGrounded = Physics2D.OverlapCircle(footTransform.position, 0.2f, layerGround);

        isGroundedNoWallJump = Physics2D.OverlapCircle(footTransform.position, 0.2f, groundNoWallJumpLayer);

        

        if (contactCauldron)
        {
            playerInput.SwitchCurrentActionMap("UI");
            Victory();
            return;
        }

        if (isDead)
        {
            playerInput.SwitchCurrentActionMap("UI");
            Dead();
            return;
        }

        if (timerIsOver)
        {
            playerInput.SwitchCurrentActionMap("UI");
            TimeIsOver();
            return;
        }

        if (playerInput.actions["Pause"].WasPressedThisFrame() || playerInput.actions["UnPause"].WasPressedThisFrame())
        {
            if (paused)
            {
                playerInput.SwitchCurrentActionMap("Player");
                water.UnPause();
                bonfire.UnPause();
                background.UnPause();
                Resume();
            }
            else
            {
                playerInput.SwitchCurrentActionMap("UI");
                water.Pause();
                bonfire.Pause();
                background.Pause();
                Pause();
            }
        }

        if (paused && playerInput.actions["Return"].WasPressedThisFrame())
        {
            ReturnMenu();
        }
        if(paused && playerInput.actions["Reset"].WasPressedThisFrame())
        {
            ResetLevel();
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

        timeRemaining.text = timerText.text;
        fruitsRemaining.text = $"{fruitsCount}/4";


        if (playerInput.actions["Return"].WasPressedThisFrame())
        {
            ReturnMenu();
        }
        else if (playerInput.actions["Reset"].WasPressedThisFrame())
        {
            ResetLevel();
        }

        if (fruitsCount >= 4)
        {
            tntUI.SetActive(true);
            textVictoryOrDefeat.text = "SUCESSO!";
            animator.Play("idle");
            imageField.sprite = facesUI[1];
            faceField.sprite = facesUI[1];
            if (oneC)
            {
                oneC = false;
                StartCoroutine(FruitDrop());
            }
        }
        else
        {
            tntUI.SetActive(false);
            textVictoryOrDefeat.text = "INGREDIENTES FALTANDO";
            timerObject.SetActive(false);
            faceField.sprite = facesUI[2];
            endMenu.SetActive(true);
            animator.Play("angry");
            imageField.sprite = facesUI[2];
      
        }
    }

    IEnumerator FruitDrop()
    {
        for (int i = 0; i < fruitsCount; i++)
        {
            GameObject fruit = Instantiate(fruitsPrefab[i], transform.position, Quaternion.identity);
            dropFruit.Play();
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

        magicExplosion.Play();
        yield return new WaitForSeconds(0.5f);
        GameObject smoke = Instantiate(smokePrefab, launchSmoke.position, Quaternion.identity);

        Destroy(smoke, 0.9f);
        tntObject.SetActive(true);

        timerObject.SetActive(false);
        endMenu.SetActive(true);
    }

    void Dead()
    {

        imageField.sprite = facesUI[3];
        deadObject.SetActive(true);
        cameraFollowOffsetScript.zooming = true;
        cameraFollowOffsetScript.targetZoom = 5f;
        cameraFollowOffsetScript.offsetX = 0f;
        rb.velocity = Vector3.zero;
        if (!isDecomposing)
        {
            isDecomposing = true;
            StartCoroutine(Decomposing());
        }

        if (isDead && playerInput.actions["Return"].WasPressedThisFrame())
        {
            ReturnMenu();
        }
        else if (isDead && playerInput.actions["Reset"].WasPressedThisFrame())
        {
            ResetLevel();
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
        playerInput.SwitchCurrentActionMap("UI");

        if (timerIsOver && playerInput.actions["Return"].WasPressedThisFrame())
        {
            ReturnMenu();
        }
        else if (timerIsOver && playerInput.actions["Reset"].WasPressedThisFrame())
        {
            ResetLevel();
        }

        if (isTouchingWall)
        {
            rb.gravityScale = 0f;
            rb.velocity = Vector3.zero;
            animator.Play("angry");
            imageField.sprite = facesUI[2];
            timeIsOverObject.SetActive(true);
        }

        if (rb.velocity.y == 0)
        {
            imageField.sprite = facesUI[2];
            animator.Play("angry");
            rb.velocity = Vector3.zero;
            timeIsOverObject.SetActive(true);
        }
    }

    public void Pause()
    {
        paused = true;
        Time.timeScale = 0f;
        pauseObejct.SetActive(true);
    }

    public void Resume()
    {
        paused = false;
        Time.timeScale = 1f;
        pauseObejct.SetActive(false);
    }

    public void ResetLevel()
    {
        // Temporário - Ajustar o valor na hora da build final
        SceneManager.LoadScene("PlataformaPrototipo");
    }

    public void ReturnMenu()
    {
        // Temporário - Ajustar o valor na hora da build final
        SceneManager.LoadScene(0);
    }

    void Move()
    {
        float direction = turnRight ? 1f : -1f;
        rb.velocity = new Vector2(direction * velocity, rb.velocity.y);

        timeSinceLastStep += Time.deltaTime;

        if (isGroundedNoWallJump && timeSinceLastStep >= stepInterval)
        {
            int index = Random.Range(0, footsteps.Length);

            if (!isGroundedNoWallJump)
            {
                footsteps[index].Stop();
            }

            if (footsteps[index].isPlaying || (lastFootstepIndex == index))
            {
                return;
            }

            footsteps[index].Play();
            lastFootstepIndex = index;
            timeSinceLastStep = 0f;
        }
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

        if (playerInput.actions["Jump"].WasPressedThisFrame() && !paused)
        {
            if (isGrounded || isGroundedNoWallJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jump.Play();
            }
            else if (isTouchingWall)
            {
                float wallDirection = turnRight ? -1f : 1f;
                rb.velocity = new Vector2(wallJumpXForce * wallDirection, wallJumpYForce);
                Flip();
                jump.Play();
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

            if (playerInput.actions["WallSlide"].IsPressed())
            {
                rb.velocity = new Vector2(0, Mathf.Max(rb.velocity.y, -wallSlideSpeed)); 
                wallSlideSpeed = Mathf.Lerp(wallSlideSpeed, 3.00f, Time.deltaTime * 8f);
            }
            else
            {
                rb.velocity = new Vector2(0, Mathf.Max(rb.velocity.y, -0.20f));
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
        bool dashInput = playerInput.actions["Dash"].WasPressedThisFrame();

        if (dashInput && canDash && !isTouchingWall && !paused)
        {
            roll.Play();
            whooshClothes.Play();
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
        if (col.CompareTag("Apple") || col.CompareTag("Guava") || col.CompareTag("Pineapple") || col.CompareTag("Mango"))
        {
            grabItem.Play();
            StartCoroutine(ChangeFace());
            impulseSource.GenerateImpulse();
            fruitsCount += 1;
        }

        if (col.CompareTag("Apple"))
        {
            appleObject.SetActive(true);
            Destroy(col.gameObject);
        }

        if (col.CompareTag("Guava"))
        {
            guavaObject.SetActive(true);
            Destroy(col.gameObject);
        }

        if (col.CompareTag("Pineapple"))
        {
            pineappleObject.SetActive(true);
            Destroy(col.gameObject);
        }

        if (col.CompareTag("Mango"))
        {
            mangoObject.SetActive(true);
            Destroy(col.gameObject);
        }

        if (col.CompareTag("Death"))
        {
            if(!isDead)
                hurt.Play();
            impulseSource.GenerateImpulse();
            isDead = true;
        }

        if (col.CompareTag("Cauldron"))
        {
            contactCauldron = true;
        }
    }

    IEnumerator ChangeFace()
    {
        imageField.sprite = facesUI[1];
        yield return new WaitForSeconds(2);
        imageField.sprite = facesUI[0];
    }
}