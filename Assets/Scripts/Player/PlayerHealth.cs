using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int health;
    public AudioSource damageSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        damageSound.Play();
        if (health <= 0)
        {
            SceneManager.LoadSceneAsync(2);
        }
    }
}
