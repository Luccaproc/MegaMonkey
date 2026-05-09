using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public bool isStartingPoint = false;

    [Header("Respawn Reference")]
    [SerializeField] private Transform respawnPoint;

    void Start()
    {
        if (isStartingPoint)
        {
            if (RespawnManager.savedRespawnPosition == null)
            {
                RespawnManager.Instance.SetRespawnPoint(respawnPoint);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            RespawnManager.Instance.SetRespawnPoint(respawnPoint);
        }
    }
}