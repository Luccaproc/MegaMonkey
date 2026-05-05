using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [Header("Parallax Variables")]
    private float startPosition, length;
    public GameObject cam;
    [SerializeField] public float parallaxSpeed;

    // Y
    private float yOffset;
    public float smoothSpeed = 10f;

    void Start()
    {
        startPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        // 🔥 alinha com a câmera no início
        Vector3 pos = transform.position;
        pos.y = cam.transform.position.y;
        transform.position = pos;

        // calcula offset depois (vai dar 0)
        yOffset = transform.position.y - cam.transform.position.y;
    }

    private void FixedUpdate()
    {
        float distance = cam.transform.position.x * parallaxSpeed;
        float movement = cam.transform.position.x * (1 - parallaxSpeed);

        float targetY = cam.transform.position.y + yOffset;

        float smoothY = Mathf.Lerp(
            transform.position.y,
            targetY,
            smoothSpeed * Time.deltaTime
        );

        transform.position = new Vector3(
            startPosition + distance,
            smoothY,
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