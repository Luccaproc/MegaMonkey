using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int health;

    public AudioSource damageSound;

    public HealthUI healthUI;

    void Start()
    {
        health = maxHealth;

        // Atualiza HUD ao iniciar
        healthUI.UpdateHealth(health);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        damageSound.Play();

        // Atualiza HUD ao tomar dano
        healthUI.UpdateHealth(health);

        if (health <= 0)
        {
            SceneManager.LoadSceneAsync(2);
        }
    }
}