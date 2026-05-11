using UnityEngine;
using UnityEngine.SceneManagement;

public class Banana : MonoBehaviour
{
    private Animator anim;
    public ParallaxSwitcher parallaxSwitcher;

    private void Start()
    {
        anim = GetComponent<Animator>(); 
        ParallaxSwitcher parallaxSwitcher = GetComponent<ParallaxSwitcher>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("isBanana");
        }
    }

     public void GoToMenu()
    {
        SceneManager.LoadScene(0);
        parallaxSwitcher.ActivateSetA();
    }
}
