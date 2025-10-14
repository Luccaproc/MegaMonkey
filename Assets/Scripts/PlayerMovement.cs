using System;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Layer Masks")]
    [SerializeField]private LayerMask groundLayer;

    [Header("Movement Variables")]
    [SerializeField] private float movementAcceleration = 70f;
    [SerializeField] private float maxMoveSpeed = 12f;
    [SerializeField] private float groundLinearDrag = 7f;
    //linearDrag é o que faz o personagem desacelerar antes de parar
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
    private float hangTimeCounter;
    private float jumpBufferCounter;
    private bool canJump => jumpBufferCounter > 0f && hangTimeCounter > 0f;

    [Header("Collision Variables")]
    [SerializeField] private float groundRaycastLenght;

    private CameraFollowOBJECT cameraFollowObject;

    [Header("Camera Variables")]
    [SerializeField] private GameObject cameraFollowGO;

    private bool onGround;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cameraFollowObject = cameraFollowGO.GetComponent<CameraFollowOBJECT>();
    }

    private void Update()
    {
        horizontalDirection = GetInput().x;
        //Atualiza onde o X do personagem tá
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferLength;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        anim.SetBool("isGrounded", onGround);
        anim.SetFloat("horizontalDirection", Mathf.Abs(horizontalDirection));
        if (horizontalDirection < 0f && facingRight)
        {
            Flip();
        }
        else if (horizontalDirection > 0f && !facingRight)
        { 
            Flip();
        }

        Debug.Log($"LinearVelocityX: {rb.linearVelocityX} | Damping: {rb.linearDamping}");

    }

    private void FixedUpdate()
    {
        CheckColissions();
        MoveCharacter();

        if (onGround)
        {
            ApplyGroundLinearDrag();
            hangTimeCounter = hangTime;
        }
        else
        {
            ApplyAirLinearDrag();
            FallMultiplier();
            hangTimeCounter -= Time.deltaTime;
        }
        if (canJump) Jump();
        if (horizontalDirection > 0f || horizontalDirection < 0f)
        {
            FlipCheck();
        }
        
    }

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //Retorna os vetores de cada input, WASD e setinhas
    }

    private void MoveCharacter()
    {
        rb.AddForce(new Vector2(horizontalDirection, 0f) * movementAcceleration);
        //Adiciona a força que o boneco vai mexer, influencia a velocidade
        if (Mathf.Abs(rb.linearVelocityX) > maxMoveSpeed)
            rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocityX) * maxMoveSpeed, rb.linearVelocityY);
        //Impede a velocidade de passar do maximo
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
    }

    private void FallMultiplier()
    {
        if (rb.linearVelocityY < 0)
        {
            rb.gravityScale = fallMultiplier;
        }
        else if (rb.linearVelocityY > 0 && !Input.GetButton("Jump"))
        {
            rb.gravityScale = lowJumpFallMultiplier;
        }
        else
        {
            rb.gravityScale = 1f;
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
        onGround = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastLenght, groundLayer);

        //Desenha o Raycast que verifica colisão com a layer do chão
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundRaycastLenght);
        //Desenha uma linha pra dar pra ver o Raycast
    }
}
