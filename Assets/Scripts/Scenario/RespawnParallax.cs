using UnityEngine;

public class RespawnParallax : MonoBehaviour
{
    private ParallaxSwitcher parallaxSwitcher;

    private void Start()
    {
        parallaxSwitcher = FindFirstObjectByType<ParallaxSwitcher>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (parallaxSwitcher != null)
            {
                parallaxSwitcher.SwitchBackToA();
            }
        }
    }
}