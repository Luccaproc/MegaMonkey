using UnityEngine;

[DefaultExecutionOrder(-1000)] // 🔥 garante que roda antes de tudo
public class SpawnComponent : MonoBehaviour
{
    private void Awake()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("Player não encontrado!");
            return;
        }

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.position = transform.position;
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            player.transform.position = transform.position;
        }
    }
}