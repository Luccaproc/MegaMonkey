using UnityEngine;

public class GroundTrigger : MonoBehaviour
{
    [SerializeField] private ThompSmash enemy;
    

    void Awake()
    {
        enemy = GetComponentInParent<ThompSmash>();
    }
    void Start()
    {
        transform.SetParent(null);
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