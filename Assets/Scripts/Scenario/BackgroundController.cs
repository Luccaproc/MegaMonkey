using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundController : MonoBehaviour
{
    [Header("Parallax Variables")]
    private float startPosition, length;
    public GameObject cam;
    [SerializeField] public float parallaxSpeed;

    // NOVO (Y)
    private float yOffset;
    public float smoothSpeed = 10f;

    void Start()
    {
        startPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        // Offset inicial do Y
        yOffset = transform.position.y - cam.transform.position.y;
    }

    private void FixedUpdate()
    {
        float distance = (cam.transform.position.x + 50) * parallaxSpeed;
        float movement = cam.transform.position.x * (1 - parallaxSpeed);

        // ===== Y (única mudança) =====
        float targetY = cam.transform.position.y + yOffset;

        float smoothY = Mathf.Lerp(
            transform.position.y,
            targetY,
            smoothSpeed * Time.deltaTime
        );
        // =============================

        transform.position = new Vector3(
            startPosition + distance, // X ORIGINAL
            smoothY,                  // só isso mudou
            transform.position.z
        );

        if (movement > startPosition + length)
        {
            startPosition += length; 
        }
        else if (movement < startPosition - length)
        {
            startPosition -= length;
        }
    }
}