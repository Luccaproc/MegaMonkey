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

    [Header("Invulnerability")]
    public float invincibilityTime = 1f;

    [SerializeField] private int playerLayer = 3;
    [SerializeField] private int enemyLayer = 7;

    private bool isInvulnerable = false;

    void Start()
    {
        health = maxHealth;
        healthUI.UpdateHealth(health);
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        health -= damage;

        damageSound.Play();
        healthUI.UpdateHealth(health);

        if (health <= 0)
        {
            SceneManager.LoadSceneAsync(2);
        }

        StartCoroutine(TemporaryCollisionIgnore());
    }

    private IEnumerator TemporaryCollisionIgnore()
    {
        isInvulnerable = true;

        // 🔥 ignora colisão Player <-> Enemy
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);
        Debug.Log("Ignorar ON");

        yield return new WaitForSeconds(invincibilityTime);

        // 🔥 volta colisão
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        Debug.Log("Ignorar OFF");

        isInvulnerable = false;
    }
}