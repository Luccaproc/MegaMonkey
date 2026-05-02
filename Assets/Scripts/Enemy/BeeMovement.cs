using UnityEngine;

public class BeeMovement : MonoBehaviour
{
    [Header("Movement Points (Ordem base)")]
    public Transform finalPointRight;
    public Transform pointA;
    public Transform pointB;
    public Transform pointC;
    public Transform finalPointLeft;

    private Rigidbody2D rb;

    private Transform[] path;
    private int currentIndex = 0;
    private bool goingForward = true;

    public float speed = 2f;
    public float stoppingDistance = 0.3f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        path = new Transform[]
        {
            finalPointRight,
            pointA,
            pointB,
            pointC,
            finalPointLeft
        };

        currentIndex = 0;
    }

    void Update()
    {
        MoveToTarget();
        HandlePointSwitch();
    }

    void MoveToTarget()
    {
        Transform target = path[currentIndex];
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    void HandlePointSwitch()
    {
        Transform target = path[currentIndex];

        if (Vector2.Distance(transform.position, target.position) < stoppingDistance)
        {
            // 🔁 Flip SOMENTE ao chegar nos extremos
            if (target == finalPointLeft)
            {
                Flip(false); // esquerda
                goingForward = false;
            }
            else if (target == finalPointRight)
            {
                Flip(true); // direita
                goingForward = true;
            }

            // ↔️ Anda no caminho (ida ou volta)
            if (goingForward)
                currentIndex++;
            else
                currentIndex--;

            // Clamp de segurança
            currentIndex = Mathf.Clamp(currentIndex, 0, path.Length - 1);
        }
    }

    void Flip(bool facingRight)
    {
        if (facingRight)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    private void OnDrawGizmos()
    {
        if (finalPointRight == null || pointA == null || pointB == null || pointC == null || finalPointLeft == null)
            return;

        Transform[] gizmoPath = new Transform[]
        {
            finalPointRight,
            pointA,
            pointB,
            pointC,
            finalPointLeft
        };

        for (int i = 0; i < gizmoPath.Length; i++)
        {
            Gizmos.DrawWireSphere(gizmoPath[i].position, 0.3f);

            if (i < gizmoPath.Length - 1)
            {
                Gizmos.DrawLine(gizmoPath[i].position, gizmoPath[i + 1].position);
            }
        }
    }
}