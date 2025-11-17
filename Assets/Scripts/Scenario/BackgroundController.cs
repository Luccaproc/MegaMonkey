using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public Transform cam;
    public float parallaxEffect;

    private float startX;
    private float length;

    void Start()
    {
        startX = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void LateUpdate()
    {
        // Parallax movement
        float temp = cam.position.x * (1 - parallaxEffect);
        float dist = cam.position.x * parallaxEffect;

        transform.position = new Vector3(startX + dist, transform.position.y, transform.position.z);

        // Endless scroll
        if (temp > startX + length)  
        {
            startX += length;
        }
        else if (temp < startX - length)
        {
            startX -= length;
        }
    }
}
