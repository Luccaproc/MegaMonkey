using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    public int damage = 3;  // Quanto de dano vai causar

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se bateu no Player
        PlayerHealth player = collision.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }
}