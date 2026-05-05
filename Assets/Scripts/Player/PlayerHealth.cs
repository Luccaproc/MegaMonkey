using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int health;

    public AudioSource damageSound;
    public HealthUI healthUI;

    private bool isInvincible = false;
    public float invincibilityTime = 1f;

    void Start()
    {
        health = maxHealth;
        healthUI.UpdateHealth(health);
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        health -= damage;
        damageSound.Play();
        healthUI.UpdateHealth(health);

        if (health <= 0)
        {
            SceneManager.LoadSceneAsync(2);
            health = 3;
            healthUI.UpdateHealth(health);
        }

        StartCoroutine(InvincibilityCoroutine());
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }
}