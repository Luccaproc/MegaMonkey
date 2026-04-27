using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] public int maxHealth;
    [SerializeField] private ParticleSystem damageParticles;

    private Transform player;

    public int health;
    private ParticleSystem damageParticlesInstance;

    void Start()
    {
        health = maxHealth;

        // pega o player (assumindo que tem tag "Player")
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        SpawnDamageParticles();

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void SpawnDamageParticles()
    {
        if (player == null) return;

        // direção do player em relação ao inimigo
        float direction = transform.position.x - player.position.x;

        // rotação baseada na direção
        Quaternion rotation = direction >= 0 
            ? Quaternion.identity              // direita
            : Quaternion.Euler(0, 180, 0);     // esquerda

        damageParticlesInstance = Instantiate(
            damageParticles, 
            transform.position, 
            rotation
        );
    }
}