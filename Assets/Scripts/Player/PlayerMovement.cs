using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float timer;
    private int frameCount;

    [Header("Components")]
    private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    public Animator Anim => anim;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Jump Delay")]
    [SerializeField] private float jumpCollisionDelay = 0.2f;
    private float jumpDelayCounter;

    [Header("Audio Sources")]
    public AudioSource jumpSound;

    [Header("Movement Inputs")]
    private PlayerControls playerControls;
    private Vector2 playerDirection;

    private void OnEnable() => playerControls.Enable();
    private void OnDisable() => playerControls.Disable();

    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void PlayerInput()
    {
        playerDirection = playerControls.Player.Move.ReadValue<Vector2>();
    }

    [Header("Movement Variables")]
    [SerializeField] private float movementAcceleration = 70f;
    [SerializeField] private float maxMoveSpeed = 12f;
    [SerializeField] private float groundLinearDrag = 7f;
    public bool isRunning;
    private float horizontalDirection;
    private bool changingDirection => (rb.linearVelocityX > 0f && horizontalDirection < 0f) || (rb.linearVelocityX < 0f && horizontalDirection > 0f);
    public bool facingRight = true;

    [Header("Jump Variables")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float airLinearDrag = 2.5f;
    [SerializeField] private float fallMultiplier = 8f;
    [SerializeField] private float lowJumpFallMultiplier = 5f;
    [SerializeField] private float hangTime = .1f;
    [SerializeField] private float jumpBufferLength = .1f;
    public bool onGround;
    private float hangTimeCounter;
    private float jumpBufferCounter;
    private bool canJump => jumpBufferCounter > 0f && hangTimeCounter > 0f;

    [Header("Dash Variables")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    private bool canDash;
    private bool isDashing;

    [Header("Collision Variables")]
    [SerializeField] private float groundRaycastLenght;
    public Transform leftFoot;
    public Transform rightFoot;

    [Header("Camera Variables")]
    private CameraFollowOBJECT cameraFollowObject;
    [SerializeField] private GameObject cameraFollowGO;
    [SerializeField] private CameraManager cameraManager;
    private float fallSpeedYDampingChangeThreshold;

    [Header("Knockback Variables")]
    public float knockBackForce;
    public float knockBackCounter;
    public float knockBackTotalTime;
    public bool knockFromRight;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cameraFollowObject = cameraFollowGO.GetComponent<CameraFollowOBJECT>();
        fallSpeedYDampingChangeThreshold = cameraManager.fallSpeedYDampingChangeThreshold;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        frameCount++;

        if (timer >= 1f)
        {
            float fps = frameCount / timer;
            Debug.Log("FPS médio: " + Mathf.Round(fps));
            timer = 0f;
            frameCount = 0;
        }

        PlayerInput();

        // ⏳ decrementa o delay do pulo
        jumpDelayCounter -= Time.deltaTime;

        if (playerControls.Player.Menu.WasPressedThisFrame())
        {
            Application.Quit();
        }

        horizontalDirection = playerDirection.x;

        if (playerControls.Player.Jump.IsPressed() && !isDashing)
        {
            jumpBufferCounter = jumpBufferLength;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (playerControls.Player.Dash.WasPressedThisFrame())
        {
            _ = HandleDash();
        }

        float yVel = rb.linearVelocityY;

        if (yVel < -2f)
            cameraManager.SetFalling(true);
        else if (yVel > 2f)
            cameraManager.SetFalling(false);

        anim.SetBool("isGrounded", onGround);
        anim.SetFloat("horizontalDirection", Mathf.Abs(horizontalDirection));

        if (!isDashing)
        {
            if (horizontalDirection < 0f && facingRight)
                Flip();
            else if (horizontalDirection > 0f && !facingRight)
                Flip();
        }

        if (rb.linearVelocity.y < -0.1f && !onGround)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", true);
        }
    }

    private void FixedUpdate()
    {
        CheckColissions();

        if (isDashing)
        {
            float direction = facingRight ? 1f : -1f;
            Vector2 dashDirection = new Vector2(direction, 1f).normalized;
            rb.linearVelocity = dashDirection * dashSpeed;
            return;
        }

        if (knockBackCounter <= 0)
        {
            MoveCharacter();
        }
        else
        {
            rb.linearVelocity = knockFromRight
                ? new Vector2(-knockBackForce, knockBackForce)
                : new Vector2(knockBackForce, knockBackForce);

            knockBackCounter -= Time.deltaTime;
        }

        if (onGround)
        {
            ApplyGroundLinearDrag();
            hangTimeCounter = hangTime;

            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
        }
        else
        {
            if (!isDashing)
            {
                ApplyAirLinearDrag();
                FallMultiplier();
            }

            hangTimeCounter -= Time.deltaTime;
        }

        // 🚫 bloqueio de pulo com delay
        if (canJump && !isDashing && jumpDelayCounter <= 0f)
        {
            Jump();
        }

        if (horizontalDirection != 0f)
        {
            FlipCheck();
        }
    }

    private void MoveCharacter()
    {
        if (!isDashing)
        {
            rb.AddForce(new Vector2(horizontalDirection, 0f) * movementAcceleration);

            if (Mathf.Abs(rb.linearVelocityX) > maxMoveSpeed)
            {
                rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocityX) * maxMoveSpeed, rb.linearVelocityY);
            }

            if (Mathf.Abs(horizontalDirection) > 0.1f && onGround)
            {
                anim.SetBool("isRunning", true);
                isRunning = true;
            }
            else
            {
                anim.SetBool("isRunning", false);
                isRunning = false;
            }
        }
    }

    private void ApplyGroundLinearDrag()
    {
        if (Math.Abs(horizontalDirection) < 0.4f || changingDirection)
            rb.linearDamping = groundLinearDrag;
        else
            rb.linearDamping = 0;
    }

    private void ApplyAirLinearDrag()
    {
        rb.linearDamping = airLinearDrag;
    }

    private void Jump()
    {
        ApplyAirLinearDrag();
        rb.linearVelocity = new Vector2(rb.linearVelocityX, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        hangTimeCounter = 0f;
        jumpBufferCounter = 0f;

        jumpSound.Play();

        anim.SetBool("isJumping", true);
        anim.SetBool("isFalling", false);
    }

    private void FallMultiplier()
    {
        if (rb.linearVelocityY < 0)
            rb.gravityScale = fallMultiplier;
        else if (rb.linearVelocityY > 0 && !playerControls.Player.Jump.IsPressed())
            rb.gravityScale = lowJumpFallMultiplier;
        else
            rb.gravityScale = 1f;
    }

    private async Awaitable Dash()
    {
        isDashing = true;
        float direction = facingRight ? 1f : -1f;

        anim.SetTrigger("Dash");
        anim.SetBool("isDashing", true);

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;
        rb.linearVelocity = new Vector2(dashSpeed * direction, dashSpeed);

        await Awaitable.WaitForSecondsAsync(dashDuration);

        isDashing = false;
        anim.SetBool("isDashing", false);
        rb.gravityScale = originalGravity;
    }

    private async Awaitable HandleDash()
    {
        if (isDashing || !canDash) return;
        await Dash();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Dash"))
            canDash = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Dash"))
            canDash = false;
    }

    // 💥 DETECÇÃO DE COLISÃO GLOBAL
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            jumpDelayCounter = jumpCollisionDelay;
        }
    }

    private void FlipCheck()
    {
        if (horizontalDirection < 0f && facingRight)
            Flip();
        else if (horizontalDirection > 0f && !facingRight)
            Flip();
    }

    void Flip()
    {
        if (facingRight)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        facingRight = !facingRight;
        cameraFollowObject.CallTurn();
    }

    private void CheckColissions()
    {
        float halfWidth = GetComponent<Collider2D>().bounds.extents.x;
        float offset = halfWidth * 0.3f;

        Vector2 leftOrigin = new Vector2(transform.position.x - offset, transform.position.y);
        Vector2 rightOrigin = new Vector2(transform.position.x + offset, transform.position.y);

        bool leftHitGround = Physics2D.Raycast(leftOrigin, Vector2.down, groundRaycastLenght, groundLayer);
        bool rightHitGround = Physics2D.Raycast(rightOrigin, Vector2.down, groundRaycastLenght, groundLayer);

        bool leftHitEnemy = Physics2D.Raycast(leftOrigin, Vector2.down, groundRaycastLenght, enemyLayer);
        bool rightHitEnemy = Physics2D.Raycast(rightOrigin, Vector2.down, groundRaycastLenght, enemyLayer);

        onGround = leftHitGround || rightHitGround || leftHitEnemy || rightHitEnemy;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundRaycastLenght);
    }
}