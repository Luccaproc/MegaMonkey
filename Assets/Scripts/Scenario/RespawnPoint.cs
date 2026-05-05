using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public bool isStartingPoint = false;

    void Start()
    {
        if (isStartingPoint)
        {
            if (RespawnManager.savedRespawnPosition == null)
            {
                RespawnManager.Instance.SetRespawnPoint(transform);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            RespawnManager.Instance.SetRespawnPoint(transform);
        }
    }
}