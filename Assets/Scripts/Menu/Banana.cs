using UnityEngine;
using UnityEngine.SceneManagement;

public class Banana : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>(); 
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
    }
}
