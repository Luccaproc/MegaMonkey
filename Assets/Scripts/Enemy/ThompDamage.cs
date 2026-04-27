using UnityEngine;

public class ThompDamage : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Esbarrei com o player aqui");
        if (collision.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerMovement != null)
            {
                // Direção do knockback
                playerMovement.knockBackCounter = playerMovement.knockBackTotalTime;

                if (collision.transform.position.x <= transform.position.x)
                {
                    playerMovement.knockFromRight = true;
                }
                else
                {
                    playerMovement.knockFromRight = false;
                }
            }

            if (playerHealth != null)
            {
                // Aplica dano
                playerHealth.TakeDamage(damage);
            }
        }
    }
}