using UnityEngine;

public class GroundTrigger : MonoBehaviour
{
    private ThompSmash enemy;

    void Start()
    {
        enemy = GetComponentInParent<ThompSmash>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemy.StartFalling();
            Debug.Log("Esbarrei com o player aqui");
        }
    }
}