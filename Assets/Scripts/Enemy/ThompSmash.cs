using UnityEngine;

public class ThompSmash : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    public Animator Anim => anim;

    public float fallGravity = 2f;
    public float riseSpeed = 3f;

    private bool isFalling = false;
    private bool isRising = false;

    private Vector3 startPosition;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        startPosition = transform.position;
    }

    void Update()
    {
        if (isRising)
        {
            anim.SetTrigger("isRising");
            float distance = Vector2.Distance(transform.position, startPosition);

            if (distance < 0.1f)
            {
                transform.position = startPosition;
                rb.linearVelocity = Vector2.zero;
                isRising = false;
                rb.gravityScale = 0;
            }
            else
            {
                rb.linearVelocity = new Vector2(0, riseSpeed);
            }
        }
    }

    public void StartFalling()
    {
        if (!isFalling && !isRising)
        {
            rb.gravityScale = fallGravity;
            isFalling = true;
            anim.SetTrigger("isFalling");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Ground"))
    {
        isFalling = false;
        isRising = true;
        rb.gravityScale = 0;
    }
}
}