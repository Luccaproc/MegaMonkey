using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage;
    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;

    public void Start()
{
    GameObject player = GameObject.FindWithTag("Player");

    if (player != null)
    {
        playerHealth = player.GetComponent<PlayerHealth>();
        playerMovement = player.GetComponent<PlayerMovement>();
    }
    else
    {
        Debug.LogWarning("Player não encontrado na cena!");
    }
}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerMovement.knockBackCounter = playerMovement.knockBackTotalTime;
            if (collision.transform.position.x <= transform.position.x)
            {
                playerMovement.knockFromRight = true;
            }
            if (collision.transform.position.x > transform.position.x)
            {
                playerMovement.knockFromRight = false;
            }
            playerHealth.TakeDamage(damage);
        }
    }
}
