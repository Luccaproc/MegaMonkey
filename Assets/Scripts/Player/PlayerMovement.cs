using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    public Animator Anim => anim;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Audio Sources")]
    public AudioSource jumpSound;

    [Header("Movement Inputs")]
    private PlayerControls playerControls;
    private Vector2 playerDirection;
    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
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
    //linearDrag é o que faz o personagem desacelerar antes de parar
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
    }

    private void Update()
    {
        PlayerInput();

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

        anim.SetBool("isGrounded", onGround);
        anim.SetFloat("horizontalDirection", Mathf.Abs(horizontalDirection));

        if (!isDashing)
        {
            if (horizontalDirection < 0f && facingRight)
            {
                Flip();
            }
            else if (horizontalDirection > 0f && !facingRight)
            {
                Flip();
            }
        }

        if (rb.linearVelocity.y < -0.1f && !onGround) // Usando uma pequena margem para evitar falsos positivos
        {
            //Animação de queda

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
            if (knockFromRight)
            {
                rb.linearVelocity = new Vector2(-knockBackForce, knockBackForce);
            }
            else
            {
                rb.linearVelocity = new Vector2(knockBackForce, knockBackForce);
            }
            knockBackCounter -= Time.deltaTime;
        }

        if (onGround)
        {
            ApplyGroundLinearDrag();
            hangTimeCounter = hangTime;

            //Animação
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
        if (canJump && !isDashing) Jump();
        if (horizontalDirection > 0f || horizontalDirection < 0f)
        {
            FlipCheck();
        }

    }

    private void MoveCharacter()
    {
        if (!isDashing)
        {
            rb.AddForce(new Vector2(horizontalDirection, 0f) * movementAcceleration);
            //Adiciona a força que o boneco vai mexer, influencia a velocidade
            if (Mathf.Abs(rb.linearVelocityX) > maxMoveSpeed)
            {
                rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocityX) * maxMoveSpeed, rb.linearVelocityY);
            }
            //Impede a velocidade de passar do maximo

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
        {
            rb.linearDamping = groundLinearDrag;
        }
        else
        {
            rb.linearDamping = 0;
        }
        //Aplica o Drag quando o personagem estiver no chão
    }

    private void ApplyAirLinearDrag()
    {
        rb.linearDamping = airLinearDrag;
        //Aplica o Drag quando o personagem estiver no ar
    }

    private void Jump()
    {
        ApplyAirLinearDrag();
        rb.linearVelocity = new Vector2(rb.linearVelocityX, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        hangTimeCounter = 0f;
        jumpBufferCounter = 0f;

        //Audio
        jumpSound.Play();

        //Animação
        anim.SetBool("isJumping", true);
        anim.SetBool("isFalling", false);
    }

    private void FallMultiplier()
    {
        if (rb.linearVelocityY < 0)
        {
            rb.gravityScale = fallMultiplier;
        }
        else if (rb.linearVelocityY > 0 && !playerControls.Player.Jump.IsPressed())
        {
            rb.gravityScale = lowJumpFallMultiplier;
        }
        else
        {
            rb.gravityScale = 1f;
        }
    }

    private async Awaitable Dash()
    {
        isDashing = true;
        float direction = facingRight ? 1f : -1f;

        //Animação
        anim.SetTrigger("Dash");
        anim.SetBool("isDashing", true);

        //Lembrar de colocar o audio aqui depois
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
        {
            canDash = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Dash"))
        {
            canDash = false;
        }
    }
    private void FlipCheck()
    {
        if (horizontalDirection < 0f && facingRight)
        {
            Flip();
        }
        else if (horizontalDirection > 0f && !facingRight)
        {
            Flip();
        }
    }
    void Flip()
    {
        if (facingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            facingRight = !facingRight;

            cameraFollowObject.CallTurn();
        }

        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            facingRight = !facingRight;

            cameraFollowObject.CallTurn();
        }
    }

    private void CheckColissions()
    {
        // Pega o collider pra medir largura
        float halfWidth = GetComponent<Collider2D>().bounds.extents.x;

        // Offset de 30% da largura para cada lado
        float offset = halfWidth * 0.3f;

        // Calcula posições dos pés
        Vector2 leftOrigin = new Vector2(transform.position.x - offset, transform.position.y);
        Vector2 rightOrigin = new Vector2(transform.position.x + offset, transform.position.y);

        // Raycasts para cada lado
        bool leftHitGround = Physics2D.Raycast(leftOrigin, Vector2.down, groundRaycastLenght, groundLayer);
        bool rightHitGround = Physics2D.Raycast(rightOrigin, Vector2.down, groundRaycastLenght, groundLayer);

        bool leftHitEnemy = Physics2D.Raycast(leftOrigin, Vector2.down, groundRaycastLenght, enemyLayer);
        bool rightHitEnemy = Physics2D.Raycast(rightOrigin, Vector2.down, groundRaycastLenght, enemyLayer);

        // O player está no chão se qualquer pé tocar
        onGround = leftHitGround || rightHitGround || leftHitEnemy || rightHitEnemy;

        //Desenha o Raycast que verifica colisão com a layer do chão
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundRaycastLenght);
        //Desenha uma linha pra dar pra ver o Raycast
    }
}
